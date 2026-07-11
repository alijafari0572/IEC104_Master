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
            lstLog = new ListBox();
            lblStatus = new Label();
            lblQoi = new Label();
            nudQoi = new NumericUpDown();
            GroupI = new Button();
            panlLoading = new Panel();
            panlLoading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudQoi).BeginInit();
            SuspendLayout();
            // 
            // panlLoading
            // 
            panlLoading.BackColor = Color.LightGray;
            panlLoading.BorderStyle = BorderStyle.FixedSingle;
            panlLoading.Controls.Add(lbLoading);
            panlLoading.Controls.Add(progressBarLoading);
            panlLoading.Location = new Point(275, 150);
            panlLoading.Name = "panlLoading";
            panlLoading.Size = new Size(250, 100);
            panlLoading.TabIndex = 5;
            panlLoading.Visible = false;
            // 
            // lbLoading
            // 
            lbLoading.AutoSize = true;
            lbLoading.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lbLoading.ForeColor = Color.DarkBlue;
            lbLoading.Location = new Point(75, 15);
            lbLoading.Name = "lbLoading";
            lbLoading.Size = new Size(142, 23);
            lbLoading.TabIndex = 6;
            lbLoading.Text = "⏳ درحال اتصال...";
            // 
            // progressBarLoading
            // 
            progressBarLoading.Location = new Point(25, 50);
            progressBarLoading.MarqueeAnimationSpeed = 30;
            progressBarLoading.Name = "progressBarLoading";
            progressBarLoading.Size = new Size(200, 23);
            progressBarLoading.Style = ProgressBarStyle.Marquee;
            progressBarLoading.TabIndex = 7;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(126, 90);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(94, 29);
            btnConnect.TabIndex = 0;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click_1;
            // 
            // btnDisconnect
            // 
            btnDisconnect.Location = new Point(328, 90);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(94, 29);
            btnDisconnect.TabIndex = 1;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click_1;
            // 
            // btnGI
            // 
            btnGI.Location = new Point(467, 90);
            btnGI.Name = "btnGI";
            btnGI.Size = new Size(94, 29);
            btnGI.TabIndex = 2;
            btnGI.Text = "GI";
            btnGI.UseVisualStyleBackColor = true;
            btnGI.Click += btnGI_Click_1;
            // 
            // lstLog
            // 
            lstLog.FormattingEnabled = true;
            lstLog.Location = new Point(12, 304);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(1240, 244);
            lstLog.TabIndex = 3;
            lstLog.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(328, 32);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(99, 20);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "Disconnected";
            // 
            // lblQoi
            // 
            lblQoi.AutoSize = true;
            lblQoi.Location = new Point(881, 106);
            lblQoi.Name = "lblQoi";
            lblQoi.Size = new Size(50, 20);
            lblQoi.TabIndex = 6;
            lblQoi.Text = "lblQoi";
            // 
            // nudQoi
            // 
            nudQoi.Location = new Point(967, 99);
            nudQoi.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            nudQoi.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudQoi.Name = "nudQoi";
            nudQoi.Size = new Size(150, 27);
            nudQoi.TabIndex = 6;
            nudQoi.TextAlign = HorizontalAlignment.Center;
            nudQoi.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // GroupI
            // 
            GroupI.Location = new Point(710, 102);
            GroupI.Name = "GroupI";
            GroupI.Size = new Size(150, 29);
            GroupI.TabIndex = 7;
            GroupI.Text = "Group Integrrate";
            GroupI.UseVisualStyleBackColor = true;
            GroupI.Click += GroupI_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1295, 746);
            Controls.Add(GroupI);
            Controls.Add(nudQoi);
            Controls.Add(lblQoi);
            Controls.Add(panlLoading);
            Controls.Add(lblStatus);
            Controls.Add(lstLog);
            Controls.Add(btnGI);
            Controls.Add(btnDisconnect);
            Controls.Add(btnConnect);
            Name = "MainForm";
            Text = "IEC 104 Master";
            Load += MainForm_Load;
            panlLoading.ResumeLayout(false);
            panlLoading.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudQoi).EndInit();
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
        private Label lblQoi;
        private NumericUpDown nudQoi;
        private Button GroupI;
    }
}