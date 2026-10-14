using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace frm_ransomrescure.Forms
{
    public class frmDonate : Form
    {
        private TabControl tabs;
        private TabPage    tabMomo;
        private TabPage    tabKofi;
        private PictureBox picMomo;
        private Label      lblKofiTitle;
        private Label      lblKofiDesc;
        private LinkLabel  lnkKofi;

        public frmDonate(bool isEnglish)
        {
            BuildUI();
            ApplyLanguage(isEnglish);
            PositionBottomRight();
        }

        // ── UI ───────────────────────────────────────────────────────────────

        private void BuildUI()
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow; // X đã có sẵn, không cần nút Đóng
            this.StartPosition   = FormStartPosition.Manual;
            this.ShowInTaskbar   = false;
            this.TopMost         = true;
            this.Size            = new Size(300, 340);
            this.BackColor       = Color.FromArgb(30, 30, 30);
            this.ForeColor       = Color.FromArgb(220, 220, 220);
            this.Font            = new Font("Segoe UI", 9f);

            tabs = new TabControl
            {
                Dock     = DockStyle.Fill,
                ItemSize = new Size(130, 24),
            };

            // ── MoMo tab ──────────────────────────────────────────────────────
            tabMomo = new TabPage();
            picMomo = new PictureBox
            {
                Dock      = DockStyle.Fill,
                SizeMode  = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                Image     = LoadMomoImage(),
            };
            tabMomo.Controls.Add(picMomo);

            // ── Ko-fi tab ─────────────────────────────────────────────────────
            tabKofi = new TabPage();
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };

            lblKofiTitle = new Label
            {
                Font      = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 180, 50),
                AutoSize  = false,
                Bounds    = new Rectangle(16, 24, 260, 36),
            };

            lblKofiDesc = new Label
            {
                ForeColor = Color.FromArgb(180, 180, 180),
                AutoSize  = false,
                Bounds    = new Rectangle(16, 68, 260, 90),
            };

            lnkKofi = new LinkLabel
            {
                Text            = "ko-fi.com/thanhnguyen150993",
                LinkColor       = Color.FromArgb(41, 182, 246),
                ActiveLinkColor = Color.White,
                AutoSize        = false,
                Bounds          = new Rectangle(16, 168, 260, 22),
                Font            = new Font("Segoe UI", 9.5f, FontStyle.Underline),
            };
            lnkKofi.LinkClicked += (s, e) =>
                System.Diagnostics.Process.Start("https://ko-fi.com/thanhnguyen150993");

            pnl.Controls.Add(lblKofiTitle);
            pnl.Controls.Add(lblKofiDesc);
            pnl.Controls.Add(lnkKofi);
            tabKofi.Controls.Add(pnl);

            foreach (TabPage tp in new[] { tabMomo, tabKofi })
            {
                tp.BackColor = Color.FromArgb(30, 30, 30);
                tp.ForeColor = Color.FromArgb(220, 220, 220);
            }

            tabs.TabPages.Add(tabMomo);
            tabs.TabPages.Add(tabKofi);
            this.Controls.Add(tabs);
        }

        // ── Ngôn ngữ ─────────────────────────────────────────────────────────

        public void ApplyLanguage(bool isEnglish)
        {
            if (isEnglish)
            {
                this.Text          = "Support the author ☕";
                tabMomo.Text       = "  📱 MoMo QR  ";
                tabKofi.Text       = "  ☕ Ko-fi  ";
                lblKofiTitle.Text  = "☕  Buy me a coffee";
                lblKofiDesc.Text   = "Any contribution, big or small,\nkeeps this tool alive and growing.\n\nClick the link below to open Ko-fi:";
            }
            else
            {
                this.Text          = "Ủng hộ tác giả ☕";
                tabMomo.Text       = "  📱 MoMo QR  ";
                tabKofi.Text       = "  ☕ Ko-fi  ";
                lblKofiTitle.Text  = "☕  Mua cho mình một ly cà phê";
                lblKofiDesc.Text   = "Mọi đóng góp dù nhỏ đều giúp mình\ncó thêm động lực phát triển tool.\n\nClick link bên dưới để mở Ko-fi:";
            }
        }

        private void PositionBottomRight()
        {
            var area = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(
                area.Right  - this.Width  - 12,
                area.Bottom - this.Height - 12);
        }

        // Chặn drag: nuốt WM_NCLBUTTONDOWN trên HTCAPTION (=2), X button dùng HTCLOSE (=20) vẫn OK
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 /*WM_NCLBUTTONDOWN*/ && m.WParam.ToInt32() == 2 /*HTCAPTION*/)
                return;
            base.WndProc(ref m);
        }

        // ── Load MoMo QR từ embedded resource ────────────────────────────────

        private static Image LoadMomoImage()
        {
            try
            {
                var asm = Assembly.GetExecutingAssembly();
                using (var stream = asm.GetManifestResourceStream("frm_ransomrescure.Resources.momo.jpg"))
                {
                    if (stream == null) return null;
                    var ms = new MemoryStream();
                    stream.CopyTo(ms);
                    return Image.FromStream(ms);
                }
            }
            catch { return null; }
        }
    }
}
