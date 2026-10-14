using System;
using System.Collections.Generic;
using System.Linq;
using frm_ransomrescure.Models;
using frm_ransomrescure.Utils;

namespace frm_ransomrescure.Core
{
    /// <summary>
    /// Nhận diện dòng họ ransomware từ bằng chứng thu thập được.
    ///
    /// Hệ thống điểm:
    ///   Extension khớp db.json  → +50đ  (primary signal)
    ///   Ransom note filename     → +30đ  (strong signal)
    ///   Note keyword/email       → +20đ  (tiebreaker + confirmation)
    ///   Magic bytes khớp         → +15đ  (xác nhận thêm)
    ///   ─────────────────────────────
    ///   Tổng tối đa             → 115đ
    ///
    ///   ≥ 75đ = XÁC ĐỊNH
    ///   &lt; 75đ = NGHI NGỜ
    ///
    /// Lý do ngưỡng 75đ:
    ///   Cho phép xác định khi ransom note đã bị xóa:
    ///   ext 50đ + keyword 20đ = 70đ → NGHI NGỜ (đúng, vì thiếu file note)
    ///   ext 50đ + note 30đ         = 80đ → XÁC ĐỊNH
    ///   ext 50đ + note 30đ + kw    = 100đ → XÁC ĐỊNH (full confidence)
    /// </summary>
    public static class Identifier
    {
        // Điểm tối thiểu để được tính là "có trùng khớp"
        private const int MinScore       = 10;
        private const int ConfirmThreshold = 75;

        // Trọng số điểm
        private const int ScoreExtension  = 50;
        private const int ScoreNoteName   = 30;
        private const int ScoreKeyword    = 20;
        private const int ScoreMagicBytes = 15;

        /// <summary>
        /// Nhận diện dòng họ ransomware từ ScanResult và danh sách families trong DB.
        /// </summary>
        public static IdentifyResult Identify(ScanResult scan, List<RansomFamily> families)
        {
            if (scan == null)    throw new ArgumentNullException(nameof(scan));
            if (families == null) throw new ArgumentNullException(nameof(families));

            var scores = new List<FamilyScore>();

            foreach (var family in families)
            {
                int score = 0;
                int scoreExt  = 0;
                int scoreNote = 0;
                int scoreKw   = 0;

                // ── +50đ: Extension khớp db.json ─────────────────────────────
                if (family.Extensions != null && family.Extensions.Count > 0)
                {
                    foreach (var ext in family.Extensions)
                    {
                        if (scan.ExtensionFrequency.ContainsKey(ext))
                        {
                            scoreExt = ScoreExtension; // Cộng 1 lần dù có nhiều ext khớp
                            break;
                        }
                    }
                }

                // ── +30đ: Ransom note filename ────────────────────────────────
                if (family.RansomNotes != null && family.RansomNotes.Count > 0
                    && scan.RansomNotePaths.Count > 0)
                {
                    foreach (var noteName in family.RansomNotes)
                    {
                        bool found = scan.RansomNotePaths.Any(p =>
                            string.Equals(
                                System.IO.Path.GetFileName(p),
                                noteName,
                                StringComparison.OrdinalIgnoreCase));

                        if (found)
                        {
                            scoreNote = ScoreNoteName;
                            break;
                        }
                    }
                }

                // ── +20đ: Keyword/email trong nội dung note ───────────────────
                if (family.NoteKeywords != null && family.NoteKeywords.Count > 0
                    && scan.RansomNoteContents.Count > 0)
                {
                    foreach (var keyword in family.NoteKeywords)
                    {
                        bool found = scan.RansomNoteContents.Values.Any(content =>
                            content.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);

                        if (found)
                        {
                            scoreKw = ScoreKeyword;
                            break;
                        }
                    }
                }

                // ── +15đ: Magic bytes khớp ────────────────────────────────────
                int scoreMagic = 0;
                if (family.MagicBytes != null && family.MagicBytes.Count > 0
                    && scan.MagicByteEvidence.Count > 0)
                {
                    foreach (var pattern in family.MagicBytes)
                    {
                        string normalized = pattern.Replace(" ", "").Replace("-", "").ToUpperInvariant();
                        bool found = scan.MagicByteEvidence.Keys.Any(k =>
                            k.StartsWith(normalized, StringComparison.OrdinalIgnoreCase));
                        if (found) { scoreMagic = ScoreMagicBytes; break; }
                    }
                }

                score = scoreExt + scoreNote + scoreKw + scoreMagic;

                if (score >= MinScore)
                {
                    scores.Add(new FamilyScore
                    {
                        Family            = family,
                        Score             = score,
                        // Expose breakdown (không dùng trong model hiện tại, để debug)
                    });
                }
            }

            // Sắp xếp giảm dần theo điểm
            scores.Sort((a, b) => b.Score.CompareTo(a.Score));

            Logger.Info($"Identifier: {scores.Count} families có score >= {MinScore}");

            // ── Build result ─────────────────────────────────────────────────
            var result = new IdentifyResult { AllScores = scores };

            if (scores.Count == 0)
            {
                result.UnknownReason = DetermineUnknownReason(scan);
                return result;
            }

            var top = scores[0];
            result.TopFamily  = top.Family;
            result.TopScore   = top.Score;
            result.OtherMatchCount = scores.Count - 1; // Không tính top

            // Breakdown điểm cho top family (tính lại)
            result.ScoreFromExtension  = ComputeExtScore(top.Family, scan);
            result.ScoreFromRansomNote = ComputeNoteScore(top.Family, scan);
            result.ScoreFromKeyword    = ComputeKwScore(top.Family, scan);

            Logger.Info($"Identifier: Top = {top.Family.Name} ({top.Score}đ), " +
                        $"Others = {result.OtherMatchCount}, " +
                        $"Confirmed = {result.IsConfirmed}");

            return result;
        }

