namespace IEC104.Master.WinForms.Form
{
    partial class ConnectionSettingsForm
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
            this.lblHost = new Label();
            this.lblPort = new Label();
            this.lblCommonAddress = new Label();
            this.lblTimeout = new Label();
            this.lblTls = new Label();

            this.txtHost = new TextBox();
            this.txtPort = new TextBox();
            this.txtCommonAddress = new TextBox();
            this.txtTimeout = new TextBox();
            this.chkTls = new CheckBox();

            this.btnOk = new Button();
            this.btnCancel = new Button();

            // ===== فرم =====
            this.SuspendLayout();

            // ===== lblHost =====
            this.lblHost.AutoSize = true;
            this.lblHost.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblHost.ForeColor = Color.FromArgb(50, 50, 50);
            this.lblHost.Location = new Point(30, 40);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new Size(70, 23);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "آدرس IP:";
            this.lblHost.TextAlign = ContentAlignment.MiddleRight;

            // ===== lblPort =====
            this.lblPort.AutoSize = true;
            this.lblPort.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblPort.ForeColor = Color.FromArgb(50, 50, 50);
            this.lblPort.Location = new Point(30, 85);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new Size(44, 23);
            this.lblPort.TabIndex = 1;
            this.lblPort.Text = "پورت:";
            this.lblPort.TextAlign = ContentAlignment.MiddleRight;

            // ===== lblCommonAddress =====
            this.lblCommonAddress.AutoSize = true;
            this.lblCommonAddress.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblCommonAddress.ForeColor = Color.FromArgb(50, 50, 50);
            this.lblCommonAddress.Location = new Point(30, 130);
            this.lblCommonAddress.Name = "lblCommonAddress";
            this.lblCommonAddress.Size = new Size(88, 23);
            this.lblCommonAddress.TabIndex = 2;
            this.lblCommonAddress.Text = "آدرس مشترک:";
            this.lblCommonAddress.TextAlign = ContentAlignment.MiddleRight;

            // ===== lblTimeout =====
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTimeout.ForeColor = Color.FromArgb(50, 50, 50);
            this.lblTimeout.Location = new Point(30, 175);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new Size(91, 23);
            this.lblTimeout.TabIndex = 3;
            this.lblTimeout.Text = "زمان انتظار (ms):";
            this.lblTimeout.TextAlign = ContentAlignment.MiddleRight;

            // ===== lblTls =====
            this.lblTls.AutoSize = true;
            this.lblTls.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTls.ForeColor = Color.FromArgb(50, 50, 50);
            this.lblTls.Location = new Point(30, 220);
            this.lblTls.Name = "lblTls";
            this.lblTls.Size = new Size(40, 23);
            this.lblTls.TabIndex = 4;
            this.lblTls.Text = "TLS:";
            this.lblTls.TextAlign = ContentAlignment.MiddleRight;

            // ===== txtHost =====
            this.txtHost.BackColor = Color.White;
            this.txtHost.BorderStyle = BorderStyle.FixedSingle;
            this.txtHost.Font = new Font("Segoe UI", 10F);
            this.txtHost.ForeColor = Color.FromArgb(50, 50, 50);
            this.txtHost.Location = new Point(140, 37);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new Size(200, 30);
            this.txtHost.TabIndex = 5;
            this.txtHost.Text = "127.0.0.1";

            // ===== txtPort =====
            this.txtPort.BackColor = Color.White;
            this.txtPort.BorderStyle = BorderStyle.FixedSingle;
            this.txtPort.Font = new Font("Segoe UI", 10F);
            this.txtPort.ForeColor = Color.FromArgb(50, 50, 50);
            this.txtPort.Location = new Point(140, 82);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new Size(200, 30);
            this.txtPort.TabIndex = 6;
            this.txtPort.Text = "2404";

            // ===== txtCommonAddress =====
            this.txtCommonAddress.BackColor = Color.White;
            this.txtCommonAddress.BorderStyle = BorderStyle.FixedSingle;
            this.txtCommonAddress.Font = new Font("Segoe UI", 10F);
            this.txtCommonAddress.ForeColor = Color.FromArgb(50, 50, 50);
            this.txtCommonAddress.Location = new Point(140, 127);
            this.txtCommonAddress.Name = "txtCommonAddress";
            this.txtCommonAddress.Size = new Size(200, 30);
            this.txtCommonAddress.TabIndex = 7;
            this.txtCommonAddress.Text = "1";

            // ===== txtTimeout =====
            this.txtTimeout.BackColor = Color.White;
            this.txtTimeout.BorderStyle = BorderStyle.FixedSingle;
            this.txtTimeout.Font = new Font("Segoe UI", 10F);
            this.txtTimeout.ForeColor = Color.FromArgb(50, 50, 50);
            this.txtTimeout.Location = new Point(140, 172);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new Size(200, 30);
            this.txtTimeout.TabIndex = 8;
            this.txtTimeout.Text = "5000";

            // ===== chkTls =====
            this.chkTls.AutoSize = true;
            this.chkTls.Font = new Font("Segoe UI", 10F);
            this.chkTls.ForeColor = Color.FromArgb(50, 50, 50);
            this.chkTls.Location = new Point(140, 220);
            this.chkTls.Name = "chkTls";
            this.chkTls.Size = new Size(55, 27);
            this.chkTls.TabIndex = 9;
            this.chkTls.Text = "فعال";
            this.chkTls.UseVisualStyleBackColor = true;
            this.chkTls.CheckedChanged += this.checkBox1_CheckedChanged;

            // ===== btnOk =====
            this.btnOk.BackColor = Color.FromArgb(0, 120, 215);
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatStyle = FlatStyle.Flat;
            this.btnOk.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnOk.ForeColor = Color.White;
            this.btnOk.Location = new Point(140, 280);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(130, 47);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "✅ تأیید";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += this.btnOk_Click;

            // ===== btnCancel =====
            this.btnCancel.BackColor = Color.FromArgb(200, 50, 50);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Location = new Point(290, 280);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(130, 47);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "❌ انصراف";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += this.btnCancel_Click;

            // ===== ConnectionSettingsForm =====
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new Size(460, 370);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chkTls);
            this.Controls.Add(this.txtTimeout);
            this.Controls.Add(this.txtCommonAddress);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.lblTls);
            this.Controls.Add(this.lblTimeout);
            this.Controls.Add(this.lblCommonAddress);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblHost);
            this.Font = new Font("Segoe UI", 9F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.Name = "ConnectionSettingsForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "⚙️ تنظیمات اتصال";
            this.Load += this.ConnectionSettingsForm_Load;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        // ===== فیلدهای کلاس =====
        private Label lblHost;
        private Label lblPort;
        private Label lblCommonAddress;
        private Label lblTimeout;
        private Label lblTls;

        private TextBox txtHost;
        private TextBox txtPort;
        private TextBox txtCommonAddress;
        private TextBox txtTimeout;
        private CheckBox chkTls;

        private Button btnOk;
        private Button btnCancel;
    }
}