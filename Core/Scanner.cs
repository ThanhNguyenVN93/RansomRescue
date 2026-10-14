using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.Win32;
using frm_ransomrescure.Models;
using frm_ransomrescure.Utils;

namespace frm_ransomrescure.Core
{
    /// <summary>
    /// Quét hệ thống tìm bằng chứng ransomware.
    ///
    /// Thu thập 2 loại bằng chứng:
    ///   A. File có phần mở rộng khớp db.json hoặc không nằm trong whitelist
    ///   B. File ghi chú tiền chuộc (ransom note) — tên file đặc trưng
    ///
    /// Hiệu năng: 2 luồng, BelowNormal priority — nhường CPU cho hệ thống.
    /// </summary>
    public class Scanner
    {
        // ── Tham số quét ────────────────────────────────────────────────────────
        private readonly List<string>       _rootDrives;       // Ổ đĩa cần quét
        private readonly List<RansomFamily> _families;         // DB ransomware
        private readonly bool               _deepScan;         // true = quét sâu

        // ── Index nhanh để tra cứu ───────────────────────────────────────────
        private readonly HashSet<string> _knownExtensions;     // tất cả .ext từ db.json
        private readonly HashSet<string> _knownNoteNames;      // tất cả tên file note
        private readonly HashSet<string> _commonExtWhitelist;  // ext lành → skip nếu không deep

        // ── State quét ──────────────────────────────────────────────────────────
        private volatile bool _shouldStop = false;
        private long   _totalScanned = 0;
        private int    _suspiciousCount = 0;
        private int    _noteCount = 0;
        private DateTime _earliestSuspiciousTime = DateTime.MaxValue;

        // ── Events (gọi trên worker thread) ─────────────────────────────────────
        /// <summary>Gọi mỗi khi progress thay đổi: (filesScanned, currentPath)</summary>
        public event Action<long, string> ProgressChanged;

        /// <summary>Gọi khi tìm thấy ransom note (để append vào log panel)</summary>
        public event Action<string> RansomNoteFound;

        /// <summary>Gọi khi quét xong hoàn toàn</summary>
        public event Action<ScanResult> ScanCompleted;

        // ── Constructor ─────────────────────────────────────────────────────────

        public Scanner(List<string> rootDrives, List<RansomFamily> families, bool deepScan)
        {
            _rootDrives = rootDrives ?? throw new ArgumentNullException(nameof(rootDrives));
            _families   = families   ?? throw new ArgumentNullException(nameof(families));
            _deepScan   = deepScan;

            // Build lookup indexes
            _knownExtensions = BuildExtensionSet(families);
            _knownNoteNames  = BuildNoteNameSet(families);
            _commonExtWhitelist = BuildWhitelist();
        }

        // ── Public API ──────────────────────────────────────────────────────────

        /// <summary>Bắt đầu quét bất đồng bộ (chạy trên Thread pool)</summary>
        public void StartAsync()
        {
            _shouldStop = false;
            var thread = new Thread(RunScan)
            {
                Priority    = ThreadPriority.Lowest,   // Nhường CPU cho hệ thống
                IsBackground = true,
                Name        = "RR-ScanMain"
            };

            // Đặt process priority thấp để không làm chậm máy user
            try { Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.BelowNormal; }
            catch { /* Không có quyền — bỏ qua */ }

            thread.Start();
        }

        /// <summary>Yêu cầu dừng quét (graceful — hoàn thành file hiện tại rồi dừng)</summary>
        public void Stop() => _shouldStop = true;

        // ── Main scan logic ──────────────────────────────────────────────────────

