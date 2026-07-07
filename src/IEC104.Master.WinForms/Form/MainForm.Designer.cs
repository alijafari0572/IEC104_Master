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
            this.panlLoading = new Panel();        // ← استفاده از this
            this.lbLoading = new Label();
            this.progressBarLoading = new ProgressBar();
            this.btnConnect = new Button();
            this.btnDisconnect = new Button();
            this.btnGI = new Button();
            this.lstLog = new ListBox();
            this.lblStatus = new Label();

            // ===== پیکربندی panlLoading (پنل اصلی) =====
            this.panlLoading.BackColor = Color.LightGray;
            this.panlLoading.BorderStyle = BorderStyle.FixedSingle;
            this.panlLoading.Location = new Point(275, 150);
            this.panlLoading.Name = "panlLoading";
            this.panlLoading.Size = new Size(250, 100);
            this.panlLoading.TabIndex = 5;
            this.panlLoading.Visible = false;
            // ★ اضافه کردن لیبل و ProgressBar به پنل
            this.panlLoading.Controls.Add(this.lbLoading);
            this.panlLoading.Controls.Add(this.progressBarLoading);

            // ===== پیکربندی lbLoading (لیبل داخل پنل) =====
            this.lbLoading.AutoSize = true;
            this.lbLoading.Location = new Point(75, 15);   // نسبت به پنل
            this.lbLoading.Name = "lbLoading";
            this.lbLoading.Size = new Size(101, 20);
            this.lbLoading.TabIndex = 6;
            this.lbLoading.Text = "⏳ درحال اتصال...";
            this.lbLoading.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lbLoading.ForeColor = Color.DarkBlue;

            // ===== پیکربندی progressBarLoading =====
            this.progressBarLoading.Location = new Point(25, 50); // نسبت به پنل
            this.progressBarLoading.Name = "progressBarLoading";
            this.progressBarLoading.Size = new Size(200, 23);
            this.progressBarLoading.TabIndex = 7;
            this.progressBarLoading.Style = ProgressBarStyle.Marquee;  // ★ حالت چرخشی
            this.progressBarLoading.MarqueeAnimationSpeed = 30;

            // ===== دکمه Connect =====
            this.btnConnect.Location = new Point(126, 90);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new Size(94, 29);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += this.btnConnect_Click_1;

            // ===== دکمه Disconnect =====
            this.btnDisconnect.Location = new Point(328, 90);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new Size(94, 29);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += this.btnDisconnect_Click_1;

            // ===== دکمه GI =====
            this.btnGI.Location = new Point(467, 90);
            this.btnGI.Name = "btnGI";
            this.btnGI.Size = new Size(94, 29);
            this.btnGI.TabIndex = 2;
            this.btnGI.Text = "GI";
            this.btnGI.UseVisualStyleBackColor = true;
            this.btnGI.Click += this.btnGI_Click_1;

            // ===== لیست لاگ =====
            this.lstLog.FormattingEnabled = true;
            this.lstLog.Location = new Point(75, 304);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new Size(582, 104);
            this.lstLog.TabIndex = 3;
            this.lstLog.SelectedIndexChanged += this.listBox1_SelectedIndexChanged;

            // ===== لیبل وضعیت =====
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new Point(328, 32);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(66, 20);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Disconnected";

            // ===== اضافه کردن کنترل‌ها به فرم =====
            // ★ فقط پنل را به فرم اضافه می‌کنیم (لیبل و ProgressBar داخل پنل هستند)
            this.Controls.Add(this.panlLoading);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.btnGI);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);

            // ===== تنظیمات فرم =====
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Name = "MainForm";
            this.Text = "IEC 104 Master";
            this.Load += this.MainForm_Load;
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
    }
}