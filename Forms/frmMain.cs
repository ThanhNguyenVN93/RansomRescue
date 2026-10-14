using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using frm_ransomrescure.Core;
using frm_ransomrescure.Models;
using frm_ransomrescure.Utils;

namespace frm_ransomrescure.Forms
{
    /// <summary>
    /// Form chính của RansomRescue.
    /// 3 tab: Tab0 = Cảnh báo, Tab1 = Quét, Tab2 = Kết quả
    /// </summary>
    public partial class frmMain : Form
    {
        // ── State ────────────────────────────────────────────────────────────────
        private List<RansomFamily>      _families;
        private Scanner                 _scanner;
        private ScanResult              _scanResult;
        private IdentifyResult          _identifyResult;
        private bool                    _isScanning   = false;
        private bool                    _isDarkMode   = false;
        private bool                    _isEnglish    = false;
        private CancellationTokenSource _kvrtCts;
        private System.Windows.Forms.Timer _dbRefreshTimer;
        private string                  _kvrtSavePath = System.IO.Path.Combine(
                                            System.IO.Path.GetTempPath(), "kvrt.exe");

        // ── Constructor ──────────────────────────────────────────────────────────
        public frmMain()
        {
            InitializeComponent();
            this.Text = "RansomRescue — Công cụ phục hồi Ransomware";
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            // Tab 0 luôn hiện trước. Dùng Selecting event để chặn chuyển tab khi chưa confirm.
            tabMain.SelectedIndex = 0;

            // Cập nhật uptime warning ngay khi load
            LoadUptimeWarning();
        }

        // ── Form Load ────────────────────────────────────────────────────────────

        private async void frmMain_Load(object sender, EventArgs e)
        {
            // Tooltips
            SetupTooltips();

            // Populate drive checkboxes
            PopulateDriveCheckboxes();

            // SHA-256 self-check — hiển thị hash để user xác minh nếu cần
            string selfHash = ComputeSelfHash();
            Logger.Info($"Self SHA-256: {selfHash}");

            // Tải DB nền (không block UI)
            lblDbStatus.Text = "Đang tải cơ sở dữ liệu...";
            try
            {
                _families = await DbLoader.LoadAsync();
                lblDbStatus.Text = $"CSDL: {DbLoader.LastSource} | SHA256: {selfHash.Substring(0, 8)}…";
                Logger.Info($"DB loaded: {_families.Count} families");

                // Background refresh timer — check mỗi 6 giờ xem DB có cũ không
                StartDbRefreshTimer();
            }
            catch (Exception ex)
            {
                lblDbStatus.Text = "CSDL: Lỗi tải — dùng dữ liệu nội bộ";
                Logger.Error("frmMain: DB load failed", ex);
                _families = new List<RansomFamily>();
            }
        }

