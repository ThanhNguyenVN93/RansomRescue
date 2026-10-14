using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using frm_ransomrescure.Models;
using frm_ransomrescure.Utils;
using Newtonsoft.Json;

namespace frm_ransomrescure.Core
{
    /// <summary>
    /// Tải và quản lý database ransomware families (db.json).
    ///
    /// Thứ tự ưu tiên:
    ///   1. Cache local (nếu còn hợp lệ < 7 ngày) → instant
    ///   2. NMR HTML scrape (8s timeout) — nguồn chính
    ///   3. GitHub raw db.json (8s timeout, song song với NMR nếu NMR lỗi trước)
    ///   4. Embedded resource trong EXE (~30 families) — luôn có sẵn
    /// </summary>
    public static class DbLoader
    {
        // ── Cấu hình ────────────────────────────────────────────────────────────
        private static readonly int    TimeoutMs       = int.Parse(ConfigurationManager.AppSettings["DbFetchTimeoutMs"] ?? "8000");
        private static readonly string UrlMain         = ConfigurationManager.AppSettings["DbUrlMain"]     ?? "https://www.nomoreransom.org/en/decryption-tools.html";
        private static readonly string UrlFallback     = ConfigurationManager.AppSettings["DbUrlFallback"] ?? "";
        private static readonly int    CacheMaxAgeDays = int.Parse(ConfigurationManager.AppSettings["CacheMaxAgeDays"] ?? "7");
        private static readonly string CachePath       = PathHelper.GetCachePath();

        // ── Trạng thái nội bộ ───────────────────────────────────────────────────
        /// <summary>
        /// true = lần tải trước NMR đã thất bại → lần sau fetch song song NMR + GitHub ngay
        /// </summary>
        private static bool _nmrFailedLastTime = false;

        // ── Public API ──────────────────────────────────────────────────────────

        /// <summary>
        /// Nguồn DB đã dùng (để hiển thị trên UI statusbar)
        /// </summary>
        public static string LastSource { get; private set; } = "Chưa tải";

        /// <summary>
        /// Tải danh sách ransomware families.
        /// Trả về list không rỗng (tối thiểu từ embedded resource).
        /// </summary>
        public static async Task<List<RansomFamily>> LoadAsync()
        {
            // ── Bước 1: Kiểm tra cache ─────────────────────────────────────────
            var cached = TryLoadCache();
            if (cached != null)
            {
                Logger.Info($"DbLoader: Cache hợp lệ, dùng cache ({cached.Count} families)");
                LastSource = $"Cache local ({cached.Count} dòng họ)";
                return cached;
            }

            // ── Bước 2: Tải từ mạng ────────────────────────────────────────────
            List<RansomFamily> result = null;

            if (_nmrFailedLastTime && !string.IsNullOrEmpty(UrlFallback))
            {
                // NMR lỗi lần trước → fetch song song để nhanh hơn
                Logger.Info("DbLoader: NMR lỗi lần trước → fetch song song NMR + GitHub");
                result = await FetchParallelAsync();
            }
            else
            {
                // Thử NMR trước
                result = await FetchFromNmrAsync();

                if (result == null && !string.IsNullOrEmpty(UrlFallback))
                {
                    // NMR thất bại → thử GitHub
                    _nmrFailedLastTime = true;
                    Logger.Warn("DbLoader: NMR thất bại, thử GitHub fallback");
                    result = await FetchFromGitHubAsync();
                }
            }

            // ── Bước 3: Fallback → Embedded resource ───────────────────────────
            if (result == null || result.Count == 0)
            {
                Logger.Warn("DbLoader: Mạng thất bại, dùng embedded DB");
                result = LoadEmbedded();
                LastSource = $"Dữ liệu nội bộ ({result.Count} dòng họ)";
                return result;
            }

            // Lưu cache
            SaveCache(result);
            return result;
        }

        // ── NMR HTML scrape ─────────────────────────────────────────────────────

        private static async Task<List<RansomFamily>> FetchFromNmrAsync()
        {
            try
            {
                string html = await FetchHtmlAsync(UrlMain, TimeoutMs);
                if (string.IsNullOrEmpty(html)) return null;

                var families = ParseNmrHtml(html);
                if (families == null || families.Count == 0) return null;

                Logger.Info($"DbLoader: NMR OK → {families.Count} families");
                _nmrFailedLastTime = false;
                LastSource = $"NMR ({families.Count} dòng họ)";
                return families;
            }
            catch (Exception ex)
            {
                Logger.Warn($"DbLoader.FetchFromNmrAsync: {ex.Message}");
                _nmrFailedLastTime = true;
                return null;
            }
        }

        private static async Task<List<RansomFamily>> FetchFromGitHubAsync()
        {
            try
            {
                string json = await FetchHtmlAsync(UrlFallback, TimeoutMs);
                if (string.IsNullOrEmpty(json)) return null;

                var db = JsonConvert.DeserializeObject<DbJson>(json);
                if (db?.Families == null || db.Families.Count == 0) return null;

                Logger.Info($"DbLoader: GitHub OK → {db.Families.Count} families");
                LastSource = $"GitHub ({db.Families.Count} dòng họ)";
                return db.Families;
            }
            catch (Exception ex)
            {
                Logger.Warn($"DbLoader.FetchFromGitHubAsync: {ex.Message}");
                return null;
            }
        }

