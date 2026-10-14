namespace frm_ransomrescure.Forms
{
    partial class frmMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components         = new System.ComponentModel.Container();
            this.tabMain            = new System.Windows.Forms.TabControl();
            this.tabWarning         = new System.Windows.Forms.TabPage();
            this.tabScan            = new System.Windows.Forms.TabPage();
            this.tabResult          = new System.Windows.Forms.TabPage();
            this.statusStrip        = new System.Windows.Forms.StatusStrip();
            this.lblDbStatus        = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlDrives          = new System.Windows.Forms.Panel();
            this.lblDrivesHeader    = new System.Windows.Forms.Label();
            this.flowDrives         = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlSpecialHandling = new System.Windows.Forms.Panel();
            this.lblSpecialIcon     = new System.Windows.Forms.Label();
            this.lblSpecialTitle    = new System.Windows.Forms.Label();
            this.lblSpecialBody     = new System.Windows.Forms.Label();
            this.lblInfectionTime   = new System.Windows.Forms.Label();
            this.pnlUptimeWarning   = new System.Windows.Forms.Panel();
            this.lblUptimeWarning   = new System.Windows.Forms.Label();
            this.pnlWarningBody     = new System.Windows.Forms.Panel();
            this.pnlWarningFooter   = new System.Windows.Forms.Panel();
            this.lblWarningTitle    = new System.Windows.Forms.Label();
            this.lblWarningText     = new System.Windows.Forms.Label();
            this.chkDownloadKvrt    = new System.Windows.Forms.CheckBox();
            this.chkConfirm         = new System.Windows.Forms.CheckBox();
            this.btnStartScan       = new System.Windows.Forms.Button();
            this.pnlScanTop         = new System.Windows.Forms.Panel();
            this.chkDeepScan        = new System.Windows.Forms.CheckBox();
            this.btnScan            = new System.Windows.Forms.Button();
            this.progressBar        = new System.Windows.Forms.ProgressBar();
            this.lblCurrentPath     = new System.Windows.Forms.Label();
            this.lblScanStatus      = new System.Windows.Forms.Label();
            this.pnlScanStats       = new System.Windows.Forms.Panel();
            this.lblScanned         = new System.Windows.Forms.Label();
            this.lblSuspect         = new System.Windows.Forms.Label();
            this.lblNotes           = new System.Windows.Forms.Label();
            this.lblLogHeader       = new System.Windows.Forms.Label();
            this.rtbLog             = new System.Windows.Forms.RichTextBox();
            this.pnlResultCard      = new System.Windows.Forms.Panel();
            this.lblResultTitle     = new System.Windows.Forms.Label();
            this.lblFamilyName      = new System.Windows.Forms.Label();
            this.lblScore           = new System.Windows.Forms.Label();
            this.lblEncryptedCount  = new System.Windows.Forms.Label();
            this.lblNoteCount       = new System.Windows.Forms.Label();
            this.lblOtherMatch      = new System.Windows.Forms.Label();
            this.pnlDecryptor       = new System.Windows.Forms.Panel();
            this.lblDecryptorHeader = new System.Windows.Forms.Label();
            this.lblToolName        = new System.Windows.Forms.Label();
            this.lnkDecryptorUrl    = new System.Windows.Forms.LinkLabel();
            this.btnDownloadTool    = new System.Windows.Forms.Button();
            this.pnlCondition       = new System.Windows.Forms.Panel();
            this.lblCondition       = new System.Windows.Forms.Label();
            this.lblFamilyNote      = new System.Windows.Forms.Label();
            this.pnlSubmit          = new System.Windows.Forms.Panel();
            this.lblSubmitHeader    = new System.Windows.Forms.Label();
            this.btnSubmitOnline    = new System.Windows.Forms.Button();
            this.lblSubmitStatus    = new System.Windows.Forms.Label();
            this.pnlEmailFallback   = new System.Windows.Forms.Panel();
            this.lblEmailHeader     = new System.Windows.Forms.Label();
            this.lblEmailTo         = new System.Windows.Forms.Label();
            this.txtEmailTo         = new System.Windows.Forms.TextBox();
            this.lblEmailSubject    = new System.Windows.Forms.Label();
            this.txtEmailSubject    = new System.Windows.Forms.TextBox();
            this.lblEmailBody       = new System.Windows.Forms.Label();
            this.txtEmailBody       = new System.Windows.Forms.RichTextBox();
            this.pnlEmailButtons    = new System.Windows.Forms.Panel();
            this.btnCopyEmail       = new System.Windows.Forms.Button();
            this.btnOpenMail        = new System.Windows.Forms.Button();
            this.btnOpenBrowser     = new System.Windows.Forms.Button();
            this.pnlNextSteps       = new System.Windows.Forms.Panel();
            this.lblNextHeader      = new System.Windows.Forms.Label();
            this.lblNextSteps       = new System.Windows.Forms.Label();
            this.btnRunKvrt         = new System.Windows.Forms.Button();
            this.btnSaveReport      = new System.Windows.Forms.Button();
            this.pbKvrt             = new System.Windows.Forms.ToolStripProgressBar();
            this.btnDarkMode        = new System.Windows.Forms.ToolStripButton();
            this.btnLangToggle      = new System.Windows.Forms.ToolStripButton();
            this.btnDonate          = new System.Windows.Forms.ToolStripButton();
            this.toolTip1           = new System.Windows.Forms.ToolTip(this.components);
            this.pnlCandidates      = new System.Windows.Forms.Panel();
            this.lblCandidatesHdr   = new System.Windows.Forms.Label();
            this.lstCandidates      = new System.Windows.Forms.ListBox();

            this.pnlCandidates.SuspendLayout();
            this.pnlDrives.SuspendLayout();
            this.pnlSpecialHandling.SuspendLayout();
            this.pnlWarningFooter.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabWarning.SuspendLayout();
            this.tabScan.SuspendLayout();
            this.tabResult.SuspendLayout();
            this.SuspendLayout();

            // ── statusStrip ──────────────────────────────────────────────────────
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.lblDbStatus, this.pbKvrt, this.btnDarkMode, this.btnLangToggle, this.btnDonate });
            this.statusStrip.Name       = "statusStrip";
            this.statusStrip.SizingGrip = false;
            this.lblDbStatus.Name      = "lblDbStatus";
            this.lblDbStatus.Text      = "CSDL: Đang tải...";
            this.lblDbStatus.Spring    = true;
            this.lblDbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.pbKvrt.Name    = "pbKvrt";
            this.pbKvrt.Size    = new System.Drawing.Size(120, 14);
            this.pbKvrt.Visible = false;

            this.btnDarkMode.CheckOnClick = true;
            this.btnDarkMode.Name         = "btnDarkMode";
            this.btnDarkMode.Text         = "🌙";
            this.btnDarkMode.ToolTipText  = "Chuyển Dark / Light Mode";
            this.btnDarkMode.Click       += new System.EventHandler(this.btnDarkMode_Click);

            this.btnLangToggle.CheckOnClick = true;
            this.btnLangToggle.Name         = "btnLangToggle";
            this.btnLangToggle.Text         = "EN";
            this.btnLangToggle.ToolTipText  = "Switch to English / Tiếng Việt";
            this.btnLangToggle.Click       += new System.EventHandler(this.btnLangToggle_Click);

            this.btnDonate.Name        = "btnDonate";
            this.btnDonate.Text        = "☕ Donate";
            this.btnDonate.ToolTipText = "Ủng hộ tác giả — MoMo / Ko-fi";
            this.btnDonate.Click      += new System.EventHandler(this.btnDonate_Click);

            // ── tabMain ──────────────────────────────────────────────────────────
            this.tabMain.Controls.Add(this.tabWarning);
            this.tabMain.Controls.Add(this.tabScan);
            this.tabMain.Controls.Add(this.tabResult);
            this.tabMain.Dock          = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Name          = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.TabIndex      = 0;
            this.tabMain.Font          = new System.Drawing.Font("Segoe UI", 10F);
            this.tabMain.Selecting     += new System.Windows.Forms.TabControlCancelEventHandler(this.tabMain_Selecting);

            // ════════════════════════════════════════════════════════════════════
            // TAB 0 — Cảnh báo
            // ════════════════════════════════════════════════════════════════════
            // Thứ tự add vào Controls quan trọng cho Dock layout:
            // pnlWarningFooter (Dock=Bottom) add sau cùng → Z-order thấp → dock trước
            // pnlWarningBody (Dock=Fill) → fill phần còn lại
            // pnlUptimeWarning (absolute) → overlay lên trên khi cần
            this.tabWarning.Controls.Add(this.pnlUptimeWarning);
            this.tabWarning.Controls.Add(this.pnlWarningBody);
            this.tabWarning.Controls.Add(this.pnlWarningFooter);
            this.tabWarning.Name    = "tabWarning";
            this.tabWarning.Padding = new System.Windows.Forms.Padding(0);
            this.tabWarning.TabIndex = 0;
            this.tabWarning.Text    = "  ⚠  Cảnh báo  ";
            this.tabWarning.UseVisualStyleBackColor = true;

            // pnlUptimeWarning — banner vàng overlay, absolute vị trí
            this.pnlUptimeWarning.Controls.Add(this.lblUptimeWarning);
            this.pnlUptimeWarning.Anchor    = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.pnlUptimeWarning.BackColor = System.Drawing.Color.FromArgb(255, 243, 199);
            this.pnlUptimeWarning.Location  = new System.Drawing.Point(0, 0);
            this.pnlUptimeWarning.Name      = "pnlUptimeWarning";
            this.pnlUptimeWarning.Size      = new System.Drawing.Size(920, 40);
            this.pnlUptimeWarning.TabIndex  = 10;
            this.pnlUptimeWarning.Visible   = false;

            this.lblUptimeWarning.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.lblUptimeWarning.ForeColor = System.Drawing.Color.FromArgb(146, 64, 14);
            this.lblUptimeWarning.Name      = "lblUptimeWarning";
            this.lblUptimeWarning.Padding   = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblUptimeWarning.Text      = "⚠️  Máy vừa được khởi động lại — Hãy xem mục 2 ở dưới.";
            this.lblUptimeWarning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // pnlWarningBody — Dock=Fill: phần thân chứa nội dung cảnh báo
            this.pnlWarningBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWarningBody.Controls.Add(this.lblWarningTitle);
            this.pnlWarningBody.Controls.Add(this.lblWarningText);
            this.pnlWarningBody.Name     = "pnlWarningBody";
            this.pnlWarningBody.TabIndex = 1;

            this.lblWarningTitle.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.lblWarningTitle.AutoSize  = false;
            this.lblWarningTitle.Font      = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblWarningTitle.ForeColor = System.Drawing.Color.FromArgb(185, 28, 28);
            this.lblWarningTitle.Location  = new System.Drawing.Point(16, 16);
            this.lblWarningTitle.Name      = "lblWarningTitle";
            this.lblWarningTitle.Size      = new System.Drawing.Size(860, 36);
            this.lblWarningTitle.Text      = "🛡  RansomRescue — Đọc kỹ trước khi sử dụng";

            this.lblWarningText.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right
                | System.Windows.Forms.AnchorStyles.Bottom);
            this.lblWarningText.AutoSize   = false;
            this.lblWarningText.Location   = new System.Drawing.Point(16, 64);
            this.lblWarningText.Name       = "lblWarningText";
            this.lblWarningText.Size       = new System.Drawing.Size(860, 380);
            this.lblWarningText.Text       =
                "TRƯỚC KHI CHẠY TOOL NÀY, HÃY ĐẢM BẢO:\r\n\r\n" +
                "1.  Ngắt kết nối mạng ngay lập tức (rút cáp LAN / tắt WiFi) để ngăn ransomware lây lan.\r\n\r\n" +
                "2.  KHÔNG khởi động lại hoặc tắt máy — một số ransomware sẽ xóa shadow copy khi shutdown.\r\n\r\n" +
                "3.  Cắm USB đã cài tool này vào máy bị nhiễm và chạy trực tiếp từ USB " +
                "(không copy sang ổ cứng bị nhiễm).\r\n\r\n" +
                "4.  Tool này CHỈ xác định dòng họ ransomware và tìm công cụ giải mã.\r\n" +
                "    Tool KHÔNG giải mã, KHÔNG xóa virus, KHÔNG thay thế phần mềm diệt virus.\r\n\r\n" +
                "5.  Sau khi xác định được dòng họ, hãy sử dụng công cụ giải mã chính hãng " +
                "từ No More Ransom (nomoreransom.org) hoặc hãng bảo mật uy tín.\r\n\r\n" +
                "6.  Chạy Kaspersky Virus Removal Tool (KVRT) để loại bỏ ransomware trước khi giải mã.\r\n\r\n" +
                "Tool này miễn phí và mã nguồn mở. Không thu thập dữ liệu cá nhân.\r\n" +
                "Phiên bản: 1.0 | Nhà phát triển: ThanhCode93";
            this.lblWarningText.TextAlign  = System.Drawing.ContentAlignment.TopLeft;

            // pnlWarningFooter — Dock=Bottom: luôn hiển thị ở dưới cùng
            this.pnlWarningFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlWarningFooter.Controls.Add(this.chkDownloadKvrt);
            this.pnlWarningFooter.Controls.Add(this.chkConfirm);
            this.pnlWarningFooter.Controls.Add(this.btnStartScan);
            this.pnlWarningFooter.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            this.pnlWarningFooter.Name      = "pnlWarningFooter";
            this.pnlWarningFooter.Size      = new System.Drawing.Size(920, 98);
            this.pnlWarningFooter.TabIndex  = 2;

            this.chkDownloadKvrt.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.chkDownloadKvrt.AutoSize = false;
            this.chkDownloadKvrt.Location = new System.Drawing.Point(16, 10);
            this.chkDownloadKvrt.Name     = "chkDownloadKvrt";
            this.chkDownloadKvrt.Size     = new System.Drawing.Size(870, 24);
            this.chkDownloadKvrt.TabIndex = 0;
            this.chkDownloadKvrt.Text     = "Tải Kaspersky KVRT ngầm trong lúc quét (~150 MB, cần internet) — lưu vào %TEMP%\\kvrt.exe";

            this.chkConfirm.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left);
            this.chkConfirm.AutoSize  = false;
            this.chkConfirm.Font      = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.chkConfirm.Location  = new System.Drawing.Point(16, 46);
            this.chkConfirm.Name      = "chkConfirm";
            this.chkConfirm.Size      = new System.Drawing.Size(620, 26);
            this.chkConfirm.TabIndex  = 1;
            this.chkConfirm.Text      = "Tôi đã đọc và hiểu các cảnh báo trên. Tôi muốn tiếp tục.";
            this.chkConfirm.CheckedChanged += new System.EventHandler(this.chkConfirm_CheckedChanged);

            this.btnStartScan.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Right);
            this.btnStartScan.BackColor = System.Drawing.Color.FromArgb(0, 120, 212);
            this.btnStartScan.Enabled   = false;
            this.btnStartScan.FlatAppearance.BorderSize = 0;
            this.btnStartScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartScan.Font      = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnStartScan.ForeColor = System.Drawing.Color.White;
            this.btnStartScan.Location  = new System.Drawing.Point(656, 38);
            this.btnStartScan.Name      = "btnStartScan";
            this.btnStartScan.Size      = new System.Drawing.Size(240, 44);
            this.btnStartScan.TabIndex  = 2;
            this.btnStartScan.Text      = "Tiếp theo  →";
            this.btnStartScan.Click    += new System.EventHandler(this.btnStartScan_Click);

            // ════════════════════════════════════════════════════════════════════
            // TAB 1 — Quét
            // ════════════════════════════════════════════════════════════════════
            this.tabScan.Controls.Add(this.pnlScanTop);
            this.tabScan.Controls.Add(this.pnlDrives);
            this.tabScan.Controls.Add(this.progressBar);
            this.tabScan.Controls.Add(this.lblCurrentPath);
            this.tabScan.Controls.Add(this.lblScanStatus);
            this.tabScan.Controls.Add(this.pnlScanStats);
            this.tabScan.Controls.Add(this.lblLogHeader);
            this.tabScan.Controls.Add(this.rtbLog);
            this.tabScan.Name     = "tabScan";
            this.tabScan.Padding  = new System.Windows.Forms.Padding(12);
            this.tabScan.TabIndex = 1;
            this.tabScan.Text     = "  🔍  Quét  ";
            this.tabScan.UseVisualStyleBackColor = true;

            // pnlScanTop
            this.pnlScanTop.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.pnlScanTop.Controls.Add(this.btnScan);
            this.pnlScanTop.Controls.Add(this.chkDeepScan);
            this.pnlScanTop.Location = new System.Drawing.Point(12, 12);
            this.pnlScanTop.Name     = "pnlScanTop";
            this.pnlScanTop.Size     = new System.Drawing.Size(868, 50);
            this.pnlScanTop.TabIndex = 0;

            this.btnScan.BackColor = System.Drawing.Color.FromArgb(0, 120, 212);
            this.btnScan.FlatAppearance.BorderSize = 0;
            this.btnScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScan.Font      = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnScan.ForeColor = System.Drawing.Color.White;
            this.btnScan.Location  = new System.Drawing.Point(0, 5);
            this.btnScan.Name      = "btnScan";
            this.btnScan.Size      = new System.Drawing.Size(220, 40);
            this.btnScan.TabIndex  = 0;
            this.btnScan.Text      = "▶  Bắt đầu quét";
            this.btnScan.Click    += new System.EventHandler(this.btnScan_Click);

            this.chkDeepScan.AutoSize = true;
            this.chkDeepScan.Location = new System.Drawing.Point(240, 13);
            this.chkDeepScan.Name     = "chkDeepScan";
            this.chkDeepScan.TabIndex = 1;
            this.chkDeepScan.Text     = "Quét sâu (chậm hơn)";

            // pnlDrives — chọn ổ đĩa cần quét
            this.pnlDrives.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.pnlDrives.Controls.Add(this.lblDrivesHeader);
            this.pnlDrives.Controls.Add(this.flowDrives);
            this.pnlDrives.Location = new System.Drawing.Point(12, 68);
            this.pnlDrives.Name     = "pnlDrives";
            this.pnlDrives.Size     = new System.Drawing.Size(868, 34);
            this.pnlDrives.TabIndex = 10;

            this.lblDrivesHeader.AutoSize = true;
            this.lblDrivesHeader.Font     = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDrivesHeader.Location = new System.Drawing.Point(0, 8);
            this.lblDrivesHeader.Name     = "lblDrivesHeader";
            this.lblDrivesHeader.Text     = "Ổ đĩa:";

            this.flowDrives.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.flowDrives.AutoSize  = true;
            this.flowDrives.Location  = new System.Drawing.Point(52, 2);
            this.flowDrives.Name      = "flowDrives";
            this.flowDrives.Size      = new System.Drawing.Size(810, 30);
            this.flowDrives.TabIndex  = 1;
            this.flowDrives.WrapContents = false;

            // progressBar
            this.progressBar.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.progressBar.Location = new System.Drawing.Point(12, 110);
            this.progressBar.Name     = "progressBar";
            this.progressBar.Size     = new System.Drawing.Size(868, 20);
            this.progressBar.TabIndex = 1;
            this.progressBar.Style    = System.Windows.Forms.ProgressBarStyle.Continuous;

            // lblCurrentPath
            this.lblCurrentPath.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.lblCurrentPath.AutoSize  = false;
            this.lblCurrentPath.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCurrentPath.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblCurrentPath.Location  = new System.Drawing.Point(12, 136);
            this.lblCurrentPath.Name      = "lblCurrentPath";
            this.lblCurrentPath.Size      = new System.Drawing.Size(868, 20);
            this.lblCurrentPath.Text      = "Chưa quét";

            // lblScanStatus
            this.lblScanStatus.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.lblScanStatus.AutoSize  = false;
            this.lblScanStatus.Font      = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblScanStatus.Location  = new System.Drawing.Point(12, 160);
            this.lblScanStatus.Name      = "lblScanStatus";
            this.lblScanStatus.Size      = new System.Drawing.Size(868, 24);
            this.lblScanStatus.Text      = "Sẵn sàng quét.";

            // pnlScanStats
            this.pnlScanStats.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.pnlScanStats.Controls.Add(this.lblScanned);
            this.pnlScanStats.Controls.Add(this.lblSuspect);
            this.pnlScanStats.Controls.Add(this.lblNotes);
            this.pnlScanStats.Location = new System.Drawing.Point(12, 190);
            this.pnlScanStats.Name     = "pnlScanStats";
            this.pnlScanStats.Size     = new System.Drawing.Size(868, 30);
            this.pnlScanStats.TabIndex = 2;

            this.lblScanned.AutoSize = true;
            this.lblScanned.Location = new System.Drawing.Point(0, 5);
            this.lblScanned.Name     = "lblScanned";
            this.lblScanned.Text     = "File đã quét: 0";

            this.lblSuspect.AutoSize = true;
            this.lblSuspect.Location = new System.Drawing.Point(220, 5);
            this.lblSuspect.Name     = "lblSuspect";
            this.lblSuspect.Text     = "Nghi ngờ: 0";

            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(400, 5);
            this.lblNotes.Name     = "lblNotes";
            this.lblNotes.Text     = "Ghi chú ransom: 0";

            this.lblLogHeader.AutoSize  = false;
            this.lblLogHeader.Font      = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLogHeader.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.lblLogHeader.Location  = new System.Drawing.Point(12, 228);
            this.lblLogHeader.Name      = "lblLogHeader";
            this.lblLogHeader.Size      = new System.Drawing.Size(200, 20);
            this.lblLogHeader.Text      = "Nhật ký quét:";

            // rtbLog — RichTextBox có màu, stretch cả 4 phía
            this.rtbLog.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right
                | System.Windows.Forms.AnchorStyles.Bottom);
            this.rtbLog.BackColor          = System.Drawing.Color.FromArgb(15, 15, 15);
            this.rtbLog.BorderStyle        = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbLog.DetectUrls         = false;
            this.rtbLog.Font               = new System.Drawing.Font("Consolas", 9F);
            this.rtbLog.ForeColor          = System.Drawing.Color.FromArgb(212, 212, 212);
            this.rtbLog.Location           = new System.Drawing.Point(12, 250);
            this.rtbLog.Name               = "rtbLog";
            this.rtbLog.ReadOnly           = true;
            this.rtbLog.ScrollBars         = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbLog.Size               = new System.Drawing.Size(868, 320);
            this.rtbLog.TabIndex           = 3;
            this.rtbLog.WordWrap           = false;

            // ════════════════════════════════════════════════════════════════════
            // TAB 2 — Kết quả
            // ════════════════════════════════════════════════════════════════════
            this.tabResult.AutoScroll = true;
            this.tabResult.Controls.Add(this.pnlResultCard);
            this.tabResult.Controls.Add(this.lblInfectionTime);
            this.tabResult.Controls.Add(this.pnlSpecialHandling);
            this.tabResult.Controls.Add(this.pnlDecryptor);
            this.tabResult.Controls.Add(this.pnlCondition);
            this.tabResult.Controls.Add(this.lblFamilyNote);
            this.tabResult.Controls.Add(this.pnlSubmit);
            this.tabResult.Controls.Add(this.pnlEmailFallback);
            this.tabResult.Controls.Add(this.pnlNextSteps);
            this.tabResult.Controls.Add(this.btnRunKvrt);
            this.tabResult.Controls.Add(this.pnlCandidates);
            this.tabResult.Name     = "tabResult";
            this.tabResult.Padding  = new System.Windows.Forms.Padding(12);
            this.tabResult.TabIndex = 2;
            this.tabResult.Text     = "  📋  Kết quả  ";
            this.tabResult.UseVisualStyleBackColor = true;

            // pnlResultCard
            this.pnlResultCard.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.pnlResultCard.BackColor   = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlResultCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlResultCard.Controls.Add(this.lblResultTitle);
            this.pnlResultCard.Controls.Add(this.lblFamilyName);
            this.pnlResultCard.Controls.Add(this.lblScore);
            this.pnlResultCard.Controls.Add(this.lblEncryptedCount);
            this.pnlResultCard.Controls.Add(this.lblNoteCount);
            this.pnlResultCard.Controls.Add(this.lblOtherMatch);
            this.pnlResultCard.Location    = new System.Drawing.Point(12, 12);
            this.pnlResultCard.Name        = "pnlResultCard";
            this.pnlResultCard.Size        = new System.Drawing.Size(860, 140);
            this.pnlResultCard.TabIndex    = 0;

            this.lblResultTitle.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.lblResultTitle.AutoSize  = false;
            this.lblResultTitle.Font      = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblResultTitle.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblResultTitle.Location  = new System.Drawing.Point(12, 10);
            this.lblResultTitle.Name      = "lblResultTitle";
            this.lblResultTitle.Size      = new System.Drawing.Size(836, 20);
            this.lblResultTitle.Text      = "DÒNG HỌ RANSOMWARE PHÁT HIỆN";

            this.lblFamilyName.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.lblFamilyName.AutoSize  = false;
            this.lblFamilyName.Font      = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblFamilyName.Location  = new System.Drawing.Point(10, 34);
            this.lblFamilyName.Name      = "lblFamilyName";
            this.lblFamilyName.Size      = new System.Drawing.Size(500, 44);
            this.lblFamilyName.Text      = "—";

            this.lblScore.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left);
            this.lblScore.AutoSize  = false;
            this.lblScore.Font      = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblScore.Location  = new System.Drawing.Point(10, 82);
            this.lblScore.Name      = "lblScore";
            this.lblScore.Size      = new System.Drawing.Size(400, 26);
            this.lblScore.Text      = "—";

            this.lblEncryptedCount.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Right);
            this.lblEncryptedCount.AutoSize = false;
            this.lblEncryptedCount.Location = new System.Drawing.Point(560, 40);
            this.lblEncryptedCount.Name     = "lblEncryptedCount";
            this.lblEncryptedCount.Size     = new System.Drawing.Size(290, 20);
            this.lblEncryptedCount.Text     = "0 file bị mã hóa";
            this.lblEncryptedCount.TextAlign = System.Drawing.ContentAlignment.TopRight;

            this.lblNoteCount.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Right);
            this.lblNoteCount.AutoSize = false;
            this.lblNoteCount.Location = new System.Drawing.Point(560, 66);
            this.lblNoteCount.Name     = "lblNoteCount";
            this.lblNoteCount.Size     = new System.Drawing.Size(290, 20);
            this.lblNoteCount.Text     = "0 ghi chú ransom";
            this.lblNoteCount.TextAlign = System.Drawing.ContentAlignment.TopRight;

            this.lblOtherMatch.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.lblOtherMatch.AutoSize  = false;
            this.lblOtherMatch.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.lblOtherMatch.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblOtherMatch.Location  = new System.Drawing.Point(10, 112);
            this.lblOtherMatch.Name      = "lblOtherMatch";
            this.lblOtherMatch.Size      = new System.Drawing.Size(840, 20);
            this.lblOtherMatch.Text      = "";

            // lblInfectionTime
            this.lblInfectionTime.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.lblInfectionTime.AutoSize  = false;
            this.lblInfectionTime.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.lblInfectionTime.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblInfectionTime.Location  = new System.Drawing.Point(12, 158);
            this.lblInfectionTime.Name      = "lblInfectionTime";
            this.lblInfectionTime.Size      = new System.Drawing.Size(860, 18);
            this.lblInfectionTime.Text      = "";
            this.lblInfectionTime.Visible   = false;

            // pnlSpecialHandling
            this.pnlSpecialHandling.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.pnlSpecialHandling.BackColor   = System.Drawing.Color.FromArgb(254, 242, 242);
            this.pnlSpecialHandling.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSpecialHandling.Controls.Add(this.lblSpecialIcon);
            this.pnlSpecialHandling.Controls.Add(this.lblSpecialTitle);
            this.pnlSpecialHandling.Controls.Add(this.lblSpecialBody);
            this.pnlSpecialHandling.Location    = new System.Drawing.Point(12, 180);
            this.pnlSpecialHandling.Name        = "pnlSpecialHandling";
            this.pnlSpecialHandling.Size        = new System.Drawing.Size(860, 110);
            this.pnlSpecialHandling.TabIndex    = 10;
            this.pnlSpecialHandling.Visible     = false;

            this.lblSpecialIcon.AutoSize  = false;
            this.lblSpecialIcon.Font      = new System.Drawing.Font("Segoe UI", 18F);
            this.lblSpecialIcon.Location  = new System.Drawing.Point(12, 14);
            this.lblSpecialIcon.Name      = "lblSpecialIcon";
            this.lblSpecialIcon.Size      = new System.Drawing.Size(40, 40);
            this.lblSpecialIcon.Text      = "🚨";

            this.lblSpecialTitle.AutoSize  = false;
            this.lblSpecialTitle.Font      = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSpecialTitle.ForeColor = System.Drawing.Color.FromArgb(153, 27, 27);
            this.lblSpecialTitle.Location  = new System.Drawing.Point(58, 10);
            this.lblSpecialTitle.Name      = "lblSpecialTitle";
            this.lblSpecialTitle.Size      = new System.Drawing.Size(790, 24);
            this.lblSpecialTitle.Text      = "XỬ LÝ ĐẶC BIỆT CẦN THIẾT";

            this.lblSpecialBody.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.lblSpecialBody.AutoSize  = false;
            this.lblSpecialBody.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSpecialBody.ForeColor = System.Drawing.Color.FromArgb(127, 29, 29);
            this.lblSpecialBody.Location  = new System.Drawing.Point(58, 36);
            this.lblSpecialBody.Name      = "lblSpecialBody";
            this.lblSpecialBody.Size      = new System.Drawing.Size(790, 64);
            this.lblSpecialBody.Text      = "";
            this.lblSpecialBody.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            // pnlDecryptor
            this.pnlDecryptor.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.pnlDecryptor.BackColor   = System.Drawing.Color.FromArgb(240, 253, 244);
            this.pnlDecryptor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDecryptor.Controls.Add(this.lblDecryptorHeader);
            this.pnlDecryptor.Controls.Add(this.lblToolName);
            this.pnlDecryptor.Controls.Add(this.lnkDecryptorUrl);
            this.pnlDecryptor.Controls.Add(this.btnDownloadTool);
            this.pnlDecryptor.Location    = new System.Drawing.Point(12, 300);
            this.pnlDecryptor.Name        = "pnlDecryptor";
            this.pnlDecryptor.Size        = new System.Drawing.Size(860, 100);
            this.pnlDecryptor.TabIndex    = 1;

            this.lblDecryptorHeader.AutoSize  = false;
            this.lblDecryptorHeader.Font      = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDecryptorHeader.ForeColor = System.Drawing.Color.FromArgb(21, 128, 61);
            this.lblDecryptorHeader.Location  = new System.Drawing.Point(12, 10);
            this.lblDecryptorHeader.Name      = "lblDecryptorHeader";
            this.lblDecryptorHeader.Size      = new System.Drawing.Size(300, 20);
            this.lblDecryptorHeader.Text      = "CÔNG CỤ GIẢI MÃ";

            this.lblToolName.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.lblToolName.AutoSize  = false;
            this.lblToolName.Font      = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblToolName.Location  = new System.Drawing.Point(10, 34);
            this.lblToolName.Name      = "lblToolName";
            this.lblToolName.Size      = new System.Drawing.Size(560, 24);
            this.lblToolName.Text      = "—";

            this.lnkDecryptorUrl.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.lnkDecryptorUrl.AutoSize = false;
            this.lnkDecryptorUrl.Font     = new System.Drawing.Font("Segoe UI", 9F);
            this.lnkDecryptorUrl.Location = new System.Drawing.Point(10, 62);
            this.lnkDecryptorUrl.Name     = "lnkDecryptorUrl";
            this.lnkDecryptorUrl.Size     = new System.Drawing.Size(560, 20);
            this.lnkDecryptorUrl.Text     = "";
            this.lnkDecryptorUrl.Visible  = false;
            this.lnkDecryptorUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDecryptorUrl_LinkClicked);

            this.btnDownloadTool.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Right);
            this.btnDownloadTool.BackColor = System.Drawing.Color.FromArgb(21, 128, 61);
            this.btnDownloadTool.Enabled   = false;
            this.btnDownloadTool.FlatAppearance.BorderSize = 0;
            this.btnDownloadTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadTool.Font      = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDownloadTool.ForeColor = System.Drawing.Color.White;
            this.btnDownloadTool.Location  = new System.Drawing.Point(648, 30);
            this.btnDownloadTool.Name      = "btnDownloadTool";
            this.btnDownloadTool.Size      = new System.Drawing.Size(200, 36);
            this.btnDownloadTool.TabIndex  = 0;
            this.btnDownloadTool.Text      = "⬇  Tải công cụ";
            this.btnDownloadTool.Click    += new System.EventHandler(this.btnDownloadTool_Click);

            // pnlCondition
            this.pnlCondition.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.pnlCondition.BackColor   = System.Drawing.Color.FromArgb(255, 251, 235);
            this.pnlCondition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCondition.Controls.Add(this.lblCondition);
            this.pnlCondition.Location    = new System.Drawing.Point(12, 410);
            this.pnlCondition.Name        = "pnlCondition";
            this.pnlCondition.Size        = new System.Drawing.Size(860, 60);
            this.pnlCondition.TabIndex    = 2;
            this.pnlCondition.Visible     = false;

            this.lblCondition.AutoSize  = false;
            this.lblCondition.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.lblCondition.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCondition.ForeColor = System.Drawing.Color.FromArgb(120, 53, 15);
            this.lblCondition.Name      = "lblCondition";
            this.lblCondition.Padding   = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.lblCondition.Text      = "";
            this.lblCondition.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            // lblFamilyNote
            this.lblFamilyNote.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.lblFamilyNote.AutoSize  = false;
            this.lblFamilyNote.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFamilyNote.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.lblFamilyNote.Location  = new System.Drawing.Point(12, 480);
            this.lblFamilyNote.Name      = "lblFamilyNote";
            this.lblFamilyNote.Size      = new System.Drawing.Size(860, 40);
            this.lblFamilyNote.Text      = "";
            this.lblFamilyNote.Visible   = false;

            // pnlSubmit
            this.pnlSubmit.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.pnlSubmit.BackColor   = System.Drawing.Color.FromArgb(239, 246, 255);
            this.pnlSubmit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSubmit.Controls.Add(this.lblSubmitHeader);
            this.pnlSubmit.Controls.Add(this.btnSubmitOnline);
            this.pnlSubmit.Controls.Add(this.btnSaveReport);
            this.pnlSubmit.Controls.Add(this.lblSubmitStatus);
            this.pnlSubmit.Location    = new System.Drawing.Point(12, 530);
            this.pnlSubmit.Name        = "pnlSubmit";
            this.pnlSubmit.Size        = new System.Drawing.Size(860, 90);
            this.pnlSubmit.TabIndex    = 3;

            this.lblSubmitHeader.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.lblSubmitHeader.AutoSize  = false;
            this.lblSubmitHeader.Font      = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSubmitHeader.ForeColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.lblSubmitHeader.Location  = new System.Drawing.Point(12, 10);
            this.lblSubmitHeader.Name      = "lblSubmitHeader";
            this.lblSubmitHeader.Size      = new System.Drawing.Size(836, 20);
            this.lblSubmitHeader.Text      = "GỬI MẪU ĐỂ XÁC ĐỊNH CHÍNH XÁC HƠN (không bắt buộc)";

            this.btnSubmitOnline.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnSubmitOnline.FlatAppearance.BorderSize = 0;
            this.btnSubmitOnline.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmitOnline.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSubmitOnline.ForeColor = System.Drawing.Color.White;
            this.btnSubmitOnline.Location  = new System.Drawing.Point(12, 38);
            this.btnSubmitOnline.Name      = "btnSubmitOnline";
            this.btnSubmitOnline.Size      = new System.Drawing.Size(220, 36);
            this.btnSubmitOnline.TabIndex  = 0;
            this.btnSubmitOnline.Text      = "🌐 Gửi lên ID-Ransomware";
            this.btnSubmitOnline.Click    += new System.EventHandler(this.btnSubmitOnline_Click);

            this.btnSaveReport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSaveReport.Location  = new System.Drawing.Point(248, 38);
            this.btnSaveReport.Name      = "btnSaveReport";
            this.btnSaveReport.Size      = new System.Drawing.Size(180, 36);
            this.btnSaveReport.TabIndex  = 1;
            this.btnSaveReport.Text      = "💾 Lưu báo cáo (.txt)";
            this.btnSaveReport.Click    += new System.EventHandler(this.btnSaveReport_Click);

            this.lblSubmitStatus.AutoSize = true;
            this.lblSubmitStatus.Location = new System.Drawing.Point(444, 46);
            this.lblSubmitStatus.Name     = "lblSubmitStatus";
            this.lblSubmitStatus.Text     = "";

            // pnlEmailFallback
            this.pnlEmailFallback.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.pnlEmailFallback.BackColor   = System.Drawing.Color.FromArgb(255, 247, 237);
            this.pnlEmailFallback.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEmailFallback.Controls.Add(this.lblEmailHeader);
            this.pnlEmailFallback.Controls.Add(this.lblEmailTo);
            this.pnlEmailFallback.Controls.Add(this.txtEmailTo);
            this.pnlEmailFallback.Controls.Add(this.lblEmailSubject);
            this.pnlEmailFallback.Controls.Add(this.txtEmailSubject);
            this.pnlEmailFallback.Controls.Add(this.lblEmailBody);
            this.pnlEmailFallback.Controls.Add(this.txtEmailBody);
            this.pnlEmailFallback.Controls.Add(this.pnlEmailButtons);
            this.pnlEmailFallback.Location    = new System.Drawing.Point(12, 630);
            this.pnlEmailFallback.Name        = "pnlEmailFallback";
            this.pnlEmailFallback.Size        = new System.Drawing.Size(860, 280);
            this.pnlEmailFallback.TabIndex    = 4;
            this.pnlEmailFallback.Visible     = false;

            this.lblEmailHeader.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.lblEmailHeader.AutoSize  = false;
            this.lblEmailHeader.Font      = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEmailHeader.ForeColor = System.Drawing.Color.FromArgb(180, 83, 9);
            this.lblEmailHeader.Location  = new System.Drawing.Point(12, 10);
            this.lblEmailHeader.Name      = "lblEmailHeader";
            this.lblEmailHeader.Size      = new System.Drawing.Size(836, 22);
            this.lblEmailHeader.Text      = "📧  Gửi mẫu qua email (khi không có internet hoặc upload thất bại)";

            this.lblEmailTo.AutoSize = true;
            this.lblEmailTo.Location = new System.Drawing.Point(12, 42);
            this.lblEmailTo.Name     = "lblEmailTo";
            this.lblEmailTo.Text     = "Gửi đến:";

            this.txtEmailTo.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.txtEmailTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmailTo.Location    = new System.Drawing.Point(90, 39);
            this.txtEmailTo.Name        = "txtEmailTo";
            this.txtEmailTo.ReadOnly    = true;
            this.txtEmailTo.Size        = new System.Drawing.Size(758, 24);
            this.txtEmailTo.TabIndex    = 0;

            this.lblEmailSubject.AutoSize = true;
            this.lblEmailSubject.Location = new System.Drawing.Point(12, 72);
            this.lblEmailSubject.Name     = "lblEmailSubject";
            this.lblEmailSubject.Text     = "Tiêu đề:";

            this.txtEmailSubject.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.txtEmailSubject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmailSubject.Location    = new System.Drawing.Point(90, 69);
            this.txtEmailSubject.Name        = "txtEmailSubject";
            this.txtEmailSubject.ReadOnly    = true;
            this.txtEmailSubject.Size        = new System.Drawing.Size(758, 24);
            this.txtEmailSubject.TabIndex    = 1;

            this.lblEmailBody.AutoSize = true;
            this.lblEmailBody.Location = new System.Drawing.Point(12, 102);
            this.lblEmailBody.Name     = "lblEmailBody";
            this.lblEmailBody.Text     = "Nội dung:";

            this.txtEmailBody.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.txtEmailBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmailBody.Location    = new System.Drawing.Point(12, 124);
            this.txtEmailBody.Name        = "txtEmailBody";
            this.txtEmailBody.ReadOnly    = true;
            this.txtEmailBody.ScrollBars  = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtEmailBody.Size        = new System.Drawing.Size(836, 100);
            this.txtEmailBody.TabIndex    = 2;

            this.pnlEmailButtons.Anchor = (System.Windows.Forms.AnchorStyles.Bottom
                | System.Windows.Forms.AnchorStyles.Left);
            this.pnlEmailButtons.Controls.Add(this.btnCopyEmail);
            this.pnlEmailButtons.Controls.Add(this.btnOpenMail);
            this.pnlEmailButtons.Controls.Add(this.btnOpenBrowser);
            this.pnlEmailButtons.Location = new System.Drawing.Point(12, 232);
            this.pnlEmailButtons.Name     = "pnlEmailButtons";
            this.pnlEmailButtons.Size     = new System.Drawing.Size(540, 38);
            this.pnlEmailButtons.TabIndex = 3;

            this.btnCopyEmail.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCopyEmail.Location  = new System.Drawing.Point(0, 0);
            this.btnCopyEmail.Name      = "btnCopyEmail";
            this.btnCopyEmail.Size      = new System.Drawing.Size(160, 36);
            this.btnCopyEmail.TabIndex  = 0;
            this.btnCopyEmail.Text      = "📋 Copy nội dung";
            this.btnCopyEmail.Click    += new System.EventHandler(this.btnCopyEmail_Click);

            this.btnOpenMail.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOpenMail.Location  = new System.Drawing.Point(170, 0);
            this.btnOpenMail.Name      = "btnOpenMail";
            this.btnOpenMail.Size      = new System.Drawing.Size(160, 36);
            this.btnOpenMail.TabIndex  = 1;
            this.btnOpenMail.Text      = "✉ Mở ứng dụng mail";
            this.btnOpenMail.Click    += new System.EventHandler(this.btnOpenMail_Click);

            this.btnOpenBrowser.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOpenBrowser.Location  = new System.Drawing.Point(340, 0);
            this.btnOpenBrowser.Name      = "btnOpenBrowser";
            this.btnOpenBrowser.Size      = new System.Drawing.Size(180, 36);
            this.btnOpenBrowser.TabIndex  = 2;
            this.btnOpenBrowser.Text      = "🌐 Mở ID-Ransomware";
            this.btnOpenBrowser.Click    += new System.EventHandler(this.btnOpenBrowser_Click);

            // pnlNextSteps
            this.pnlNextSteps.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.pnlNextSteps.BackColor   = System.Drawing.Color.FromArgb(245, 245, 245);
            this.pnlNextSteps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlNextSteps.Controls.Add(this.lblNextHeader);
            this.pnlNextSteps.Controls.Add(this.lblNextSteps);
            this.pnlNextSteps.Location    = new System.Drawing.Point(12, 920);
            this.pnlNextSteps.Name        = "pnlNextSteps";
            this.pnlNextSteps.Size        = new System.Drawing.Size(860, 180);
            this.pnlNextSteps.TabIndex    = 5;

            this.lblNextHeader.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.lblNextHeader.AutoSize  = false;
            this.lblNextHeader.Font      = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNextHeader.Location  = new System.Drawing.Point(12, 10);
            this.lblNextHeader.Name      = "lblNextHeader";
            this.lblNextHeader.Size      = new System.Drawing.Size(836, 24);
            this.lblNextHeader.Text      = "📋  Bước tiếp theo";

            this.lblNextSteps.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.lblNextSteps.AutoSize  = false;
            this.lblNextSteps.Font      = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblNextSteps.Location  = new System.Drawing.Point(12, 40);
            this.lblNextSteps.Name      = "lblNextSteps";
            this.lblNextSteps.Size      = new System.Drawing.Size(836, 130);
            this.lblNextSteps.Text      =
                "1.  Xóa ransomware: Tải Kaspersky Virus Removal Tool (KVRT) từ máy sạch → chạy từ USB.\r\n\r\n" +
                "2.  Giải mã file: Dùng công cụ giải mã bên trên (nếu có). Chạy thử trên 1-2 file " +
                "trước khi chạy toàn bộ.\r\n\r\n" +
                "3.  Backup: Trước khi giải mã, copy toàn bộ file mã hóa sang ổ cứng ngoài (giữ bản gốc " +
                "phòng khi công cụ lỗi).\r\n\r\n" +
                "4.  Nếu chưa có công cụ giải mã: Theo dõi nomoreransom.org — công cụ mới được thêm " +
                "thường xuyên khi cảnh sát thu giữ khóa.";
            this.lblNextSteps.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            // btnRunKvrt — nút chạy KVRT sau khi tải xong, ẩn mặc định
            this.btnRunKvrt.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.btnRunKvrt.BackColor = System.Drawing.Color.FromArgb(124, 45, 18);
            this.btnRunKvrt.FlatAppearance.BorderSize = 0;
            this.btnRunKvrt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunKvrt.Font      = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRunKvrt.ForeColor = System.Drawing.Color.White;
            this.btnRunKvrt.Location  = new System.Drawing.Point(12, 1108);
            this.btnRunKvrt.Name      = "btnRunKvrt";
            this.btnRunKvrt.Size      = new System.Drawing.Size(860, 42);
            this.btnRunKvrt.TabIndex  = 6;
            this.btnRunKvrt.Text      = "🛡  Chạy Kaspersky KVRT (đã tải về %TEMP%)";
            this.btnRunKvrt.Visible   = false;
            this.btnRunKvrt.Click    += new System.EventHandler(this.btnRunKvrt_Click);

            // pnlCandidates — top 2-3 dòng họ gần nhất (multi-family detection)
            this.pnlCandidates.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.pnlCandidates.BackColor   = System.Drawing.Color.FromArgb(239, 246, 255);
            this.pnlCandidates.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCandidates.Controls.Add(this.lblCandidatesHdr);
            this.pnlCandidates.Controls.Add(this.lstCandidates);
            this.pnlCandidates.Location    = new System.Drawing.Point(12, 1200);
            this.pnlCandidates.Name        = "pnlCandidates";
            this.pnlCandidates.Size        = new System.Drawing.Size(860, 120);
            this.pnlCandidates.TabIndex    = 8;
            this.pnlCandidates.Visible     = false;

            this.lblCandidatesHdr.AutoSize  = false;
            this.lblCandidatesHdr.Font      = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCandidatesHdr.ForeColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.lblCandidatesHdr.Location  = new System.Drawing.Point(12, 8);
            this.lblCandidatesHdr.Name      = "lblCandidatesHdr";
            this.lblCandidatesHdr.Size      = new System.Drawing.Size(836, 20);
            this.lblCandidatesHdr.Text      = "CÁC DÒNG HỌ KHÁC CÓ THỂ TRÙNG KHỚP";

            this.lstCandidates.Anchor = (System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right);
            this.lstCandidates.BorderStyle  = System.Windows.Forms.BorderStyle.None;
            this.lstCandidates.Font         = new System.Drawing.Font("Segoe UI", 9F);
            this.lstCandidates.Location     = new System.Drawing.Point(12, 32);
            this.lstCandidates.Name         = "lstCandidates";
            this.lstCandidates.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lstCandidates.Size         = new System.Drawing.Size(836, 80);
            this.lstCandidates.TabIndex     = 0;

            // ════════════════════════════════════════════════════════════════════
            // frmMain
            // ════════════════════════════════════════════════════════════════════
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(920, 661);
            this.MinimumSize         = new System.Drawing.Size(700, 540);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox     = false;
            this.MinimizeBox     = true;
            this.Name            = "frmMain";
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text            = "RansomRescue — Công cụ phục hồi Ransomware";
            this.Load           += new System.EventHandler(this.frmMain_Load);
            this.FormClosing    += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);

            this.pnlCandidates.ResumeLayout(false);
            this.pnlCandidates.PerformLayout();
            this.pnlDrives.ResumeLayout(false);
            this.pnlDrives.PerformLayout();
            this.pnlSpecialHandling.ResumeLayout(false);
            this.pnlSpecialHandling.PerformLayout();
            this.pnlWarningFooter.ResumeLayout(false);
            this.pnlWarningFooter.PerformLayout();
            this.tabResult.ResumeLayout(false);
            this.tabResult.PerformLayout();
            this.tabScan.ResumeLayout(false);
            this.tabScan.PerformLayout();
            this.tabWarning.ResumeLayout(false);
            this.tabWarning.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        // ── Field declarations ───────────────────────────────────────────────────
        private System.Windows.Forms.TabControl            tabMain;
        private System.Windows.Forms.TabPage               tabWarning;
        private System.Windows.Forms.TabPage               tabScan;
        private System.Windows.Forms.TabPage               tabResult;
        private System.Windows.Forms.StatusStrip           statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel  lblDbStatus;
        private System.Windows.Forms.Panel                 pnlUptimeWarning;
        private System.Windows.Forms.Label                 lblUptimeWarning;
        private System.Windows.Forms.Panel                 pnlWarningBody;
        private System.Windows.Forms.Panel                 pnlWarningFooter;
        private System.Windows.Forms.Label                 lblWarningTitle;
        private System.Windows.Forms.Label                 lblWarningText;
        private System.Windows.Forms.CheckBox              chkDownloadKvrt;
        private System.Windows.Forms.CheckBox              chkConfirm;
        private System.Windows.Forms.Button                btnStartScan;
        private System.Windows.Forms.Panel                 pnlScanTop;
        private System.Windows.Forms.CheckBox              chkDeepScan;
        private System.Windows.Forms.Button                btnScan;
        private System.Windows.Forms.ProgressBar           progressBar;
        private System.Windows.Forms.Label                 lblCurrentPath;
        private System.Windows.Forms.Label                 lblScanStatus;
        private System.Windows.Forms.Panel                 pnlScanStats;
        private System.Windows.Forms.Label                 lblScanned;
        private System.Windows.Forms.Label                 lblSuspect;
        private System.Windows.Forms.Label                 lblNotes;
        private System.Windows.Forms.Label                 lblLogHeader;
        private System.Windows.Forms.RichTextBox           rtbLog;
        private System.Windows.Forms.Panel                 pnlResultCard;
        private System.Windows.Forms.Label                 lblResultTitle;
        private System.Windows.Forms.Label                 lblFamilyName;
        private System.Windows.Forms.Label                 lblScore;
        private System.Windows.Forms.Label                 lblEncryptedCount;
        private System.Windows.Forms.Label                 lblNoteCount;
        private System.Windows.Forms.Label                 lblOtherMatch;
        private System.Windows.Forms.Panel                 pnlDecryptor;
        private System.Windows.Forms.Label                 lblDecryptorHeader;
        private System.Windows.Forms.Label                 lblToolName;
        private System.Windows.Forms.LinkLabel             lnkDecryptorUrl;
        private System.Windows.Forms.Button                btnDownloadTool;
        private System.Windows.Forms.Panel                 pnlCondition;
        private System.Windows.Forms.Label                 lblCondition;
        private System.Windows.Forms.Label                 lblFamilyNote;
        private System.Windows.Forms.Panel                 pnlSubmit;
        private System.Windows.Forms.Label                 lblSubmitHeader;
        private System.Windows.Forms.Button                btnSubmitOnline;
        private System.Windows.Forms.Label                 lblSubmitStatus;
        private System.Windows.Forms.Panel                 pnlEmailFallback;
        private System.Windows.Forms.Label                 lblEmailHeader;
        private System.Windows.Forms.Label                 lblEmailTo;
        private System.Windows.Forms.TextBox               txtEmailTo;
        private System.Windows.Forms.Label                 lblEmailSubject;
        private System.Windows.Forms.TextBox               txtEmailSubject;
        private System.Windows.Forms.Label                 lblEmailBody;
        private System.Windows.Forms.RichTextBox           txtEmailBody;
        private System.Windows.Forms.Panel                 pnlEmailButtons;
        private System.Windows.Forms.Button                btnCopyEmail;
        private System.Windows.Forms.Button                btnOpenMail;
        private System.Windows.Forms.Button                btnOpenBrowser;
        private System.Windows.Forms.Panel                 pnlNextSteps;
        private System.Windows.Forms.Label                 lblNextHeader;
        private System.Windows.Forms.Label                 lblNextSteps;
        private System.Windows.Forms.Button                btnRunKvrt;
        private System.Windows.Forms.Button                btnSaveReport;
        private System.Windows.Forms.ToolStripProgressBar  pbKvrt;
        private System.Windows.Forms.ToolStripButton       btnDarkMode;
        private System.Windows.Forms.ToolStripButton       btnLangToggle;
        private System.Windows.Forms.ToolStripButton       btnDonate;
        private System.Windows.Forms.ToolTip               toolTip1;
        private System.Windows.Forms.Panel                 pnlCandidates;
        private System.Windows.Forms.Label                 lblCandidatesHdr;
        private System.Windows.Forms.ListBox               lstCandidates;
        private System.Windows.Forms.Panel                 pnlDrives;
        private System.Windows.Forms.Label                 lblDrivesHeader;
        private System.Windows.Forms.FlowLayoutPanel       flowDrives;
        private System.Windows.Forms.Panel                 pnlSpecialHandling;
        private System.Windows.Forms.Label                 lblSpecialIcon;
        private System.Windows.Forms.Label                 lblSpecialTitle;
        private System.Windows.Forms.Label                 lblSpecialBody;
        private System.Windows.Forms.Label                 lblInfectionTime;
    }
}