        private void RunScan()
        {
            var result   = new ScanResult { IsDeepScan = _deepScan };
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Chia ổ đĩa cho 2 luồng nếu có nhiều hơn 1 ổ
                if (_rootDrives.Count == 1)
                {
                    ScanDrive(_rootDrives[0], result);
                }
                else
                {
                    // Luồng 1: ổ đầu tiên; Luồng 2: các ổ còn lại (song song)
                    var drive1   = _rootDrives[0];
                    var drives2  = _rootDrives.GetRange(1, _rootDrives.Count - 1);

                    var t1 = new Thread(() => ScanDrive(drive1, result))
                             { Priority = ThreadPriority.Lowest, IsBackground = true, Name = "RR-Thread1" };
                    var t2 = new Thread(() => { foreach (var d in drives2) ScanDrive(d, result); })
                             { Priority = ThreadPriority.Lowest, IsBackground = true, Name = "RR-Thread2" };

                    t1.Start();
                    t2.Start();
                    t1.Join();
                    t2.Join();
                }

                result.IsComplete          = !_shouldStop;
                result.TotalFilesScanned   = _totalScanned;
                result.SuspiciousFileCount = _suspiciousCount;
                result.ScanDurationSeconds = stopwatch.Elapsed.TotalSeconds;

                if (_earliestSuspiciousTime != DateTime.MaxValue)
                    result.EstimatedInfectionTime = _earliestSuspiciousTime;

                // Registry scan (nhanh, chạy trên thread chính của scan)
                ScanRegistry(result);
            }
            catch (Exception ex)
            {
                Logger.Error("Scanner.RunScan", ex);
                result.IsComplete = false;
            }
            finally
            {
                stopwatch.Stop();
                // Khôi phục process priority
                try { Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal; }
                catch { }

                ScanCompleted?.Invoke(result);
            }
        }

        private void ScanDrive(string root, ScanResult result)
        {
            ScanDirectory(root, result);
        }

        private void ScanDirectory(string dirPath, ScanResult result)
        {
            if (_shouldStop) return;

            // ── Quyết định có skip không ───────────────────────────────────────
            if (PathHelper.ShouldSkipDirectory(dirPath)) return;

            bool noteOnly = PathHelper.IsNoteOnlyDirectory(dirPath);

            // ── Quét files trong thư mục này ───────────────────────────────────
            try
            {
                string[] files = Directory.GetFiles(dirPath);
                foreach (string filePath in files)
                {
                    if (_shouldStop) return;
                    ProcessFile(filePath, noteOnly, result);
                }
            }
            catch (UnauthorizedAccessException) { /* Bỏ qua thư mục bị denied */ }
            catch (Exception ex) { Logger.Warn($"Scanner.GetFiles({dirPath}): {ex.Message}"); }

            // ── Đệ quy vào thư mục con ─────────────────────────────────────────
            try
            {
                string[] subDirs = Directory.GetDirectories(dirPath);
                foreach (string subDir in subDirs)
                {
                    if (_shouldStop) return;
                    ScanDirectory(subDir, result);
                }
            }
            catch (UnauthorizedAccessException) { }
            catch (Exception ex) { Logger.Warn($"Scanner.GetDirectories({dirPath}): {ex.Message}"); }
        }

