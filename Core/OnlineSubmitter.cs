using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using frm_ransomrescure.Models;
using frm_ransomrescure.Utils;

namespace frm_ransomrescure.Core
{
    /// <summary>
    /// Gửi mẫu file lên ID-Ransomware để xác định online.
    ///
    /// Flow:
    ///   1. Thử gửi tối đa 3 lần, delay 3s giữa mỗi lần
    ///   2. Nếu offline: ping 8.8.8.8 → hiện "Kết nối mạng rồi thử lại"
    ///   3. Nếu 3 lần đều fail: trả về lỗi → UI hiện hướng dẫn gửi email thủ công
    /// </summary>
    public class OnlineSubmitter
    {
        private const string SubmitUrl    = "https://id-ransomware.malwarehunterteam.com/upload.php";
        private const int    MaxRetries   = 3;
        private const int    RetryDelayMs = 3000;

        private readonly int _retryCount;
        private readonly int _retryDelayMs;

        public OnlineSubmitter()
        {
            _retryCount   = int.Parse(ConfigurationManager.AppSettings["SubmitRetryCount"]   ?? "3");
            _retryDelayMs = int.Parse(ConfigurationManager.AppSettings["SubmitRetryDelayMs"] ?? "3000");
        }

        // ── Result type ─────────────────────────────────────────────────────────

        public class SubmitResult
        {
            public bool   Success    { get; set; }
            public string ResultUrl  { get; set; }   // URL kết quả trả về từ server
            public string FamilyName { get; set; }   // Tên family nếu server nhận diện được
            public bool   IsOffline  { get; set; }   // true = không có mạng
            public string ErrorMsg   { get; set; }   // Mô tả lỗi chi tiết
        }

        // ── Public API ──────────────────────────────────────────────────────────

        /// <summary>
        /// Gửi file ghi chú ransom và/hoặc file bị mã hóa lên ID-Ransomware.
        /// </summary>
        /// <param name="ransomNotePath">Đường dẫn file ghi chú ransom (bắt buộc)</param>
        /// <param name="encryptedFilePath">Đường dẫn file bị mã hóa (tùy chọn, tăng độ chính xác)</param>
        public async Task<SubmitResult> SubmitAsync(
            string ransomNotePath,
            string encryptedFilePath = null)
        {
            // Kiểm tra file tồn tại
            if (!File.Exists(ransomNotePath))
                return new SubmitResult { Success = false, ErrorMsg = "Không tìm thấy file ghi chú ransom." };

            // Kiểm tra mạng trước
            bool isOnline = await CheckOnlineAsync();
            if (!isOnline)
            {
                return new SubmitResult
                {
                    Success   = false,
                    IsOffline = true,
                    ErrorMsg  = "Không có kết nối internet. Vui lòng kết nối mạng rồi thử lại."
                };
            }

            // Thử gửi với retry
            for (int attempt = 1; attempt <= _retryCount; attempt++)
            {
                Logger.Info($"OnlineSubmitter: Attempt {attempt}/{_retryCount}");
                try
                {
                    var result = await TrySubmitAsync(ransomNotePath, encryptedFilePath);
                    if (result.Success)
                    {
                        Logger.Info($"OnlineSubmitter: Success on attempt {attempt}");
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Warn($"OnlineSubmitter attempt {attempt}: {ex.Message}");
                }

                // Chờ trước khi retry (không chờ sau lần cuối)
                if (attempt < _retryCount)
                    await Task.Delay(_retryDelayMs);
            }

            // Tất cả retry đều thất bại
            Logger.Warn("OnlineSubmitter: All retries failed → fallback to email instructions");
            return new SubmitResult
            {
                Success  = false,
                ErrorMsg = $"Không thể kết nối sau {_retryCount} lần thử. " +
                           "Bạn có thể gửi mẫu qua email theo hướng dẫn bên dưới."
            };
        }

        // ── Private: HTTP submit ─────────────────────────────────────────────────

        private async Task<SubmitResult> TrySubmitAsync(
            string ransomNotePath,
            string encryptedFilePath)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30);

                using (var form = new MultipartFormDataContent())
                {
                    // Đính kèm ransom note
                    var noteBytes   = File.ReadAllBytes(ransomNotePath);
                    var noteContent = new ByteArrayContent(noteBytes);
                    noteContent.Headers.ContentType =
                        new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
                    form.Add(noteContent, "note", Path.GetFileName(ransomNotePath));

                    // Đính kèm file mã hóa nếu có (tăng độ chính xác nhận diện)
                    if (!string.IsNullOrEmpty(encryptedFilePath) && File.Exists(encryptedFilePath))
                    {
                        // Chỉ gửi tối đa 1MB đầu của file mã hóa
                        byte[] encBytes = ReadMaxBytes(encryptedFilePath, maxBytes: 1024 * 1024);
                        var encContent  = new ByteArrayContent(encBytes);
                        encContent.Headers.ContentType =
                            new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                        form.Add(encContent, "file", Path.GetFileName(encryptedFilePath));
                    }

                    var response = await client.PostAsync(SubmitUrl, form);

                    if (!response.IsSuccessStatusCode)
                        return new SubmitResult
                        {
                            Success  = false,
                            ErrorMsg = $"Server trả về HTTP {(int)response.StatusCode}"
                        };

                    string body = await response.Content.ReadAsStringAsync();
                    return ParseResponse(body);
                }
            }
        }

        private static SubmitResult ParseResponse(string responseBody)
        {
            // ID-Ransomware trả về JSON với trường "result" và "url"
            // Đây là parse đơn giản — nếu API thay đổi thì cập nhật
            try
            {
                var json = Newtonsoft.Json.Linq.JObject.Parse(responseBody);
                string status = json["status"]?.ToString() ?? "";
                string url    = json["url"]?.ToString()    ?? "";
                string family = json["family"]?.ToString() ?? "";

                bool success = status.Equals("identified", StringComparison.OrdinalIgnoreCase)
                            || !string.IsNullOrEmpty(url);

                return new SubmitResult
                {
                    Success    = success,
                    ResultUrl  = url,
                    FamilyName = family,
                    ErrorMsg   = success ? null : "Server không nhận diện được dòng họ."
                };
            }
            catch
            {
                // Không parse được → coi như thất bại
                return new SubmitResult
                {
                    Success  = false,
                    ErrorMsg = "Không thể đọc kết quả từ server."
                };
            }
        }

        // ── Private: Online check ────────────────────────────────────────────────

        /// <summary>
        /// Ping 8.8.8.8 để kiểm tra có mạng không.
        /// Nhanh hơn DNS lookup trong môi trường bị nhiễm.
        /// </summary>
        private static async Task<bool> CheckOnlineAsync()
        {
            try
            {
                using (var ping = new System.Net.NetworkInformation.Ping())
                {
                    var reply = await Task.Run(() =>
                        ping.Send("8.8.8.8", timeout: 2000));
                    return reply.Status == System.Net.NetworkInformation.IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        // ── Helper ───────────────────────────────────────────────────────────────

        private static byte[] ReadMaxBytes(string filePath, int maxBytes)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                int toRead = (int)Math.Min(fs.Length, maxBytes);
                byte[] buffer = new byte[toRead];
                fs.Read(buffer, 0, toRead);
                return buffer;
            }
        }
    }
}
