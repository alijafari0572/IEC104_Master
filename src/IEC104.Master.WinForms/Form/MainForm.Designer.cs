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
            Panel panlLoading;
            lbLoading = new Label();
            progressBarLoading = new ProgressBar();
            btnConnect = new Button();
            btnDisconnect = new Button();
            btnGI = new Button();
            lblStatus = new Label();
            splitContainerMain = new SplitContainer();
            dgvAsduData = new DataGridView();
            lstLog = new ListBox();
            panlLoading = new Panel();
            panlLoading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAsduData).BeginInit();
            SuspendLayout();
            // 
            // panlLoading
            // 
            panlLoading.Anchor = AnchorStyles.None;
            panlLoading.BackColor = Color.FromArgb(240, 240, 240);
            panlLoading.BorderStyle = BorderStyle.FixedSingle;
            panlLoading.Controls.Add(lbLoading);
            panlLoading.Controls.Add(progressBarLoading);
            panlLoading.Location = new Point(437, 172);
            panlLoading.Name = "panlLoading";
            panlLoading.Size = new Size(280, 110);
            panlLoading.TabIndex = 5;
            panlLoading.Visible = false;
            // 
            // lbLoading
            // 
            lbLoading.AutoSize = true;
            lbLoading.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lbLoading.ForeColor = Color.FromArgb(0, 120, 215);
            lbLoading.Location = new Point(60, 20);
            lbLoading.Name = "lbLoading";
            lbLoading.Size = new Size(165, 28);
            lbLoading.TabIndex = 6;
            lbLoading.Text = "⏳ درحال اتصال...";
            // 
            // progressBarLoading
            // 
            progressBarLoading.ForeColor = Color.FromArgb(0, 120, 215);
            progressBarLoading.Location = new Point(35, 60);
            progressBarLoading.MarqueeAnimationSpeed = 30;
            progressBarLoading.Name = "progressBarLoading";
            progressBarLoading.Size = new Size(210, 30);
            progressBarLoading.Style = ProgressBarStyle.Marquee;
            progressBarLoading.TabIndex = 7;
            // 
            // btnConnect
            // 
            btnConnect.BackColor = Color.FromArgb(0, 120, 215);
            btnConnect.FlatAppearance.BorderSize = 0;
            btnConnect.FlatStyle = FlatStyle.Flat;
            btnConnect.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnConnect.ForeColor = Color.White;
            btnConnect.Location = new Point(318, 90);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(130, 47);
            btnConnect.TabIndex = 0;
            btnConnect.Text = "🔗 Connect";
            btnConnect.UseVisualStyleBackColor = false;
            btnConnect.Click += btnConnect_Click_1;
            // 
            // btnDisconnect
            // 
            btnDisconnect.BackColor = Color.FromArgb(200, 50, 50);
            btnDisconnect.FlatAppearance.BorderSize = 0;
            btnDisconnect.FlatStyle = FlatStyle.Flat;
            btnDisconnect.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDisconnect.ForeColor = Color.Black;
            btnDisconnect.Location = new Point(516, 90);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(128, 47);
            btnDisconnect.TabIndex = 1;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = false;
            btnDisconnect.Click += btnDisconnect_Click_1;
            // 
            // btnGI
            // 
            btnGI.BackColor = Color.FromArgb(50, 180, 100);
            btnGI.FlatAppearance.BorderSize = 0;
            btnGI.FlatStyle = FlatStyle.Flat;
            btnGI.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGI.ForeColor = Color.White;
            btnGI.Location = new Point(745, 90);
            btnGI.Name = "btnGI";
            btnGI.Size = new Size(115, 47);
            btnGI.TabIndex = 2;
            btnGI.Text = "📡 GI";
            btnGI.UseVisualStyleBackColor = false;
            btnGI.Click += btnGI_Click_1;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Top;
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblStatus.Location = new Point(498, 28);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(146, 23);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "🔴 Disconnected";
            // 
            // splitContainerMain
            // 
            splitContainerMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainerMain.Location = new Point(23, 160);
            splitContainerMain.Name = "splitContainerMain";
            splitContainerMain.Orientation = Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.BackColor = Color.White;
            splitContainerMain.Panel1.Controls.Add(dgvAsduData);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.BackColor = Color.White;
            splitContainerMain.Panel2.Controls.Add(lstLog);
            splitContainerMain.Size = new Size(1168, 574);
            splitContainerMain.SplitterDistance = 280;
            splitContainerMain.SplitterWidth = 6;
            splitContainerMain.TabIndex = 9;
            // 
            // dgvAsduData
            // 
            dgvAsduData.AllowUserToAddRows = false;
            dgvAsduData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAsduData.BackgroundColor = Color.White;
            dgvAsduData.BorderStyle = BorderStyle.None;
            dgvAsduData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAsduData.Dock = DockStyle.Fill;
            dgvAsduData.GridColor = Color.FromArgb(230, 230, 230);
            dgvAsduData.Location = new Point(0, 0);
            dgvAsduData.Name = "dgvAsduData";
            dgvAsduData.ReadOnly = true;
            dgvAsduData.RowHeadersVisible = false;
            dgvAsduData.RowHeadersWidth = 51;
            dgvAsduData.RowTemplate.Height = 28;
            dgvAsduData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAsduData.Size = new Size(1168, 280);
            dgvAsduData.TabIndex = 8;
            dgvAsduData.CellContentClick += dataGridView1_CellContentClick;
            // 
            // lstLog
            // 
            lstLog.BackColor = Color.FromArgb(245, 245, 245);
            lstLog.Dock = DockStyle.Fill;
            lstLog.Font = new Font("Consolas", 9F);
            lstLog.ForeColor = Color.FromArgb(50, 50, 50);
            lstLog.FormattingEnabled = true;
            lstLog.Location = new Point(0, 0);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(1168, 288);
            lstLog.TabIndex = 3;
            lstLog.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1212, 746);
            Controls.Add(splitContainerMain);
            Controls.Add(panlLoading);
            Controls.Add(lblStatus);
            Controls.Add(btnGI);
            Controls.Add(btnDisconnect);
            Controls.Add(btnConnect);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(900, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "⚡ IEC 104 Master - SCADA Client";
            Load += MainForm_Load;
            panlLoading.ResumeLayout(false);
            panlLoading.PerformLayout();
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAsduData).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
        private SplitContainer splitContainerMain; // ← اضافه شده

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
    }
}