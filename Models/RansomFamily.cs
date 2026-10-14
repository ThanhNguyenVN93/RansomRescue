using System.Collections.Generic;
using Newtonsoft.Json;

namespace frm_ransomrescure.Models
{
    /// <summary>
    /// Đại diện cho một dòng họ ransomware trong db.json
    /// </summary>
    public class RansomFamily
    {
        /// <summary>ID định danh nội bộ, ví dụ: "stop_djvu"</summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>Tên hiển thị, ví dụ: "STOP/Djvu"</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Danh sách phần mở rộng file bị mã hóa, ví dụ: [".djvu", ".puma"]</summary>
        [JsonProperty("extensions")]
        public List<string> Extensions { get; set; } = new List<string>();

        /// <summary>Tên file ghi chú tiền chuộc điển hình, ví dụ: ["_readme.txt"]</summary>
        [JsonProperty("ransom_notes")]
        public List<string> RansomNotes { get; set; } = new List<string>();

        /// <summary>Từ khóa / email trong nội dung ghi chú, dùng để tăng điểm nhận diện</summary>
        [JsonProperty("note_keywords")]
        public List<string> NoteKeywords { get; set; } = new List<string>();

        /// <summary>true = có công cụ giải mã miễn phí (dù có điều kiện)</summary>
        [JsonProperty("decryptor_available")]
        public bool DecryptorAvailable { get; set; }

        /// <summary>true = công cụ chỉ hoạt động trong một số điều kiện nhất định</summary>
        [JsonProperty("decryptor_conditional")]
        public bool DecryptorConditional { get; set; }

        /// <summary>
        /// Mô tả điều kiện giải mã, ví dụ:
        /// "Chỉ giải mã được khi dùng offline key (~30% trường hợp)"
        /// </summary>
        [JsonProperty("decryptor_condition")]
        public string DecryptorCondition { get; set; }

        /// <summary>Tên công cụ giải mã, ví dụ: "Emsisoft STOP Djvu Decryptor"</summary>
        [JsonProperty("decryptor_tool_name")]
        public string DecryptorToolName { get; set; }

        /// <summary>Link tải công cụ giải mã</summary>
        [JsonProperty("decryptor_url")]
        public string DecryptorUrl { get; set; }

        /// <summary>
        /// ID xử lý đặc biệt nếu cần, ví dụ: "stop_djvu_key_check", "wannacry_ram_key"
        /// Để trống nếu không có logic đặc biệt
        /// </summary>
        [JsonProperty("special_handling")]
        public string SpecialHandling { get; set; }

        /// <summary>
        /// Magic bytes (8 hex chars, không có space) của file bị mã hóa.
        /// Ví dụ: ["57414E414352592100"] cho WannaCry ("WANACRY!\0")
        /// </summary>
        [JsonProperty("magic_bytes")]
        public List<string> MagicBytes { get; set; } = new List<string>();

        /// <summary>Ghi chú thêm hiển thị cho user (ví dụ: cảnh báo biến thể cũ/mới)</summary>
        [JsonProperty("note")]
        public string Note { get; set; }
    }
}