        private static async Task<List<RansomFamily>> FetchParallelAsync()
        {
            var cts = new CancellationTokenSource();
            var nmrTask    = FetchFromNmrAsync();
            var githubTask = FetchFromGitHubAsync();

            // Lấy kết quả đầu tiên không null
            while (true)
            {
                var done = await Task.WhenAny(nmrTask, githubTask);
                var result = await done;
                if (result != null && result.Count > 0)
                {
                    cts.Cancel();
                    return result;
                }
                // Nếu cả 2 done và đều null
                if (nmrTask.IsCompleted && githubTask.IsCompleted)
                    return null;
            }
        }

        // ── Parse NMR HTML ──────────────────────────────────────────────────────

        /// <summary>
        /// Parse trang /en/decryption-tools.html của NMR bằng Regex.
        /// Không cần HtmlAgilityPack — trang NMR có cấu trúc ổn định.
        ///
        /// Pattern tìm kiếm (ví dụ):
        ///   &lt;h3&gt;STOP Djvu Ransomware&lt;/h3&gt;
        ///   ...
        ///   &lt;a href="https://www.emsisoft.com/..."&gt;Download&lt;/a&gt;
        /// </summary>
        private static List<RansomFamily> ParseNmrHtml(string html)
        {
            var families = new List<RansomFamily>();

            // Tìm các block tool — NMR dùng pattern nhất quán
            // Mỗi tool có: tên + link download
            var blockPattern = new Regex(
                @"<div[^>]*class=""[^""]*decrypt[^""]*""[^>]*>(.*?)</div>",
                RegexOptions.Singleline | RegexOptions.IgnoreCase);

            var namePattern = new Regex(
                @"<h[23][^>]*>\s*(.+?)\s*</h[23]>",
                RegexOptions.Singleline | RegexOptions.IgnoreCase);

            var linkPattern = new Regex(
                @"href=""(https?://[^""]+)""[^>]*>\s*(Download|Tải về|Get it)",
                RegexOptions.IgnoreCase);

            foreach (Match block in blockPattern.Matches(html))
            {
                string blockHtml = block.Groups[1].Value;

                var nameMatch = namePattern.Match(blockHtml);
                if (!nameMatch.Success) continue;

                string name = WebUtility.HtmlDecode(nameMatch.Groups[1].Value).Trim();
                if (string.IsNullOrWhiteSpace(name)) continue;

                string url = "";
                var linkMatch = linkPattern.Match(blockHtml);
                if (linkMatch.Success)
                    url = linkMatch.Groups[1].Value;

                // Tạo family tối giản từ NMR — chi tiết hơn ở GitHub db.json
                var family = new RansomFamily
                {
                    Id                 = name.ToLowerInvariant().Replace(" ", "_").Replace("/", "_"),
                    Name               = name,
                    DecryptorAvailable = true,
                    DecryptorUrl       = url,
                    DecryptorToolName  = $"{name} Decryptor",
                };

                families.Add(family);
            }

            Logger.Info($"DbLoader.ParseNmrHtml: parsed {families.Count} entries");
            return families;
        }

        // ── Embedded resource ───────────────────────────────────────────────────

        private static List<RansomFamily> LoadEmbedded()
        {
            try
            {
                var asm = Assembly.GetExecutingAssembly();
                // Resource name: frm_ransomrescure.Resources.db.json
                string resName = "frm_ransomrescure.Resources.db.json";

                using (var stream = asm.GetManifestResourceStream(resName))
                {
                    if (stream == null)
                    {
                        Logger.Error("DbLoader: Embedded db.json không tìm thấy!");
                        return new List<RansomFamily>();
                    }

                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string json = reader.ReadToEnd();
                        var db = JsonConvert.DeserializeObject<DbJson>(json);
                        return db?.Families ?? new List<RansomFamily>();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("DbLoader.LoadEmbedded", ex);
                return new List<RansomFamily>();
            }
        }

        // ── Cache ───────────────────────────────────────────────────────────────

        private static List<RansomFamily> TryLoadCache()
        {
            try
            {
                if (!File.Exists(CachePath)) return null;

                var fileInfo = new FileInfo(CachePath);
                double ageDays = (DateTime.Now - fileInfo.LastWriteTime).TotalDays;
                if (ageDays > CacheMaxAgeDays)
                {
                    Logger.Info($"DbLoader: Cache hết hạn ({ageDays:F1} ngày > {CacheMaxAgeDays} ngày)");
                    return null;
                }

                string json = File.ReadAllText(CachePath, Encoding.UTF8);
                var db = JsonConvert.DeserializeObject<DbJson>(json);
                return db?.Families;
            }
            catch (Exception ex)
            {
                Logger.Warn($"DbLoader.TryLoadCache: {ex.Message}");
                return null;
            }
        }

        private static void SaveCache(List<RansomFamily> families)
        {
            try
            {
                string dir = Path.GetDirectoryName(CachePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var db = new DbJson { Families = families, Version = DateTime.Now.ToString("yyyy.MM.dd") };
                string json = JsonConvert.SerializeObject(db, Formatting.Indented);
                File.WriteAllText(CachePath, json, Encoding.UTF8);
                Logger.Info($"DbLoader: Cache saved ({families.Count} families) → {CachePath}");
            }
            catch (Exception ex)
            {
                Logger.Warn($"DbLoader.SaveCache: {ex.Message}");
                // Không nghiêm trọng — lần sau sẽ tải lại
            }
        }

        // ── HTTP helper ─────────────────────────────────────────────────────────

        private static async Task<string> FetchHtmlAsync(string url, int timeoutMs)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(timeoutMs);
                client.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) RansomRescue/1.0");

                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        // ── DB JSON schema ──────────────────────────────────────────────────────

        private class DbJson
        {
            [JsonProperty("version")]
            public string Version { get; set; }

            [JsonProperty("families")]
            public List<RansomFamily> Families { get; set; }
        }
    }
}
