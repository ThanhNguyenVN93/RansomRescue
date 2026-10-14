using System;
using System.Collections.Generic;
using System.IO;

namespace frm_ransomrescure.Utils
{
    /// <summary>
    /// Các thư mục và file cần bỏ qua khi quét, và helper liên quan đến đường dẫn.
    /// Chiến lược: skip những thứ ransomware CŨNG thường skip để tránh phá Windows.
    /// </summary>
    public static class PathHelper
    {
        // ── Thư mục bỏ qua hoàn toàn (không quét extension lẫn ransom note) ────
        // Lý do: ransomware thường skip để không phá hệ thống → mình cũng skip cho nhanh
        public static readonly HashSet<string> SkipDirNames = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase)
        {
            // Windows internals
            "WinSxS",
            "Installer",
            "servicing",
            "assembly",          // .NET GAC
            "Microsoft.NET",
            "Windows Defender",
            "Windows Defender Advanced Threat Protection",
            "Microsoft Antimalware",

            // Boot & recovery
            "Boot",
            "Recovery",
            "System Volume Information",
            "$Recycle.Bin",
            "$RECYCLE.BIN",
            "RECYCLER",

            // Hệ thống khác
            "ProgramData\\Microsoft\\Windows\\WER",  // Windows Error Reporting
            "Windows\\Logs",
            "Windows\\Prefetch",
        };

        // ── Thư mục chỉ quét ransom note, KHÔNG quét extension ─────────────────
        // Lý do: ransomware vẫn đặt note vào đây nhưng không mã hóa system files
        public static readonly HashSet<string> NoteOnlyDirNames = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase)
        {
            "Windows",
            "Windows\\System32",
            "Windows\\SysWOW64",
            "Program Files",
            "Program Files (x86)",
        };

        // ── File đơn lẻ bỏ qua ────────────────────────────────────────────────
        public static readonly HashSet<string> SkipFileNames = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase)
        {
            "pagefile.sys",
            "hiberfil.sys",
            "swapfile.sys",
            "NTUSER.DAT",
            "NTUSER.DAT.LOG1",
            "NTUSER.DAT.LOG2",
            "UsrClass.dat",
        };

        // ── Extension bỏ qua (không có ý nghĩa với ransomware scan) ───────────
        public static readonly HashSet<string> SkipExtensions = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase)
        {
            ".lnk",   // shortcut
            ".sys",   // driver
            ".dll",   // library
            ".exe",   // executable
            ".msi",   // installer
            ".ini",   // config nhỏ
            ".dat",   // data binary
            ".log",   // log
            ".tmp",   // temp
            ".pf",    // prefetch
        };

        /// <summary>
        /// Kiểm tra xem một thư mục có nên bị bỏ qua hoàn toàn không.
        /// Hỗ trợ cả tên đơn lẻ ("WinSxS") và path nhiều cấp ("Windows\\Logs").
        /// </summary>
        public static bool ShouldSkipDirectory(string dirPath)
        {
            if (string.IsNullOrEmpty(dirPath)) return false;
            string norm = dirPath.TrimEnd('\\', '/');
            foreach (string skip in SkipDirNames)
                if (PathEndsWith(norm, skip)) return true;
            return false;
        }

        /// <summary>
        /// Kiểm tra xem thư mục này chỉ nên quét ransom note (không quét extension).
        /// Hỗ trợ cả tên đơn lẻ ("Windows") và path nhiều cấp ("Windows\\System32").
        /// </summary>
        public static bool IsNoteOnlyDirectory(string dirPath)
        {
            if (string.IsNullOrEmpty(dirPath)) return false;
            string norm = dirPath.TrimEnd('\\', '/');
            foreach (string entry in NoteOnlyDirNames)
                if (PathEndsWith(norm, entry)) return true;
            return false;
        }

        // Kiểm tra path kết thúc bằng suffix với boundary đúng (không match "NotWindows" khi cần "Windows")
        private static bool PathEndsWith(string fullPath, string suffix)
        {
            if (string.Equals(fullPath, suffix, StringComparison.OrdinalIgnoreCase))
                return true;
            return fullPath.EndsWith(Path.DirectorySeparatorChar + suffix,
                                     StringComparison.OrdinalIgnoreCase)
                || fullPath.EndsWith('/' + suffix, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Kiểm tra xem một file có nên bị bỏ qua không (theo tên hoặc extension).
        /// </summary>
        public static bool ShouldSkipFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return false;
            string name = Path.GetFileName(filePath);
            string ext  = Path.GetExtension(filePath);
            return SkipFileNames.Contains(name) || SkipExtensions.Contains(ext);
        }

        /// <summary>
        /// Lấy đường dẫn cache DB, expand %ProgramData%.
        /// Mặc định: C:\ProgramData\Microsoft\Windows\RR\db_cache.json
        /// (trông như Windows component → ransomware thường bỏ qua)
        /// </summary>
        public static string GetCachePath()
        {
            string raw = System.Configuration.ConfigurationManager.AppSettings["CachePath"]
                         ?? @"%ProgramData%\Microsoft\Windows\RR\db_cache.json";
            return Environment.ExpandEnvironmentVariables(raw);
        }
    }
}
