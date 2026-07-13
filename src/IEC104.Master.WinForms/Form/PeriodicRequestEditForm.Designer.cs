namespace IEC104.Master.WinForms.Form
{
    partial class PeriodicRequestEditForm
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

            this.lblName = new Label();
            this.txtName = new TextBox();
            this.lblType = new Label();
            this.cmbType = new ComboBox();
            this.lblParameter = new Label();
            this.txtParameter = new TextBox();
            this.lblInterval = new Label();
            this.nudInterval = new NumericUpDown();
            this.lblMinutes = new Label();
            this.chkActive = new CheckBox();
            this.btnSave = new Button();
            this.btnCancel = new Button();

            ((System.ComponentModel.ISupportInitialize)this.nudInterval).BeginInit();
            this.SuspendLayout();

            // ===== lblName =====
            this.lblName.AutoSize = true;
            this.lblName.Font = new Font("Segoe UI", 10F);
            this.lblName.Location = new Point(30, 35);
            this.lblName.Name = "lblName";
            this.lblName.Size = new Size(43, 23);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "نام:";

            // ===== txtName =====
            this.txtName.Font = new Font("Segoe UI", 10F);
            this.txtName.Location = new Point(120, 32);
            this.txtName.Name = "txtName";
            this.txtName.Size = new Size(300, 30);
            this.txtName.TabIndex = 1;

            // ===== lblType =====
            this.lblType.AutoSize = true;
            this.lblType.Font = new Font("Segoe UI", 10F);
            this.lblType.Location = new Point(30, 80);
            this.lblType.Name = "lblType";
            this.lblType.Size = new Size(43, 23);
            this.lblType.TabIndex = 2;
            this.lblType.Text = "نوع:";

            // ===== cmbType =====
            this.cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbType.Font = new Font("Segoe UI", 10F);
            this.cmbType.Location = new Point(120, 77);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new Size(300, 28);
            this.cmbType.TabIndex = 3;
            this.cmbType.Items.AddRange(new object[] { "GeneralInterrogation", "GroupInterrogation", "SinglePoint" });

            // ===== lblParameter =====
            this.lblParameter.AutoSize = true;
            this.lblParameter.Font = new Font("Segoe UI", 10F);
            this.lblParameter.Location = new Point(30, 125);
            this.lblParameter.Name = "lblParameter";
            this.lblParameter.Size = new Size(73, 23);
            this.lblParameter.TabIndex = 4;
            this.lblParameter.Text = "پارامتر:";
            // ★ استفاده از _toolTip
            this._toolTip.SetToolTip(this.lblParameter, "برای GI: QOI (پیش‌فرض 20)\nبرای SinglePoint: IOA");

            // ===== txtParameter =====
            this.txtParameter.Font = new Font("Segoe UI", 10F);
            this.txtParameter.Location = new Point(120, 122);
            this.txtParameter.Name = "txtParameter";
            this.txtParameter.Size = new Size(300, 30);
            this.txtParameter.TabIndex = 5;
            this.txtParameter.Text = "20";

            // ===== lblInterval =====
            this.lblInterval.AutoSize = true;
            this.lblInterval.Font = new Font("Segoe UI", 10F);
            this.lblInterval.Location = new Point(30, 170);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new Size(77, 23);
            this.lblInterval.TabIndex = 6;
            this.lblInterval.Text = "بازه زمانی:";

            // ===== nudInterval =====
            this.nudInterval.Font = new Font("Segoe UI", 10F);
            this.nudInterval.Location = new Point(120, 167);
            this.nudInterval.Name = "nudInterval";
            this.nudInterval.Size = new Size(80, 30);
            this.nudInterval.TabIndex = 7;
            this.nudInterval.Minimum = 1;
            this.nudInterval.Maximum = 1440;
            this.nudInterval.Value = 10;

            // ===== lblMinutes =====
            this.lblMinutes.AutoSize = true;
            this.lblMinutes.Font = new Font("Segoe UI", 10F);
            this.lblMinutes.Location = new Point(210, 170);
            this.lblMinutes.Name = "lblMinutes";
            this.lblMinutes.Size = new Size(39, 23);
            this.lblMinutes.TabIndex = 8;
            this.lblMinutes.Text = "دقیقه";

            // ===== chkActive =====
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = CheckState.Checked;
            this.chkActive.Font = new Font("Segoe UI", 10F);
            this.chkActive.Location = new Point(120, 210);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new Size(63, 27);
            this.chkActive.TabIndex = 9;
            this.chkActive.Text = "فعال";
            this.chkActive.UseVisualStyleBackColor = true;

            // ===== btnSave =====
            this.btnSave.BackColor = Color.FromArgb(0, 120, 215);
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new Point(120, 270);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(120, 45);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "💾 ذخیره";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += this.btnSave_Click;

            // ===== btnCancel =====
            this.btnCancel.BackColor = Color.FromArgb(200, 50, 50);
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Location = new Point(260, 270);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(120, 45);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "✖ انصراف";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += this.btnCancel_Click;

            // ===== PeriodicRequestEditForm =====
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(480, 360);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.lblMinutes);
            this.Controls.Add(this.nudInterval);
            this.Controls.Add(this.lblInterval);
            this.Controls.Add(this.txtParameter);
            this.Controls.Add(this.lblParameter);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Font = new Font("Segoe UI", 9F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PeriodicRequestEditForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "✏️ ویرایش درخواست";
            this.Load += this.PeriodicRequestEditForm_Load;

            ((System.ComponentModel.ISupportInitialize)this.nudInterval).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lblName;
        private TextBox txtName;
        private Label lblType;
        private ComboBox cmbType;
        private Label lblParameter;
        private TextBox txtParameter;
        private Label lblInterval;
        private NumericUpDown nudInterval;
        private Label lblMinutes;
        private CheckBox chkActive;
        private Button btnSave;
        private Button btnCancel;
        private ToolTip _toolTip; // ← تغییر نام
    }
}