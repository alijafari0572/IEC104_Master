namespace IEC104.Master.WinForms.Form
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            // ===== تعریف کنترل‌ها =====
            this.panlLoading = new Panel();
            this.lbLoading = new Label();
            this.progressBarLoading = new ProgressBar();
            this.flowLayoutPanelTop = new FlowLayoutPanel(); // ← جدید
            this.btnConnect = new Button();
            this.btnDisconnect = new Button();
            this.btnGI = new Button();
            this.lblStatus = new Label();
            this.splitContainerMain = new SplitContainer();
            this.dgvAsduData = new DataGridView();
            this.lstLog = new ListBox();

            this.btnManageRequests = new Button();
            this.btnSendRequest = new Button();

            // btnManageRequests
            this.btnManageRequests.BackColor = Color.FromArgb(156, 39, 176);
            this.btnManageRequests.FlatAppearance.BorderSize = 0;
            this.btnManageRequests.FlatStyle = FlatStyle.Flat;
            this.btnManageRequests.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnManageRequests.ForeColor = Color.White;
            this.btnManageRequests.Location = new Point(650, 90);
            this.btnManageRequests.Name = "btnManageRequests";
            this.btnManageRequests.Size = new Size(130, 47);
            this.btnManageRequests.TabIndex = 10;
            this.btnManageRequests.Text = "⏱️ زمان‌بندی";
            this.btnManageRequests.UseVisualStyleBackColor = false;
            this.btnManageRequests.Click += this.btnManageRequests_Click;

            // btnSendRequest
            this.btnSendRequest.BackColor = Color.FromArgb(255, 152, 0);
            this.btnSendRequest.FlatAppearance.BorderSize = 0;
            this.btnSendRequest.FlatStyle = FlatStyle.Flat;
            this.btnSendRequest.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSendRequest.ForeColor = Color.White;
            this.btnSendRequest.Location = new Point(790, 90);
            this.btnSendRequest.Name = "btnSendRequest";
            this.btnSendRequest.Size = new Size(130, 47);
            this.btnSendRequest.TabIndex = 11;
            this.btnSendRequest.Text = "📤 ارسال دستی";
            this.btnSendRequest.UseVisualStyleBackColor = false;
            this.btnSendRequest.Click += this.btnSendRequest_Click;

            // ===== panlLoading =====
            this.panlLoading.Anchor = AnchorStyles.None;
            this.panlLoading.BackColor = Color.FromArgb(240, 240, 240);
            this.panlLoading.BorderStyle = BorderStyle.FixedSingle;
            this.panlLoading.Controls.Add(this.lbLoading);
            this.panlLoading.Controls.Add(this.progressBarLoading);
            this.panlLoading.Location = new Point(437, 172);
            this.panlLoading.Name = "panlLoading";
            this.panlLoading.Size = new Size(280, 110);
            this.panlLoading.TabIndex = 5;
            this.panlLoading.Visible = false;

            // ===== lbLoading =====
            this.lbLoading.AutoSize = true;
            this.lbLoading.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lbLoading.ForeColor = Color.FromArgb(0, 120, 215);
            this.lbLoading.Location = new Point(60, 20);
            this.lbLoading.Name = "lbLoading";
            this.lbLoading.Size = new Size(165, 28);
            this.lbLoading.TabIndex = 6;
            this.lbLoading.Text = "⏳ درحال اتصال...";

            // ===== progressBarLoading =====
            this.progressBarLoading.ForeColor = Color.FromArgb(0, 120, 215);
            this.progressBarLoading.Location = new Point(35, 60);
            this.progressBarLoading.MarqueeAnimationSpeed = 30;
            this.progressBarLoading.Name = "progressBarLoading";
            this.progressBarLoading.Size = new Size(210, 30);
            this.progressBarLoading.Style = ProgressBarStyle.Marquee;
            this.progressBarLoading.TabIndex = 7;

            // ===== flowLayoutPanelTop (دکمه‌ها و وضعیت) =====
            this.flowLayoutPanelTop.Dock = DockStyle.Top;
            this.flowLayoutPanelTop.Height = 140;
            this.flowLayoutPanelTop.BackColor = Color.White;
            this.flowLayoutPanelTop.FlowDirection = FlowDirection.LeftToRight;
            this.flowLayoutPanelTop.Padding = new Padding(20, 20, 20, 20);
            this.flowLayoutPanelTop.WrapContents = true; // اگر دکمه‌ها جا نشدند، به خط بعد بروند
            this.flowLayoutPanelTop.Controls.Add(this.lblStatus);
            this.flowLayoutPanelTop.Controls.Add(this.btnConnect);
            this.flowLayoutPanelTop.Controls.Add(this.btnDisconnect);
            this.flowLayoutPanelTop.Controls.Add(this.btnGI);

            // اضافه کردن به flowLayoutPanelTop
            this.flowLayoutPanelTop.Controls.Add(this.btnManageRequests);
            this.flowLayoutPanelTop.Controls.Add(this.btnSendRequest);

            // ===== lblStatus =====
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblStatus.ForeColor = Color.FromArgb(200, 50, 50);
            this.lblStatus.Location = new Point(20, 20);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(176, 28);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "🔴 Disconnected";
            this.lblStatus.TextAlign = ContentAlignment.MiddleLeft;

            // ===== btnConnect =====
            this.btnConnect.BackColor = Color.FromArgb(0, 120, 215);
            this.btnConnect.FlatAppearance.BorderSize = 0;
            this.btnConnect.FlatStyle = FlatStyle.Flat;
            this.btnConnect.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnConnect.ForeColor = Color.White;
            this.btnConnect.Location = new Point(196, 20);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new Size(130, 47);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "🔗 Connect";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += this.btnConnect_Click_1;

            // ===== btnDisconnect =====
            this.btnDisconnect.BackColor = Color.FromArgb(200, 50, 50);
            this.btnDisconnect.FlatAppearance.BorderSize = 0;
            this.btnDisconnect.FlatStyle = FlatStyle.Flat;
            this.btnDisconnect.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnDisconnect.ForeColor = Color.White;
            this.btnDisconnect.Location = new Point(326, 20);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new Size(140, 47);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "⛔ Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = false;
            this.btnDisconnect.Click += this.btnDisconnect_Click_1;

            // ===== btnGI =====
            this.btnGI.BackColor = Color.FromArgb(50, 180, 100);
            this.btnGI.FlatAppearance.BorderSize = 0;
            this.btnGI.FlatStyle = FlatStyle.Flat;
            this.btnGI.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnGI.ForeColor = Color.White;
            this.btnGI.Location = new Point(454, 20);
            this.btnGI.Name = "btnGI";
            this.btnGI.Size = new Size(115, 47);
            this.btnGI.TabIndex = 2;
            this.btnGI.Text = "📡 GI";
            this.btnGI.UseVisualStyleBackColor = false;
            this.btnGI.Click += this.btnGI_Click_1;

            // ===== splitContainerMain =====
            this.splitContainerMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.splitContainerMain.Location = new Point(23, 160);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = Orientation.Horizontal;
            this.splitContainerMain.Panel1.BackColor = Color.White;
            this.splitContainerMain.Panel2.BackColor = Color.White;
            this.splitContainerMain.Size = new Size(1168, 574);
            this.splitContainerMain.SplitterDistance = 280;
            this.splitContainerMain.SplitterWidth = 6;
            this.splitContainerMain.TabIndex = 9;

            // ===== dgvAsduData =====
            this.dgvAsduData.AllowUserToAddRows = false;
            this.dgvAsduData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAsduData.BackgroundColor = Color.White;
            this.dgvAsduData.BorderStyle = BorderStyle.None;
            this.dgvAsduData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAsduData.Dock = DockStyle.Fill;
            this.dgvAsduData.GridColor = Color.FromArgb(230, 230, 230);
            this.dgvAsduData.Name = "dgvAsduData";
            this.dgvAsduData.ReadOnly = true;
            this.dgvAsduData.RowHeadersVisible = false;
            this.dgvAsduData.RowHeadersWidth = 51;
            this.dgvAsduData.RowTemplate.Height = 28;
            this.dgvAsduData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvAsduData.TabIndex = 8;
            this.dgvAsduData.CellContentClick += this.dataGridView1_CellContentClick;
            this.splitContainerMain.Panel1.Controls.Add(this.dgvAsduData);

            // ===== lstLog =====
            this.lstLog.BackColor = Color.FromArgb(245, 245, 245);
            this.lstLog.Dock = DockStyle.Fill;
            this.lstLog.Font = new Font("Consolas", 9F);
            this.lstLog.ForeColor = Color.FromArgb(50, 50, 50);
            this.lstLog.FormattingEnabled = true;
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new Size(1168, 288);
            this.lstLog.TabIndex = 3;
            this.lstLog.SelectedIndexChanged += this.listBox1_SelectedIndexChanged;
            this.splitContainerMain.Panel2.Controls.Add(this.lstLog);

            // ===== MainForm =====
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1212, 746);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.panlLoading);
            this.Controls.Add(this.flowLayoutPanelTop);
            this.Font = new Font("Segoe UI", 9F);
            this.MinimumSize = new Size(900, 600);
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "⚡ IEC 104 Master - SCADA Client";
            this.Load += this.MainForm_Load;
            this.panlLoading.ResumeLayout(false);
            this.panlLoading.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitContainerMain).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.dgvAsduData).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        // ===== فیلدهای کلاس =====
        private Button btnConnect;
        private Button btnDisconnect;
        private Button btnGI;
        private ListBox lstLog;
        private Label lblStatus;
        private Panel panlLoading;
        private Label lbLoading;
        private ProgressBar progressBarLoading;
        private DataGridView dgvAsduData;
        private SplitContainer splitContainerMain;
        private FlowLayoutPanel flowLayoutPanelTop; 

        private Button btnManageRequests;
        private Button btnSendRequest;

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
    }
}