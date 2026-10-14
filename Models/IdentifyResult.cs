using System.Collections.Generic;

namespace frm_ransomrescure.Models
{
    /// <summary>
    /// Kết quả nhận diện dòng họ ransomware sau khi chấm điểm
    /// </summary>
    public class IdentifyResult
    {
        // ── Kết quả chính ──────────────────────────────────────────────────────

        /// <summary>Dòng họ có điểm cao nhất (null nếu không nhận diện được)</summary>
        public RansomFamily TopFamily { get; set; }

        /// <summary>
        /// Điểm tổng của TopFamily:
        ///   Extension match db.json  → +50đ
        ///   Ransom note filename      → +30đ
        ///   Note keyword/email match  → +20đ
        ///   Tối đa: 100đ
        /// </summary>
        public int TopScore { get; set; }

        /// <summary>
        /// true  = XÁC ĐỊNH (score >= 75)
        /// false = NGHI NGỜ (score < 75)
        /// </summary>
        public bool IsConfirmed => TopScore >= 75;

        /// <summary>
        /// Số lượng dòng họ khác cũng khớp một phần (score > 0, không phải top).
        /// Hiển thị: "+ 2 dòng họ khác trùng một phần"
        /// </summary>
        public int OtherMatchCount { get; set; }

        // ── Chi tiết điểm (để debug / hiển thị nâng cao) ──────────────────────

        /// <summary>Điểm từ việc khớp phần mở rộng với db.json (+50 hoặc 0)</summary>
        public int ScoreFromExtension { get; set; }

        /// <summary>Điểm từ việc tìm thấy file ghi chú ransom đúng tên (+30 hoặc 0)</summary>
        public int ScoreFromRansomNote { get; set; }

        /// <summary>Điểm từ keyword/email trong nội dung ghi chú (+20 hoặc 0)</summary>
        public int ScoreFromKeyword { get; set; }

        // ── Trạng thái ────────────────────────────────────────────────────────

        /// <summary>true = không nhận diện được gì (score = 0 với mọi family)</summary>
        public bool IsUnknown => TopFamily == null || TopScore == 0;

        /// <summary>Thông báo hiển thị cho user khi IsUnknown = true</summary>
        public string UnknownReason { get; set; }

        /// <summary>Danh sách tất cả các family có score > 0, sắp xếp giảm dần</summary>
        public List<FamilyScore> AllScores { get; set; } = new List<FamilyScore>();
    }

    /// <summary>Điểm của một dòng họ cụ thể — dùng để hiển thị danh sách trùng khớp</summary>
    public class FamilyScore
    {
        public RansomFamily Family { get; set; }
        public int Score { get; set; }
        public bool IsConfirmed => Score >= 75;
    }
}