        private void ProcessFile(string filePath, bool noteOnly, ScanResult result)
        {
            try
            {
                string fileName = Path.GetFileName(filePath);
                string ext      = Path.GetExtension(filePath);

                // Skip file hệ thống không liên quan
                if (PathHelper.ShouldSkipFile(filePath)) return;

                Interlocked.Increment(ref _totalScanned);

                // Cập nhật progress (không cần lock vì chỉ dùng để hiển thị)
                ProgressChanged?.Invoke(_totalScanned, filePath);

                // ── Bằng chứng B: Ransom note (tên file) ──────────────────────
                if (_knownNoteNames.Contains(fileName))
                {
                    // Đọc nội dung note (tối đa 4KB để tránh tốn RAM)
                    string content = ReadNoteContent(filePath, maxBytes: 4096);

                    lock (result.RansomNotePaths)
                    {
                        result.RansomNotePaths.Add(filePath);
                        if (!string.IsNullOrEmpty(content))
                            result.RansomNoteContents[filePath] = content;
                    }

                    Interlocked.Increment(ref _noteCount);
                    RansomNoteFound?.Invoke(filePath);
                    return; // Đã là ransom note → không cần check ext nữa
                }

                // ── noteOnly mode: chỉ quét note, bỏ qua extension ────────────
                if (noteOnly) return;

                // ── Bằng chứng A: Phần mở rộng ───────────────────────────────
                if (string.IsNullOrEmpty(ext)) return;

                bool isSuspicious = false;

                // Quét thường: khớp với db.json là primary
                if (_knownExtensions.Contains(ext))
                {
                    isSuspicious = true;
                }
                // Quét sâu: thêm cả ext không nằm trong whitelist
                else if (_deepScan && !_commonExtWhitelist.Contains(ext))
                {
                    isSuspicious = true;
                }

                if (isSuspicious)
                {
                    lock (result.ExtensionFrequency)
                    {
                        if (result.ExtensionFrequency.ContainsKey(ext))
                            result.ExtensionFrequency[ext]++;
                        else
                            result.ExtensionFrequency[ext] = 1;
                    }
                    Interlocked.Increment(ref _suspiciousCount);

                    // Thu thập magic bytes và ước tính thời gian nhiễm
                    try
                    {
                        var info = new FileInfo(filePath);
                        // Track earliest mtime của file bị mã hóa
                        if (info.LastWriteTime < _earliestSuspiciousTime)
                        {
                            lock (this) { if (info.LastWriteTime < _earliestSuspiciousTime) _earliestSuspiciousTime = info.LastWriteTime; }
                        }

                        // Đọc magic bytes (8 byte đầu) — chỉ lưu mẫu đầu tiên của mỗi ext
                        lock (result.MagicByteEvidence)
                        {
                            if (!result.MagicByteEvidence.ContainsValue(ext))
                            {
                                string magic = ReadMagicBytes(filePath);
                                if (!string.IsNullOrEmpty(magic))
                                    result.MagicByteEvidence[magic] = ext;
                            }
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Logger.Warn($"Scanner.ProcessFile({filePath}): {ex.Message}");
            }
        }

        // ── Helpers ─────────────────────────────────────────────────────────────

        private static string ReadNoteContent(string filePath, int maxBytes)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    byte[] buffer = new byte[Math.Min(maxBytes, fs.Length)];
                    int    read   = fs.Read(buffer, 0, buffer.Length);
                    return System.Text.Encoding.UTF8.GetString(buffer, 0, read);
                }
            }
            catch { return string.Empty; }
        }