        private static string ComputeSelfHash()
        {
            try
            {
                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                using (var sha = System.Security.Cryptography.SHA256.Create())
                using (var fs  = File.OpenRead(exePath))
                {
                    byte[] hash = sha.ComputeHash(fs);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
            catch { return "unavailable"; }
        }

        private void StartDbRefreshTimer()
        {
            int intervalHours = 6;
            _dbRefreshTimer = new System.Windows.Forms.Timer
            {
                Interval = intervalHours * 60 * 60 * 1000
            };
            _dbRefreshTimer.Tick += async (s, ev) =>
            {
                int maxAgeDays = int.TryParse(
                    System.Configuration.ConfigurationManager.AppSettings["DbAutoRefreshDays"],
                    out int d) ? d : 7;

                string cachePath = Utils.PathHelper.GetCachePath();
                bool isStale = !File.Exists(cachePath)
                    || (DateTime.Now - File.GetLastWriteTime(cachePath)).TotalDays > maxAgeDays;

                if (!isStale) return;

                Logger.Info("Auto-refreshing DB (cache stale > 7 days)...");
                lblDbStatus.Text = "CSDL: Đang tự động làm mới...";
                try
                {
                    _families = await DbLoader.LoadAsync();
                    lblDbStatus.Text = $"CSDL: {DbLoader.LastSource} (tự động làm mới)";
                    Logger.Info($"Auto-refresh OK: {_families.Count} families");
                }
                catch (Exception ex)
                {
                    Logger.Error("Auto-refresh DB failed", ex);
                    lblDbStatus.Text = "CSDL: Làm mới thất bại";
                }
            };
            _dbRefreshTimer.Start();
        }

        private void SetupTooltips()
        {
            toolTip1.SetToolTip(chkDownloadKvrt,
                "Tải Kaspersky Virus Removal Tool (~150 MB) ngầm trong lúc quét.\nSau khi tải xong, nút 'Chạy KVRT' sẽ hiện ở Tab Kết quả.");
            toolTip1.SetToolTip(chkConfirm,
                "Xác nhận bạn đã đọc các cảnh báo quan trọng trước khi tiếp tục.");
            toolTip1.SetToolTip(btnStartScan,
                "Chuyển sang Tab Quét để bắt đầu phân tích hệ thống.");
            toolTip1.SetToolTip(chkDeepScan,
                "Quét sâu hơn: kiểm tra tất cả phần mở rộng không phổ biến,\nkhông chỉ những ext đã biết trong CSDL. Chậm hơn ~3-5 lần.");
            toolTip1.SetToolTip(btnScan,
                "Bắt đầu quét hệ thống. Nhấn lại để dừng.");
            toolTip1.SetToolTip(btnDownloadTool,
                "Mở trình duyệt để tải công cụ giải mã chính thức.");
            toolTip1.SetToolTip(btnSubmitOnline,
                "Gửi file ghi chú ransom lên ID-Ransomware.com để xác định chính xác hơn.\nKhông gửi dữ liệu cá nhân, chỉ gửi file note.");
            toolTip1.SetToolTip(btnSaveReport,
                "Lưu toàn bộ báo cáo phân tích ra file .txt\nđể gửi cho kỹ thuật viên hoặc lưu trữ.");
            toolTip1.SetToolTip(btnRunKvrt,
                "Chạy Kaspersky Virus Removal Tool để diệt ransomware.\nChạy trước khi giải mã file.");
        }

        private void PopulateDriveCheckboxes()
        {
            flowDrives.Controls.Clear();
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType != DriveType.Fixed && drive.DriveType != DriveType.Removable)
                    continue;
                try
                {
                    if (!drive.IsReady) continue;
                    string label = drive.VolumeLabel.Length > 0
                        ? $"{drive.Name} ({drive.VolumeLabel})"
                        : drive.Name;
                    var chk = new CheckBox
                    {
                        Text    = label,
                        Tag     = drive.RootDirectory.FullName,
                        Checked = drive.DriveType == DriveType.Fixed,
                        AutoSize = true,
                        Margin  = new System.Windows.Forms.Padding(0, 4, 10, 0),
                        Font    = new Font("Segoe UI", 9F),
                    };
                    flowDrives.Controls.Add(chk);
                }
                catch { }
            }
            // Fallback nếu không detect được
            if (flowDrives.Controls.Count == 0)
            {
                var chk = new CheckBox { Text = @"C:\", Tag = @"C:\", Checked = true, AutoSize = true };
                flowDrives.Controls.Add(chk);
            }
        }

        // ── Tab 0: Cảnh báo ─────────────────────────────────────────────────────

        // ── KVRT download ────────────────────────────────────────────────────────

        private async Task DownloadKvrtAsync()
        {
            _kvrtCts?.Cancel();
            _kvrtCts = new CancellationTokenSource();
            var token = _kvrtCts.Token;

            string kvrtUrl = System.Configuration.ConfigurationManager.AppSettings["KvrtUrl"]
                             ?? "https://media.kaspersky.com/utilities/VirusUtilities/EN/kvrt.exe";

            this.Invoke((Action)(() => { pbKvrt.Visible = true; pbKvrt.Value = 0; }));
            try
            {
                using (var http = new System.Net.Http.HttpClient { Timeout = TimeSpan.FromMinutes(10) })
                using (var response = await http.GetAsync(kvrtUrl,
                           System.Net.Http.HttpCompletionOption.ResponseHeadersRead, token)
                           .ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    long total = response.Content.Headers.ContentLength ?? -1;

                    using (var src  = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (var dest = new FileStream(_kvrtSavePath, FileMode.Create, FileAccess.Write,
                                                     FileShare.None, 81920, true))
                    {
                        byte[] buf = new byte[81920];
                        long   read = 0;
                        int    n;
                        while ((n = await src.ReadAsync(buf, 0, buf.Length, token).ConfigureAwait(false)) > 0)
                        {
                            await dest.WriteAsync(buf, 0, n, token).ConfigureAwait(false);
                            read += n;
                            if (total > 0)
                            {
                                int pct = (int)(read * 100 / total);
                                this.Invoke((Action)(() =>
                                {
                                    pbKvrt.Value    = pct;
                                    lblDbStatus.Text = $"Đang tải KVRT… {pct}% ({read / 1048576} MB)";
                                }));
                            }
                        }
                    }

                    this.Invoke((Action)(() =>
                    {
                        pbKvrt.Visible   = false;
                        lblDbStatus.Text = $"KVRT đã tải: {_kvrtSavePath}";
                        btnRunKvrt.Visible = true;
                        Logger.Info($"KVRT downloaded to {_kvrtSavePath}");
                    }));
                }
            }
            catch (OperationCanceledException)
            {
                this.Invoke((Action)(() => pbKvrt.Visible = false));
            }
            catch (Exception ex)
            {
                Logger.Error("KVRT download failed", ex);
                if (!token.IsCancellationRequested)
                    this.Invoke((Action)(() =>
                    {
                        pbKvrt.Visible   = false;
                        lblDbStatus.Text = "KVRT: Tải thất bại — tải thủ công từ kaspersky.com";
                    }));
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _kvrtCts?.Cancel();
            _scanner?.Stop();
            _dbRefreshTimer?.Stop();
            _dbRefreshTimer?.Dispose();
        }

        private void LoadUptimeWarning()
        {
            TimeSpan uptime = UptimeHelper.GetUptime();
            string uptimeStr = UptimeHelper.FormatUptime();

            if (UptimeHelper.IsRecentlyRestarted(10))
            {
                // Cảnh báo vàng: vừa restart
                pnlUptimeWarning.Visible   = true;
                pnlUptimeWarning.BackColor = Color.FromArgb(255, 243, 199); // amber light
                lblUptimeWarning.Text =
                    $"⚠️  Phát hiện: Máy đang bật được {uptimeStr}. " +
                    "Có thể bạn vừa khởi động lại sau khi bị nhiễm.";
                lblUptimeWarning.ForeColor = Color.FromArgb(146, 64, 14);   // amber dark
            }
            else
            {
                pnlUptimeWarning.Visible = false;
            }
        }

        private void chkConfirm_CheckedChanged(object sender, EventArgs e)
        {
            btnStartScan.Enabled = chkConfirm.Checked;
        }

        private void btnStartScan_Click(object sender, EventArgs e)
        {
            // Đánh dấu đã confirm → chuyển sang Tab 1
            tabMain.SelectedIndex = 1;
        }

        // Chặn user click thẳng vào tab header khi chưa confirm
        private void tabMain_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == 0) return; // Tab 0 luôn được phép

            if (!chkConfirm.Checked)
            {
                e.Cancel = true; // chặn chuyển tab
                // Nhắc nhở nhẹ nhàng
                lblWarningText.BackColor = Color.FromArgb(255, 243, 199);
                System.Windows.Forms.Timer t = new System.Windows.Forms.Timer { Interval = 1200 };
                t.Tick += (s, _) => { lblWarningText.BackColor = Color.Transparent; t.Stop(); t.Dispose(); };
                t.Start();
            }
        }

        // ── Tab 1: Quét ──────────────────────────────────────────────────────────

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (_isScanning)
            {
                // Dừng quét
                _scanner?.Stop();
                btnScan.Text    = "▶  Bắt đầu quét";
                btnScan.BackColor = Color.FromArgb(0, 120, 212);
                _isScanning = false;
                lblScanStatus.Text = "Đã dừng quét.";
                return;
            }

            // Bắt đầu quét
            if (_families == null || _families.Count == 0)
            {
                MessageBox.Show(
                    "Cơ sở dữ liệu chưa sẵn sàng. Vui lòng đợi hoặc khởi động lại tool.",
                    "Chưa sẵn sàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tải KVRT ngầm nếu được chọn
            if (chkDownloadKvrt.Checked && !File.Exists(_kvrtSavePath))
                _ = DownloadKvrtAsync();

            StartScan();
        }

        private void StartScan()
        {
            // Reset UI
            rtbLog.Clear();
            progressBar.Value = 0;
            lblCurrentPath.Text = "Đang chuẩn bị...";
            lblScanStatus.Text  = "Đang quét...";
            lblScanned.Text  = "File đã quét: 0";
            lblSuspect.Text  = "Nghi ngờ: 0";
            lblNotes.Text    = "Ghi chú ransom: 0";
            pnlSpecialHandling.Visible = false;
            lblInfectionTime.Visible   = false;

            btnScan.Text      = "⏹  Dừng quét";
            btnScan.BackColor = Color.FromArgb(196, 43, 28); // đỏ khi đang quét
            _isScanning = true;

            // Lấy danh sách ổ đĩa cần quét
            var drives = GetDrivesToScan();
            bool deepScan = chkDeepScan.Checked;

            _scanner = new Scanner(drives, _families, deepScan);

            // Đăng ký events (marshal sang UI thread)
            _scanner.ProgressChanged += (total, path) =>
            {
                if (this.IsDisposed) return;
                this.BeginInvoke((Action)(() =>
                {
                    lblCurrentPath.Text = TruncatePath(path, 80);
                    lblScanned.Text     = $"File đã quét: {total:N0}";
                    // Progress bar ước tính (không biết tổng → fake progress)
                    if (progressBar.Value < 95) progressBar.Value++;
                }));
            };

            _scanner.RansomNoteFound += (notePath) =>
            {
                if (this.IsDisposed) return;
                this.BeginInvoke((Action)(() =>
                {
                    AppendLog($"⚠ Tìm thấy: {notePath}");
                    int count = int.TryParse(
                        lblNotes.Text.Replace("Ghi chú ransom: ", ""), out int n) ? n + 1 : 1;
                    lblNotes.Text = $"Ghi chú ransom: {count}";
                }));
            };

            _scanner.ScanCompleted += (result) =>
            {
                if (this.IsDisposed) return;
                this.BeginInvoke((Action)(() => OnScanCompleted(result)));
            };

            // Bắt đầu quét
            AppendLog($"Bắt đầu quét · CSDL: {DbLoader.LastSource} · Deep scan: {(deepScan ? "Có" : "Không")}");
            _scanner.StartAsync();
        }

        private void OnScanCompleted(ScanResult result)
        {
            _scanResult = result;
            _isScanning = false;

            btnScan.Text      = "▶  Bắt đầu quét";
            btnScan.BackColor = Color.FromArgb(0, 120, 212);
            progressBar.Value = 100;

            // Thống kê cuối
            lblScanned.Text = $"File đã quét: {result.TotalFilesScanned:N0}";
            lblSuspect.Text = $"Nghi ngờ: {result.SuspiciousFileCount}";
            lblNotes.Text   = $"Ghi chú ransom: {result.RansomNotePaths.Count}";

            string duration = result.ScanDurationSeconds < 60
                ? $"{result.ScanDurationSeconds:F0}s"
                : $"{result.ScanDurationSeconds / 60:F0}m {result.ScanDurationSeconds % 60:F0}s";

            if (result.IsComplete)
            {
                lblScanStatus.Text = $"Quét xong · Thời gian: {duration}";
                AppendLog($"✓ Quét hoàn tất · {result.TotalFilesScanned:N0} file · {duration}");
            }
            else
            {
                lblScanStatus.Text = $"Đã dừng · {result.TotalFilesScanned:N0} file đã quét";
                AppendLog("⏹ Quét bị dừng.");
            }

            // Hiện kết quả registry vào log
            if (result.RegistryFindings.Count > 0)
            {
                AppendLog("— Registry / Shadow Copy —");
                foreach (var finding in result.RegistryFindings)
                    AppendLog("⚠ " + finding);
            }

            // Nhận diện và chuyển sang tab kết quả
            RunIdentification(result);
        }

        private void RunIdentification(ScanResult scan)
        {
            try
            {
                _identifyResult = Identifier.Identify(scan, _families);

                // Cập nhật tab kết quả
                tabResult.Enabled = true;
                DisplayResult(_identifyResult, scan);

                // Tự động chuyển sang tab kết quả
                tabMain.SelectedIndex = 2;
            }
            catch (Exception ex)
            {
                Logger.Error("frmMain.RunIdentification", ex);
                MessageBox.Show("Lỗi khi nhận diện: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Tab 2: Kết quả ───────────────────────────────────────────────────────

        private void DisplayResult(IdentifyResult result, ScanResult scan)
        {
            // Hiển thị thời gian nhiễm ước tính
            if (scan.EstimatedInfectionTime.HasValue)
            {
                lblInfectionTime.Text    = $"⏱ Ước tính thời gian nhiễm: {scan.EstimatedInfectionTime.Value:dd/MM/yyyy HH:mm} " +
                                           $"(dựa theo mtime file bị mã hóa sớm nhất)";
                lblInfectionTime.Visible = true;
            }
            else
            {
                lblInfectionTime.Visible = false;
            }

            if (result.IsUnknown)
            {
                ShowUnknownResult(result.UnknownReason);
                pnlSpecialHandling.Visible = false;
                return;
            }

            var family = result.TopFamily;
            string status = result.IsConfirmed ? "✅ XÁC ĐỊNH" : "⚠ NGHI NGỜ";
            string statusColor = result.IsConfirmed ? "Xác định" : "Nghi ngờ";

            // Tên + điểm
            lblFamilyName.Text   = family.Name;
            lblScore.Text        = $"{result.TopScore}đ · {status}";
            lblScore.ForeColor   = result.IsConfirmed
                ? Color.FromArgb(21, 128, 61)   // xanh lá
                : Color.FromArgb(146, 64, 14);  // vàng cam

            // Thống kê
            lblEncryptedCount.Text  = $"{scan.SuspiciousFileCount} file bị mã hóa";
            lblNoteCount.Text       = $"{scan.RansomNotePaths.Count} ghi chú ransom";
            lblOtherMatch.Text      = result.OtherMatchCount > 0
                ? $"+ {result.OtherMatchCount} dòng họ khác trùng một phần"
                : "";

            // Công cụ giải mã
            if (family.DecryptorAvailable)
            {
                lblToolName.Text = family.DecryptorToolName ?? "Có công cụ giải mã";
                lnkDecryptorUrl.Text    = family.DecryptorUrl ?? "";
                lnkDecryptorUrl.Visible = !string.IsNullOrEmpty(family.DecryptorUrl);
                btnDownloadTool.Enabled = !string.IsNullOrEmpty(family.DecryptorUrl);
            }
            else
            {
                lblToolName.Text        = "Chưa có công cụ giải mã miễn phí";
                lnkDecryptorUrl.Visible = false;
                btnDownloadTool.Enabled = false;
            }

            // Điều kiện (condition box)
            if (family.DecryptorConditional && !string.IsNullOrEmpty(family.DecryptorCondition))
            {
                pnlCondition.Visible  = true;
                lblCondition.Text     = "⚠️  " + family.DecryptorCondition;
            }
            else
            {
                pnlCondition.Visible = false;
            }

            // Ghi chú
            lblFamilyNote.Text    = family.Note ?? "";
            lblFamilyNote.Visible = !string.IsNullOrEmpty(family.Note);

            // Multi-family: liệt kê top 2-3 candidates
            ShowCandidates(result);

            // Step 5.3: Xử lý đặc biệt
            ShowSpecialHandling(family.SpecialHandling);
        }

        private void ShowSpecialHandling(string handlingId)
        {
            if (string.IsNullOrEmpty(handlingId))
            {
                pnlSpecialHandling.Visible = false;
                return;
            }

            string body;
            switch (handlingId)
            {
                case "wannacry_ram_key":
                    lblSpecialTitle.Text = "🚨 KHẨN CẤP: KHÔNG KHỞI ĐỘNG LẠI MÁY!";
                    body = "WannaCry lưu khóa giải mã trong RAM. Nếu khởi động lại, khóa sẽ MẤT VĨNH VIỄN.\r\n" +
                           "→ Tải WanaKiwi hoặc WannaKey từ máy sạch, chạy ngay trên máy này TRƯỚC KHI TẮT.\r\n" +
                           "→ Link: https://github.com/gentilkiwi/wanakiwi/releases";
                    break;

                case "stop_djvu_key_check":
                    lblSpecialTitle.Text = "⚠ STOP/Djvu: Kiểm tra loại khóa trước khi decrypt";
                    body = "Chỉ ~30% trường hợp dùng OFFLINE KEY (có thể giải mã miễn phí).\r\n" +
                           "Nếu máy có mạng lúc bị nhiễm → ONLINE KEY → Emsisoft không giải mã được.\r\n" +
                           "→ Gửi 1 file gốc + 1 file mã hóa cùng loại lên Emsisoft để kiểm tra: " +
                           "https://www.emsisoft.com/ransomware-decryption-tools/stop-djvu";
                    break;

                case "petya_mbr":
                    lblSpecialTitle.Text = "⚠ Petya/NotPetya: Mã hóa MBR — cần flow riêng";
                    body = "Ransomware này mã hóa Master Boot Record (MBR), không phải file thông thường.\r\n" +
                           "→ Petya 2016: Dùng Petya Sector Decryptor (cần 1 file từ MBR).\r\n" +
                           "→ NotPetya 2017: Là WIPER — KHÔNG có khóa giải mã, dữ liệu không thể phục hồi.\r\n" +
                           "→ KHÔNG chạy công cụ giải mã file thông thường trên trường hợp này.";
                    break;

                case "nas_network":
                    lblSpecialTitle.Text = "🚨 NAS Ransomware: Cách ly mạng NGAY LẬP TỨC!";
                    body = "Loại ransomware này tấn công qua mạng, không phải máy tính cục bộ.\r\n" +
                           "→ Rút cáp mạng / tắt WiFi của NAS ngay lập tức để ngăn lây lan.\r\n" +
                           "→ KHÔNG quét ổ cứng máy tính — quét trực tiếp trên NAS hoặc gắn ổ NAS vào máy sạch.\r\n" +
                           "→ Kiểm tra firmware NAS, thay đổi mật khẩu admin sau khi phục hồi.";
                    break;

                case "double_encrypted":
                    lblSpecialTitle.Text = "⚠ Phát hiện 2 lớp mã hóa";
                    body = "File bị mã hóa 2 lần bởi 2 dòng ransomware khác nhau.\r\n" +
                           "→ Cần giải mã theo đúng thứ tự (lớp ngoài trước, lớp trong sau).\r\n" +
                           "→ Xem cột 'OtherMatchCount' — các family còn lại có thể là lớp mã hóa thứ nhất.\r\n" +
                           "→ Giải mã lần 1 với tool của family hiện tại, kiểm tra kết quả, rồi giải mã lần 2.";
                    break;

                default:
                    pnlSpecialHandling.Visible = false;
                    return;
            }

            lblSpecialBody.Text        = body;
            pnlSpecialHandling.Visible = true;
        }

        private void ShowUnknownResult(string reason)
        {
            lblFamilyName.Text      = "Không nhận diện được";
            lblScore.Text           = "0đ";
            lblScore.ForeColor      = Color.FromArgb(100, 100, 100);
            lblToolName.Text        = reason;
            pnlCondition.Visible   = false;
            btnDownloadTool.Enabled = false;
            lnkDecryptorUrl.Visible = false;
            pnlCandidates.Visible  = false;
        }

        private void ShowCandidates(IdentifyResult result)
        {
            // Hiện danh sách các family runner-up nếu có và điểm gần nhau (trong 30đ)
            var others = result.AllScores
                .Skip(1)
                .Where(s => result.TopScore - s.Score <= 30)
                .Take(3)
                .ToList();

            if (others.Count == 0)
            {
                pnlCandidates.Visible = false;
                lblOtherMatch.Text    = result.OtherMatchCount > 0
                    ? $"+ {result.OtherMatchCount} dòng họ khác trùng một phần (điểm thấp hơn nhiều)"
                    : "";
                return;
            }

            lstCandidates.Items.Clear();
            foreach (var s in others)
            {
                string decryptor = s.Family.DecryptorAvailable ? "✅ Có tool giải mã" : "❌ Chưa có tool";
                lstCandidates.Items.Add(
                    $"  {s.Family.Name,-30} {s.Score}đ   {(s.IsConfirmed ? "XÁC ĐỊNH" : "NGHI NGỜ")}   {decryptor}");
            }
            pnlCandidates.Visible = true;
            lblOtherMatch.Text    = $"+ {result.OtherMatchCount} dòng họ khác — xem danh sách bên dưới ↓";
        }

        // ── Dark mode ────────────────────────────────────────────────────────────

        private frmDonate _donateForm;

        private void btnDonate_Click(object sender, EventArgs e)
        {
            if (_donateForm != null && !_donateForm.IsDisposed)
            {
                _donateForm.BringToFront();
                return;
            }
            _donateForm = new frmDonate(_isEnglish);
            _donateForm.FormClosed += (s, _) => _donateForm = null;
            _donateForm.Show();
        }

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            _isDarkMode = btnDarkMode.Checked;
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            Color bg        = _isDarkMode ? Color.FromArgb(32, 32, 32)  : SystemColors.Control;
            Color bgPanel   = _isDarkMode ? Color.FromArgb(24, 24, 24)  : Color.White;
            Color bgInput   = _isDarkMode ? Color.FromArgb(15, 15, 15)  : Color.FromArgb(15, 15, 15);
            Color fg        = _isDarkMode ? Color.FromArgb(220, 220, 220) : SystemColors.ControlText;
            Color fgMuted   = _isDarkMode ? Color.FromArgb(160, 160, 160) : Color.FromArgb(80, 80, 80);

            this.BackColor = bg;
            foreach (TabPage tab in tabMain.TabPages)
            {
                tab.BackColor = bg;
                ApplyThemeToControls(tab.Controls, bg, bgPanel, bgInput, fg, fgMuted);
            }
            rtbLog.BackColor = bgInput;
            rtbLog.ForeColor = _isDarkMode ? Color.FromArgb(212, 212, 212) : fg;
        }

        private void ApplyThemeToControls(System.Windows.Forms.Control.ControlCollection controls,
            Color bg, Color bgPanel, Color bgInput, Color fg, Color fgMuted)
        {
            foreach (System.Windows.Forms.Control c in controls)
            {
                if (c is Panel || c is FlowLayoutPanel)
                {
                    c.BackColor = bgPanel;
                    ApplyThemeToControls(c.Controls, bg, bgPanel, bgInput, fg, fgMuted);
                }
                else if (c is Label lbl)
                {
                    // Giữ màu đặc biệt (đỏ/xanh/vàng) của labels có ý nghĩa
                    if (lbl.ForeColor == SystemColors.ControlText || lbl.ForeColor == Color.FromArgb(80, 80, 80))
                        lbl.ForeColor = fgMuted;
                    lbl.BackColor = Color.Transparent;
                }
                else if (c is CheckBox)
                {
                    c.BackColor = bgPanel;
                    c.ForeColor = fg;
                }
                else if (c is Button btn)
                {
                    // Buttons với màu custom → giữ nguyên ý đồ thiết kế, chỉ dim nhẹ khi dark
                    Color? origBack = GetButtonOriginalColor(btn.Name);
                    if (origBack.HasValue)
                    {
                        btn.BackColor = _isDarkMode ? DimColor(origBack.Value, 0.7f) : origBack.Value;
                        btn.ForeColor = Color.White;
                    }
                    else
                    {
                        btn.BackColor = bgPanel;
                        btn.ForeColor = fg;
                    }
                }
                else if (c is ProgressBar)
                {
                    // giữ nguyên
                }
                else
                {
                    c.BackColor = bgPanel;
                    c.ForeColor = fg;
                    if (c.Controls.Count > 0)
                        ApplyThemeToControls(c.Controls, bg, bgPanel, bgInput, fg, fgMuted);
                }
            }
        }

        private static Color? GetButtonOriginalColor(string name)
        {
            switch (name)
            {
                case "btnStartScan":
                case "btnScan":        return Color.FromArgb(0,   120, 212);
                case "btnSubmitOnline":return Color.FromArgb(37,   99, 235);
                case "btnDownloadTool":return Color.FromArgb(21,  128,  61);
                case "btnRunKvrt":     return Color.FromArgb(124,  45,  18);
                default:               return null;
            }
        }

        private static Color DimColor(Color c, float factor)
        {
            return Color.FromArgb(
                (int)(c.R * factor),
                (int)(c.G * factor),
                (int)(c.B * factor));
        }

        // ── Language toggle ──────────────────────────────────────────────────────

        private void btnLangToggle_Click(object sender, EventArgs e)
        {
            _isEnglish = btnLangToggle.Checked;
            UpdateLanguage();
        }

        private void UpdateLanguage()
        {
            if (_isEnglish)
            {
                // Tab headers
                tabWarning.Text = "  ⚠  Warning  ";
                tabScan.Text    = "  🔍  Scan  ";
                tabResult.Text  = "  📋  Result  ";
                // Tab 0
                lblWarningTitle.Text = "🛡  RansomRescue — Read carefully before use";
                lblWarningText.Text =
                    "BEFORE RUNNING THIS TOOL, MAKE SURE:\r\n\r\n" +
                    "1.  Disconnect from the network immediately (unplug LAN / disable WiFi) to stop ransomware from spreading.\r\n\r\n" +
                    "2.  DO NOT restart or shut down the computer — some ransomware will delete shadow copies on shutdown.\r\n\r\n" +
                    "3.  Run this tool directly from a USB drive. Do NOT copy it to the infected hard drive.\r\n\r\n" +
                    "4.  This tool ONLY identifies the ransomware family and finds decryption tools.\r\n" +
                    "    It does NOT decrypt files, remove viruses, or replace antivirus software.\r\n\r\n" +
                    "5.  After identifying the family, use official decryption tools from No More Ransom (nomoreransom.org).\r\n\r\n" +
                    "6.  Run Kaspersky Virus Removal Tool (KVRT) to remove the ransomware before decrypting.\r\n\r\n" +
                    "This tool is free and open source. No personal data is collected.";
                chkDownloadKvrt.Text = "Download Kaspersky KVRT in background during scan (~150 MB, requires internet) — saved to %TEMP%\\kvrt.exe";
                chkConfirm.Text      = "I have read and understood the warnings above. I want to continue.";
                btnStartScan.Text    = "Next  →";
                // Tab 1
                chkDeepScan.Text     = "Deep scan (slower)";
                lblDrivesHeader.Text = "Drives:";
                lblLogHeader.Text    = "Scan log:";
                // Tab 2
                lblDecryptorHeader.Text = "DECRYPTION TOOL";
                lblSubmitHeader.Text    = "SUBMIT SAMPLE FOR MORE ACCURATE IDENTIFICATION (optional)";
                btnSubmitOnline.Text    = "🌐 Submit to ID-Ransomware";
                btnSaveReport.Text      = "💾 Save report (.txt)";
                lblNextHeader.Text      = "📋  Next steps";
                lblNextSteps.Text =
                    "1.  Remove ransomware: Download Kaspersky KVRT from a clean PC → run from USB.\r\n\r\n" +
                    "2.  Decrypt files: Use the decryption tool above (if available). Test on 1-2 files first.\r\n\r\n" +
                    "3.  Backup: Before decrypting, copy all encrypted files to an external drive (keep originals).\r\n\r\n" +
                    "4.  No tool yet: Check nomoreransom.org regularly — new tools are added as police seize keys.";
                lblCandidatesHdr.Text   = "OTHER POSSIBLY MATCHING FAMILIES";
                btnRunKvrt.Text         = "🛡  Run Kaspersky KVRT (downloaded to %TEMP%)";
            }
            else
            {
                tabWarning.Text = "  ⚠  Cảnh báo  ";
                tabScan.Text    = "  🔍  Quét  ";
                tabResult.Text  = "  📋  Kết quả  ";
                lblWarningTitle.Text = "🛡  RansomRescue — Đọc kỹ trước khi sử dụng";
                lblWarningText.Text =
                    "TRƯỚC KHI CHẠY TOOL NÀY, HÃY ĐẢM BẢO:\r\n\r\n" +
                    "1.  Ngắt kết nối mạng ngay lập tức (rút cáp LAN / tắt WiFi) để ngăn ransomware lây lan.\r\n\r\n" +
                    "2.  KHÔNG khởi động lại hoặc tắt máy — một số ransomware sẽ xóa shadow copy khi shutdown.\r\n\r\n" +
                    "3.  Cắm USB đã cài tool này vào máy bị nhiễm và chạy trực tiếp từ USB " +
                    "(không copy sang ổ cứng bị nhiễm).\r\n\r\n" +
                    "4.  Tool này CHỈ xác định dòng họ ransomware và tìm công cụ giải mã.\r\n" +
                    "    Tool KHÔNG giải mã, KHÔNG xóa virus, KHÔNG thay thế phần mềm diệt virus.\r\n\r\n" +
                    "5.  Sau khi xác định được dòng họ, hãy sử dụng công cụ giải mã chính hãng " +
                    "từ No More Ransom (nomoreransom.org) hoặc hãng bảo mật uy tín.\r\n\r\n" +
                    "6.  Chạy Kaspersky Virus Removal Tool (KVRT) để loại bỏ ransomware trước khi giải mã.\r\n\r\n" +
                    "Tool này miễn phí và mã nguồn mở. Không thu thập dữ liệu cá nhân.\r\n" +
                    "Phiên bản: 1.0 | Nhà phát triển: ThanhCode93";
                chkDownloadKvrt.Text = "Tải Kaspersky KVRT ngầm trong lúc quét (~150 MB, cần internet) — lưu vào %TEMP%\\kvrt.exe";
                chkConfirm.Text      = "Tôi đã đọc và hiểu các cảnh báo trên. Tôi muốn tiếp tục.";
                btnStartScan.Text    = "Tiếp theo  →";
                chkDeepScan.Text     = "Quét sâu (chậm hơn)";
                lblDrivesHeader.Text = "Ổ đĩa:";
                lblLogHeader.Text    = "Nhật ký quét:";
                lblDecryptorHeader.Text = "CÔNG CỤ GIẢI MÃ";
                lblSubmitHeader.Text    = "GỬI MẪU ĐỂ XÁC ĐỊNH CHÍNH XÁC HƠN (không bắt buộc)";
                btnSubmitOnline.Text    = "🌐 Gửi lên ID-Ransomware";
                btnSaveReport.Text      = "💾 Lưu báo cáo (.txt)";
                lblNextHeader.Text      = "📋  Bước tiếp theo";
                lblNextSteps.Text =
                    "1.  Xóa ransomware: Tải Kaspersky Virus Removal Tool (KVRT) từ máy sạch → chạy từ USB.\r\n\r\n" +
                    "2.  Giải mã file: Dùng công cụ giải mã bên trên (nếu có). Chạy thử trên 1-2 file " +
                    "trước khi chạy toàn bộ.\r\n\r\n" +
                    "3.  Backup: Trước khi giải mã, copy toàn bộ file mã hóa sang ổ cứng ngoài (giữ bản gốc " +
                    "phòng khi công cụ lỗi).\r\n\r\n" +
                    "4.  Nếu chưa có công cụ giải mã: Theo dõi nomoreransom.org — công cụ mới được thêm " +
                    "thường xuyên khi cảnh sát thu giữ khóa.";
                lblCandidatesHdr.Text   = "CÁC DÒNG HỌ KHÁC CÓ THỂ TRÙNG KHỚP";
                btnRunKvrt.Text         = "🛡  Chạy Kaspersky KVRT (đã tải về %TEMP%)";
            }

            // Đồng bộ popup donate nếu đang mở
            if (_donateForm != null && !_donateForm.IsDisposed)
                _donateForm.ApplyLanguage(_isEnglish);
        }

        private void btnRunKvrt_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(_kvrtSavePath))
                {
                    MessageBox.Show("Không tìm thấy file KVRT. Hãy thử tải lại.",
                        "Không tìm thấy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Process.Start(_kvrtSavePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể chạy KVRT: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error("btnRunKvrt: " + ex.Message, ex);
            }
        }

        private void btnSaveReport_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog
            {
                Title      = "Lưu báo cáo",
                Filter     = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                FileName   = $"RansomRescue_Report_{DateTime.Now:yyyyMMdd_HHmmss}.txt",
                DefaultExt = "txt",
            })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                try
                {
                    File.WriteAllText(dlg.FileName, BuildReport(), System.Text.Encoding.UTF8);
                    MessageBox.Show("Đã lưu báo cáo:\n" + dlg.FileName,
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Logger.Info("Report saved: " + dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string BuildReport()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("==================================================");
            sb.AppendLine("  RANSOMRESCUE — BÁO CÁO PHÂN TÍCH RANSOMWARE");
            sb.AppendLine($"  Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine("==================================================");
            sb.AppendLine();

            if (_identifyResult != null && !_identifyResult.IsUnknown)
            {
                var f = _identifyResult.TopFamily;
                sb.AppendLine($"KẾT QUẢ: {(_identifyResult.IsConfirmed ? "XÁC ĐỊNH" : "NGHI NGỜ")}");
                sb.AppendLine($"Dòng họ: {f.Name}");
                sb.AppendLine($"Điểm:    {_identifyResult.TopScore}đ");
                if (f.DecryptorAvailable)
                    sb.AppendLine($"Công cụ: {f.DecryptorToolName ?? "Có"} — {f.DecryptorUrl}");
                else
                    sb.AppendLine("Công cụ: Chưa có công cụ giải mã miễn phí");
            }
            else
            {
                sb.AppendLine($"KẾT QUẢ: Không nhận diện được — {_identifyResult?.UnknownReason}");
            }
            sb.AppendLine();

            if (_scanResult != null)
            {
                sb.AppendLine($"THỐNG KÊ QUÉT:");
                sb.AppendLine($"  File đã quét:      {_scanResult.TotalFilesScanned:N0}");
                sb.AppendLine($"  File nghi ngờ:     {_scanResult.SuspiciousFileCount}");
                sb.AppendLine($"  Ghi chú ransom:    {_scanResult.RansomNotePaths.Count}");
                if (_scanResult.EstimatedInfectionTime.HasValue)
                    sb.AppendLine($"  Ước tính nhiễm:   {_scanResult.EstimatedInfectionTime.Value:dd/MM/yyyy HH:mm}");
                sb.AppendLine();

                if (_scanResult.RansomNotePaths.Count > 0)
                {
                    sb.AppendLine("GHI CHÚ RANSOM TÌM THẤY:");
                    foreach (var p in _scanResult.RansomNotePaths)
                        sb.AppendLine("  " + p);
                    sb.AppendLine();
                }

                if (_scanResult.RegistryFindings.Count > 0)
                {
                    sb.AppendLine("REGISTRY / SHADOW COPY:");
                    foreach (var r in _scanResult.RegistryFindings)
                        sb.AppendLine("  " + r);
                    sb.AppendLine();
                }

                if (_scanResult.ExtensionFrequency.Count > 0)
                {
                    sb.AppendLine("PHẦN MỞ RỘNG BỊ MÃ HÓA:");
                    foreach (var kv in _scanResult.ExtensionFrequency)
                        sb.AppendLine($"  {kv.Key,-20} {kv.Value} file");
                    sb.AppendLine();
                }
            }

            sb.AppendLine("NHẬT KÝ QUÉT:");
            sb.AppendLine(rtbLog.Text);
            sb.AppendLine("==================================================");
            return sb.ToString();
        }

        private void btnDownloadTool_Click(object sender, EventArgs e)
        {
            if (_identifyResult?.TopFamily?.DecryptorUrl != null)
            {
                try { Process.Start(_identifyResult.TopFamily.DecryptorUrl); }
                catch (Exception ex) { Logger.Warn("btnDownloadTool: " + ex.Message); }
            }
        }

        private void btnSubmitOnline_Click(object sender, EventArgs e)
        {
            // Mở ID-Ransomware trực tiếp nếu chưa có kết quả để submit
            if (_scanResult == null || _scanResult.RansomNotePaths.Count == 0)
            {
                MessageBox.Show(
                    "Cần quét xong và tìm được ít nhất 1 file ghi chú ransom trước khi gửi.",
                    "Chưa sẵn sàng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Gửi async (không block UI)
            SubmitToIdRansomwareAsync();
        }

        private async void SubmitToIdRansomwareAsync()
        {
            btnSubmitOnline.Enabled = false;
            btnSubmitOnline.Text    = "Đang gửi...";

            try
            {
                string notePath    = _scanResult.RansomNotePaths[0];
                var submitter      = new OnlineSubmitter();
                var result         = await submitter.SubmitAsync(notePath);

                if (result.Success)
                {
                    lblSubmitStatus.Text      = $"✅ Đã gửi · {result.FamilyName}";
                    lblSubmitStatus.ForeColor = Color.FromArgb(21, 128, 61);
                    if (!string.IsNullOrEmpty(result.ResultUrl))
                    {
                        try { Process.Start(result.ResultUrl); }
                        catch { }
                    }
                }
                else if (result.IsOffline)
                {
                    lblSubmitStatus.Text      = "❌ Không có mạng";
                    lblSubmitStatus.ForeColor = Color.FromArgb(185, 28, 28);
                    MessageBox.Show(
                        result.ErrorMsg + "\n\nKết nối mạng rồi nhấn Gửi lại.",
                        "Mất kết nối", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    // Tất cả retry thất bại → hiện panel hướng dẫn email
                    lblSubmitStatus.Text      = "❌ Gửi thất bại — xem hướng dẫn email";
                    lblSubmitStatus.ForeColor = Color.FromArgb(185, 28, 28);
                    ShowEmailFallback();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("frmMain.SubmitToIdRansomware", ex);
                lblSubmitStatus.Text = "❌ Lỗi: " + ex.Message;
            }
            finally
            {
                btnSubmitOnline.Enabled = true;
                btnSubmitOnline.Text    = "🌐 Gửi lên ID-Ransomware";
            }
        }

        private void ShowEmailFallback()
        {
            // Điền sẵn thông tin email fallback
            string ext = _identifyResult?.TopFamily != null
                ? string.Join(", ", _identifyResult.TopFamily.Extensions)
                : "(chưa xác định)";

            string emailTo      = System.Configuration.ConfigurationManager.AppSettings["FallbackEmail"]
                                  ?? "support@id-ransomware.malwarehunterteam.com";
            string emailSubject = $"Ransomware sample - Extension: {ext}";
            string emailBody    = $"Xin chào,\n\nMáy tôi bị nhiễm ransomware.\n" +
                                  $"Phần mở rộng file bị mã hóa: {ext}\n" +
                                  $"Đính kèm: file ghi chú ransom + 1 file bị mã hóa (nhỏ).\n\n" +
                                  $"Cảm ơn.";

            txtEmailTo.Text      = emailTo;
            txtEmailSubject.Text = emailSubject;
            txtEmailBody.Text    = emailBody;
            pnlEmailFallback.Visible = true;
        }

        private void btnCopyEmail_Click(object sender, EventArgs e)
        {
            string content = $"To: {txtEmailTo.Text}\n" +
                             $"Subject: {txtEmailSubject.Text}\n\n" +
                             txtEmailBody.Text;
            try
            {
                Clipboard.SetText(content);
                MessageBox.Show("Đã copy thông tin email vào clipboard.",
                    "Đã copy", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch { }
        }

        private void btnOpenMail_Click(object sender, EventArgs e)
        {
            try
            {
                string mailto = $"mailto:{Uri.EscapeDataString(txtEmailTo.Text)}" +
                                $"?subject={Uri.EscapeDataString(txtEmailSubject.Text)}" +
                                $"&body={Uri.EscapeDataString(txtEmailBody.Text)}";
                Process.Start(mailto);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở ứng dụng email: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnOpenBrowser_Click(object sender, EventArgs e)
        {
            try { Process.Start("https://id-ransomware.malwarehunterteam.com"); }
            catch { }
        }

        private void lnkDecryptorUrl_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(lnkDecryptorUrl.Text))
            {
                try { Process.Start(lnkDecryptorUrl.Text); }
                catch (Exception ex) { Logger.Warn("lnkDecryptorUrl: " + ex.Message); }
            }
        }

        // ── Helpers ──────────────────────────────────────────────────────────────

        private List<string> GetDrivesToScan()
        {
            var drives = new List<string>();
            foreach (Control ctrl in flowDrives.Controls)
            {
                if (ctrl is CheckBox chk && chk.Checked && chk.Tag is string path)
                    drives.Add(path);
            }
            if (drives.Count == 0) drives.Add(@"C:\");
            return drives;
        }

        private void AppendLog(string message)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            string line = $"[{time}] {message}{Environment.NewLine}";
            bool isSuspicious = message.StartsWith("⚠") || message.StartsWith("✓");

            rtbLog.SelectionStart  = rtbLog.TextLength;
            rtbLog.SelectionLength = 0;
            rtbLog.SelectionColor  = isSuspicious && message.StartsWith("⚠")
                ? Color.FromArgb(255, 100, 100)   // đỏ cam — dòng nghi ngờ
                : Color.FromArgb(180, 220, 180);  // xanh nhạt — dòng bình thường
            rtbLog.AppendText(line);
            rtbLog.SelectionColor = rtbLog.ForeColor;
            rtbLog.ScrollToCaret();
        }

        private static string TruncatePath(string path, int maxLen)
        {
            if (string.IsNullOrEmpty(path) || path.Length <= maxLen) return path;
            return "..." + path.Substring(path.Length - maxLen + 3);
        }
    }
}
