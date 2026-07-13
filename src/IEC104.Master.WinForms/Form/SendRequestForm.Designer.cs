namespace IEC104.Master.WinForms.Form
{
    partial class SendRequestForm
    {
        private System.ComponentModel.IContainer components = null;
        //private ToolTip _toolTip; // ← تغییر نام

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
            this.components = new System.ComponentModel.Container();
            this._toolTip = new ToolTip(this.components); // ← تغییر نام

            this.lblType = new Label();
            this.cmbType = new ComboBox();
            this.lblParameter = new Label();
            this.txtParameter = new TextBox();
            this.btnSend = new Button();
            this.btnClose = new Button();
            this.grpResult = new GroupBox();
            this.txtResult = new TextBox();
            this.grpResult.SuspendLayout();
            this.SuspendLayout();

            // ===== lblType =====
            this.lblType.AutoSize = true;
            this.lblType.Font = new Font("Segoe UI", 10F);
            this.lblType.Location = new Point(30, 35);
            this.lblType.Name = "lblType";
            this.lblType.Size = new Size(43, 23);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "نوع:";

            // ===== cmbType =====
            this.cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbType.Font = new Font("Segoe UI", 10F);
            this.cmbType.Location = new Point(120, 32);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new Size(220, 28);
            this.cmbType.TabIndex = 1;
            this.cmbType.Items.AddRange(new object[] { "GeneralInterrogation", "GroupInterrogation", "SinglePoint" });
            this.cmbType.SelectedIndex = 0;

            // ===== lblParameter =====
            this.lblParameter.AutoSize = true;
            this.lblParameter.Font = new Font("Segoe UI", 10F);
            this.lblParameter.Location = new Point(30, 80);
            this.lblParameter.Name = "lblParameter";
            this.lblParameter.Size = new Size(73, 23);
            this.lblParameter.TabIndex = 2;
            this.lblParameter.Text = "پارامتر:";
            // ★ استفاده از _toolTip
            this._toolTip.SetToolTip(this.lblParameter, "برای GI: QOI (پیش‌فرض 20)\nبرای SinglePoint: IOA");

            // ===== txtParameter =====
            this.txtParameter.Font = new Font("Segoe UI", 10F);
            this.txtParameter.Location = new Point(120, 77);
            this.txtParameter.Name = "txtParameter";
            this.txtParameter.Size = new Size(220, 30);
            this.txtParameter.TabIndex = 3;
            this.txtParameter.Text = "20";

            // ===== btnSend =====
            this.btnSend.BackColor = Color.FromArgb(0, 120, 215);
            this.btnSend.FlatAppearance.BorderSize = 0;
            this.btnSend.FlatStyle = FlatStyle.Flat;
            this.btnSend.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSend.ForeColor = Color.White;
            this.btnSend.Location = new Point(120, 130);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new Size(130, 45);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "📤 ارسال";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += this.btnSend_Click;

            // ===== btnClose =====
            this.btnClose.BackColor = Color.FromArgb(100, 100, 100);
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.Location = new Point(270, 130);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(130, 45);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "✖ بستن";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += this.btnClose_Click;

            // ===== grpResult =====
            this.grpResult.Controls.Add(this.txtResult);
            this.grpResult.Font = new Font("Segoe UI", 9F);
            this.grpResult.Location = new Point(30, 190);
            this.grpResult.Name = "grpResult";
            this.grpResult.Size = new Size(460, 120);
            this.grpResult.TabIndex = 6;
            this.grpResult.TabStop = false;
            this.grpResult.Text = "نتیجه";

            // ===== txtResult =====
            this.txtResult.BackColor = Color.FromArgb(245, 245, 245);
            this.txtResult.Font = new Font("Consolas", 9F);
            this.txtResult.Location = new Point(6, 30);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = ScrollBars.Vertical;
            this.txtResult.Size = new Size(448, 84);
            this.txtResult.TabIndex = 0;
            this.txtResult.Text = "منتظر ارسال درخواست...";

            // ===== SendRequestForm =====
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(520, 340);
            this.Controls.Add(this.grpResult);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtParameter);
            this.Controls.Add(this.lblParameter);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.lblType);
            this.Font = new Font("Segoe UI", 9F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendRequestForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "📤 ارسال دستی درخواست";
            this.grpResult.ResumeLayout(false);
            this.grpResult.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lblType;
        private ComboBox cmbType;
        private Label lblParameter;
        private TextBox txtParameter;
        private Button btnSend;
        private Button btnClose;
        private GroupBox grpResult;
        private TextBox txtResult;
        private ToolTip _toolTip; // ← تغییر نام
    }
}