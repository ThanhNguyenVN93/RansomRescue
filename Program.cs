using System;
using System.Windows.Forms;
using frm_ransomrescure.Forms;
using frm_ransomrescure.Core;

namespace frm_ransomrescure
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Kiểm tra chạy từ USB → cảnh báo trước khi mở form chính
            UsbGuard.CheckAndWarn();

            Application.Run(new frmMain());
        }
    }
}
