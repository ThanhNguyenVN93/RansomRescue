using System;

namespace frm_ransomrescure.Utils
{
    /// <summary>
    /// Đọc thời gian máy đã bật liên tục (uptime).
    /// Dùng để phát hiện khả năng user vừa restart sau khi nhiễm.
    /// </summary>
    public static class UptimeHelper
    {
        /// <summary>
        /// Lấy uptime hiện tại của máy.
        /// Dùng Environment.TickCount (int, wrap sau ~25 ngày) cho .NET 4.6.1.
        /// Với máy bật lâu hơn 25 ngày, kết quả không chính xác — chấp nhận được
        /// vì mục đích chỉ là phát hiện "vừa restart" (< 10 phút).
        /// </summary>
        public static TimeSpan GetUptime()
        {
            // Environment.TickCount là số ms kể từ lúc Windows khởi động
            // Giá trị có thể âm sau ~24.9 ngày do int overflow → dùng unchecked
            int tickCount = Environment.TickCount;
            long ms = unchecked((uint)tickCount); // chuyển sang uint để tránh âm
            return TimeSpan.FromMilliseconds(ms);
        }

        /// <summary>
        /// Kiểm tra xem máy có vẻ vừa được restart gần đây không.
        /// Ngưỡng mặc định: 10 phút.
        /// </summary>
        /// <param name="thresholdMinutes">Ngưỡng thời gian tính bằng phút</param>
        public static bool IsRecentlyRestarted(double thresholdMinutes = 10.0)
        {
            return GetUptime().TotalMinutes < thresholdMinutes;
        }

        /// <summary>
        /// Định dạng uptime thành chuỗi thân thiện để hiển thị.
        /// Ví dụ: "4 phút 12 giây", "2 giờ 15 phút"
        /// </summary>
        public static string FormatUptime()
        {
            TimeSpan uptime = GetUptime();

            if (uptime.TotalSeconds < 60)
                return $"{(int)uptime.TotalSeconds} giây";

            if (uptime.TotalMinutes < 60)
                return $"{(int)uptime.TotalMinutes} phút {uptime.Seconds} giây";

            if (uptime.TotalHours < 24)
                return $"{(int)uptime.TotalHours} giờ {uptime.Minutes} phút";

            return $"{(int)uptime.TotalDays} ngày {uptime.Hours} giờ";
        }
    }
}
