using System.Collections.Generic;

namespace frm_ransomrescure.Models
{
    /// <summary>
    /// Kết quả của quá trình quét — tập hợp toàn bộ bằng chứng thu thập được
    /// </summary>
    public class ScanResult
    {
        // ── Thống kê tổng quát ─────────────────────────────────────────────────

        /// <summary>Tổng số file đã duyệt qua (không tính file bị skip)</summary>
        public long TotalFilesScanned { get; set; }

        /// <summary>Số file có phần mở rộng khớp với db.json hoặc không nằm trong whitelist</summary>
        public int SuspiciousFileCount { get; set; }

        // ── Bằng chứng A: Phần mở rộng ────────────────────────────────────────

        /// <summary>
        /// Tần suất xuất hiện mỗi phần mở rộng nghi ngờ.
        /// Key = ".djvu", Value = số lần gặp
        /// </summary>
        public Dictionary<string, int> ExtensionFrequency { get; set; }
            = new Dictionary<string, int>(System.StringComparer.OrdinalIgnoreCase);

        // ── Bằng chứng B: Ghi chú tiền chuộc ──────────────────────────────────

        /// <summary>Danh sách đường dẫn đầy đủ đến các file ghi chú ransom tìm thấy</summary>
        public List<string> RansomNotePaths { get; set; } = new List<string>();

        /// <summary>
        /// Nội dung các file ghi chú (tối đa 4KB mỗi file để tránh tốn RAM).
        /// Key = đường dẫn file, Value = nội dung text
        /// </summary>
        public Dictionary<string, string> RansomNoteContents { get; set; }
            = new Dictionary<string, string>();

        // ── Bằng chứng C: Magic bytes ─────────────────────────────────────────────

        /// <summary>
        /// Magic bytes đọc từ các file khả nghi (tối đa 8 byte đầu).
        /// Key = 16 ký tự hex uppercase (8 byte), Value = phần mở rộng file
        /// </summary>
        public Dictionary<string, string> MagicByteEvidence { get; set; }
            = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);

        // ── Bằng chứng D: Registry & mtime ───────────────────────────────────────

        /// <summary>Các phát hiện registry bất thường (mô tả dạng chuỗi để hiển thị)</summary>
        public List<string> RegistryFindings { get; set; } = new List<string>();

        /// <summary>Thời điểm ước tính máy bị nhiễm (dựa trên mtime file bị mã hóa sớm nhất)</summary>
        public System.DateTime? EstimatedInfectionTime { get; set; }

        // ── Trạng thái quét ────────────────────────────────────────────────────

        /// <summary>true = quét sâu (full whitelist scan), false = quét thường</summary>
        public bool IsDeepScan { get; set; }

        /// <summary>true = quét đã hoàn thành (không bị dừng giữa chừng)</summary>
        public bool IsComplete { get; set; }

        /// <summary>Thời gian quét (giây)</summary>
        public double ScanDurationSeconds { get; set; }
    }
}