        // ── Private helpers ──────────────────────────────────────────────────────

        private static int ComputeExtScore(RansomFamily f, ScanResult s)
        {
            if (f.Extensions == null) return 0;
            return f.Extensions.Any(e => s.ExtensionFrequency.ContainsKey(e))
                   ? ScoreExtension : 0;
        }

        private static int ComputeNoteScore(RansomFamily f, ScanResult s)
        {
            if (f.RansomNotes == null || s.RansomNotePaths.Count == 0) return 0;
            return f.RansomNotes.Any(n =>
                       s.RansomNotePaths.Any(p =>
                           string.Equals(System.IO.Path.GetFileName(p), n,
                                         StringComparison.OrdinalIgnoreCase)))
                   ? ScoreNoteName : 0;
        }

        private static int ComputeKwScore(RansomFamily f, ScanResult s)
        {
            if (f.NoteKeywords == null || s.RansomNoteContents.Count == 0) return 0;
            return f.NoteKeywords.Any(kw =>
                       s.RansomNoteContents.Values.Any(c =>
                           c.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0))
                   ? ScoreKeyword : 0;
        }

        private static string DetermineUnknownReason(ScanResult scan)
        {
            if (scan.RansomNotePaths.Count == 0 && scan.ExtensionFrequency.Count == 0)
                return "Không tìm thấy bằng chứng rõ ràng. Ransomware có thể đã tự xóa dấu vết, " +
                       "hoặc máy chưa bị nhiễm ransomware.";

            if (scan.ExtensionFrequency.Count > 0 && scan.RansomNotePaths.Count == 0)
                return "Tìm thấy file khả nghi nhưng không có ghi chú ransom. " +
                       "Dòng họ này có thể chưa có trong cơ sở dữ liệu. " +
                       "Thử gửi mẫu lên ID-Ransomware để xác định chính xác hơn.";

            return "Không thể nhận diện chính xác. Thử gửi mẫu lên ID-Ransomware.";
        }
    }
}
