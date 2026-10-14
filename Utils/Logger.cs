using System;
using System.IO;
using System.Text;

namespace frm_ransomrescure.Utils
{
    /// <summary>
    /// Logger nhẹ — ghi log ra file bên cạnh EXE để debug.
    /// Trong Release build, log file chỉ tạo nếu có lỗi (tránh để lại vết trên máy user).
    /// </summary>
    public static class Logger
    {
        private static readonly object _lock = new object();
        private static string _logPath;

        private static string LogPath
        {
            get
            {
                if (_logPath == null)
                {
                    string dir = Path.GetDirectoryName(
                        System.Reflection.Assembly.GetExecutingAssembly().Location)
                        ?? AppDomain.CurrentDomain.BaseDirectory;
                    _logPath = Path.Combine(dir, "RansomRescue_log.txt");
                }
                return _logPath;
            }
        }

        public static void Info(string message)  => Write("INFO ", message);
        public static void Warn(string message)  => Write("WARN ", message);
        public static void Error(string message) => Write("ERROR", message);

        public static void Error(string message, Exception ex)
            => Write("ERROR", $"{message} → {ex.GetType().Name}: {ex.Message}");

        private static void Write(string level, string message)
        {
            try
            {
                string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
                lock (_lock)
                {
                    File.AppendAllText(LogPath, line + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch
            {
                // Không được crash tool chỉ vì log lỗi
            }
        }
    }
}
