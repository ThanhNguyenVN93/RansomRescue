using System;
using System.IO;
using System.Windows.Forms;

namespace frm_ransomrescure.Core
{
    /// <summary>
    /// Phát hiện tool đang chạy từ USB và cảnh báo user.
    /// Ransomware chạy với quyền admin có thể mã hóa USB trong vài giây —
    /// chạy trong Safe Mode sẽ ngăn điều này.
    /// </summary>
    public static class UsbGuard
    {
        /// <summary>
        /// Gọi ngay khi khởi động (trong Program.Main, trước khi mở form).
        /// Nếu đang chạy từ USB → đặt ReadOnly + hiện cảnh báo.
        /// </summary>
        public static void CheckAndWarn()
        {
            try
            {
                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string root    = Path.GetPathRoot(exePath);

                if (string.IsNullOrEmpty(root)) return;

                var driveInfo = new DriveInfo(root);
                if (driveInfo.DriveType == DriveType.Removable)
                {
                    // Cố gắng đặt ReadOnly để ransomware khó ghi đè EXE
                    // (không phải giải pháp tuyệt đối nhưng tạo thêm một lớp bảo vệ)
                    TrySetReadOnly(exePath);

                    ShowUsbWarning(root);
                }
            }
            catch (Exception ex)
            {
                Utils.Logger.Warn($"UsbGuard.CheckAndWarn failed: {ex.Message}");
                // Không block tool chạy nếu check thất bại
            }
        }

        private static void TrySetReadOnly(string filePath)
        {
            try
            {
                FileAttributes attrs = File.GetAttributes(filePath);
                if ((attrs & FileAttributes.ReadOnly) == 0)
                    File.SetAttributes(filePath, attrs | FileAttributes.ReadOnly);
            }
            catch
            {
                // Có thể không có quyền — bỏ qua
            }
        }

        private static void ShowUsbWarning(string usbRoot)
        {
            string msg =
                "⚠️  CẢNH BÁO: Bạn đang chạy RansomRescue từ USB!\n\n" +
                $"Ổ đĩa: {usbRoot}\n\n" +
                "Nếu ransomware còn đang hoạt động (KHÔNG PHẢI Safe Mode),\n" +
                "nó CÓ THỂ mã hóa luôn USB của bạn trong vài giây.\n\n" +
                "✅  Khuyến nghị:\n" +
                "   • Khởi động vào Safe Mode trước khi chạy tool\n" +
                "   • Win 10/11: Shift + Restart → Troubleshoot → Safe Mode\n" +
                "   • Win 7: F8 khi khởi động → Safe Mode\n\n" +
                "Bạn có muốn tiếp tục không?\n" +
                "(Chọn \"Không\" để thoát, vào Safe Mode rồi chạy lại)";

            var answer = MessageBox.Show(
                msg,
                "Cảnh báo USB — RansomRescue",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2  // Mặc định focus vào "Không"
            );

            if (answer == DialogResult.No)
                Environment.Exit(0);
        }
    }
}