        private static HashSet<string> BuildExtensionSet(List<RansomFamily> families)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var f in families)
                foreach (var ext in f.Extensions)
                    set.Add(ext);
            return set;
        }

        private static HashSet<string> BuildNoteNameSet(List<RansomFamily> families)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var f in families)
                foreach (var note in f.RansomNotes)
                    set.Add(note);
            return set;
        }

        /// <summary>
        /// Extension phổ biến, lành mạnh — skip khi không phải deep scan.
        /// Càng dài càng tốt để giảm false positive.
        /// </summary>
        private static string ReadMagicBytes(string filePath)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    if (fs.Length < 4) return null;
                    byte[] buf = new byte[Math.Min(8, fs.Length)];
                    fs.Read(buf, 0, buf.Length);
                    return BitConverter.ToString(buf).Replace("-", "").ToUpperInvariant();
                }
            }
            catch { return null; }
        }

        private static void ScanRegistry(ScanResult result)
        {
            // Kiểm tra HKCU\Run — ransomware thường add persistence ở đây
            ScanRunKey(result,
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                RegistryHive.CurrentUser, "HKCU\\Run");

            // Kiểm tra HKLM\Run
            ScanRunKey(result,
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                RegistryHive.LocalMachine, "HKLM\\Run");

            // Kiểm tra wallpaper (ransomware thường đổi thành ransom note image)
            CheckWallpaper(result);

            // Kiểm tra shadow copies bị xóa (thông qua Volume Shadow Copy Service)
            CheckShadowCopies(result);
        }

        private static void ScanRunKey(ScanResult result, string subKey, RegistryHive hive, string label)
        {
            try
            {
                using (var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
                using (var key = baseKey.OpenSubKey(subKey, writable: false))
                {
                    if (key == null) return;
                    foreach (string name in key.GetValueNames())
                    {
                        string val = key.GetValue(name)?.ToString() ?? "";
                        // Cờ đỏ: trỏ đến %TEMP%, %APPDATA%, hoặc tên file ngẫu nhiên
                        if (IsSuspiciousRunEntry(val))
                        {
                            lock (result.RegistryFindings)
                                result.RegistryFindings.Add(
                                    $"[{label}] {name} → {TruncatePath(val, 80)}");
                        }
                    }
                }
            }
            catch { /* Không có quyền hoặc key không tồn tại */ }
        }

        private static bool IsSuspiciousRunEntry(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            string lower = value.ToLowerInvariant();
            // Các vị trí ransomware thường dùng để persistence
            return lower.Contains("\\temp\\")
                || lower.Contains("\\appdata\\roaming\\")
                || lower.Contains("\\appdata\\local\\temp\\")
                || lower.Contains("\\users\\public\\")
                || (lower.EndsWith(".exe") && lower.Contains("\\appdata\\"));
        }

        private static void CheckWallpaper(ScanResult result)
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", writable: false))
                {
                    if (key == null) return;
                    string wallpaper = key.GetValue("Wallpaper")?.ToString() ?? "";
                    if (string.IsNullOrEmpty(wallpaper)) return;

                    // Cờ đỏ: wallpaper trỏ đến file trong %TEMP% hoặc có tên liên quan ransom
                    string lower = wallpaper.ToLowerInvariant();
                    if (lower.Contains("\\temp\\") || lower.Contains("ransom")
                        || lower.Contains("decrypt") || lower.Contains("readme")
                        || lower.Contains("recover") || lower.Contains("restore"))
                    {
                        lock (result.RegistryFindings)
                            result.RegistryFindings.Add(
                                $"[Wallpaper] Ảnh nền đã bị thay đổi → {TruncatePath(wallpaper, 80)}");
                    }
                }
            }
            catch { }
        }

        private static void CheckShadowCopies(ScanResult result)
        {
            try
            {
                // Dùng WMI để kiểm tra xem còn shadow copy không
                using (var searcher = new System.Management.ManagementObjectSearcher(
                    "SELECT * FROM Win32_ShadowCopy"))
                using (var shadows = searcher.Get())
                {
                    int count = shadows.Count;
                    if (count == 0)
                    {
                        lock (result.RegistryFindings)
                            result.RegistryFindings.Add(
                                "[Shadow Copy] Không có bản sao ẩn (Shadow Copy) nào — ransomware có thể đã xóa.");
                    }
                }
            }
            catch { /* WMI không có hoặc không có quyền — bỏ qua */ }
        }

        private static string TruncatePath(string path, int maxLen)
        {
            if (string.IsNullOrEmpty(path) || path.Length <= maxLen) return path;
            return "..." + path.Substring(path.Length - (maxLen - 3));
        }

        private static HashSet<string> BuildWhitelist()
        {
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                // Documents
                ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx",
                ".pdf", ".txt", ".rtf", ".odt", ".ods", ".odp",
                // Images
                ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp", ".svg",
                // Audio/Video
                ".mp3", ".mp4", ".avi", ".mkv", ".mov", ".flac", ".wav", ".aac",
                // Archives
                ".zip", ".rar", ".7z", ".tar", ".gz",
                // Code/Data
                ".html", ".htm", ".css", ".js", ".json", ".xml", ".csv",
                ".py", ".cs", ".java", ".cpp", ".h", ".php", ".sql",
                // System (không mã hóa được)
                ".exe", ".dll", ".sys", ".lnk", ".msi", ".bat", ".cmd",
                // Misc lành
                ".ico", ".cur", ".ttf", ".otf", ".woff", ".woff2",
                ".db", ".sqlite", ".mdb",
            };
        }
    }
}
