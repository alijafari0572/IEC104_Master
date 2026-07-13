namespace IEC104.Master.WinForms.Form
{
    partial class PeriodicRequestsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.dgvRequests = new DataGridView();
            this.btnAdd = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.btnToggleActive = new Button();
            this.btnSendNow = new Button();
            this.btnClose = new Button();
            this.panelButtons = new Panel();

            ((System.ComponentModel.ISupportInitialize)this.dgvRequests).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();

            // ===== dgvRequests =====
            this.dgvRequests.AllowUserToAddRows = false;
            this.dgvRequests.AllowUserToDeleteRows = false;
            this.dgvRequests.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dgvRequests.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRequests.BackgroundColor = Color.White;
            this.dgvRequests.BorderStyle = BorderStyle.None;
            this.dgvRequests.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRequests.Location = new Point(12, 12);
            this.dgvRequests.Name = "dgvRequests";
            this.dgvRequests.ReadOnly = true;
            this.dgvRequests.RowHeadersVisible = false;
            this.dgvRequests.RowTemplate.Height = 28;
            this.dgvRequests.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvRequests.Size = new Size(776, 350);
            this.dgvRequests.TabIndex = 0;
            this.dgvRequests.MultiSelect = false;

            // ===== panelButtons =====
            this.panelButtons.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.panelButtons.Controls.Add(this.btnAdd);
            this.panelButtons.Controls.Add(this.btnEdit);
            this.panelButtons.Controls.Add(this.btnDelete);
            this.panelButtons.Controls.Add(this.btnToggleActive);
            this.panelButtons.Controls.Add(this.btnSendNow);
            this.panelButtons.Controls.Add(this.btnClose);
            //this.panelButtons.FlowDirection = FlowDirection.LeftToRight;
            this.panelButtons.Location = new Point(12, 370);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new Size(776, 50);
            this.panelButtons.TabIndex = 1;
            //this.panelButtons.WrapContents = false;

            // ===== btnAdd =====
            this.btnAdd.BackColor = Color.FromArgb(0, 120, 215);
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = FlatStyle.Flat;
            this.btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.Location = new Point(3, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(100, 40);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "➕ افزودن";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += this.btnAdd_Click;

            // ===== btnEdit =====
            this.btnEdit.BackColor = Color.FromArgb(255, 193, 7);
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = FlatStyle.Flat;
            this.btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnEdit.ForeColor = Color.Black;
            this.btnEdit.Location = new Point(109, 3);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new Size(100, 40);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "✏️ ویرایش";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += this.btnEdit_Click;

            // ===== btnDelete =====
            this.btnDelete.BackColor = Color.FromArgb(200, 50, 50);
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.Location = new Point(215, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(100, 40);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "🗑️ حذف";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += this.btnDelete_Click;

            // ===== btnToggleActive =====
            this.btnToggleActive.BackColor = Color.FromArgb(50, 180, 100);
            this.btnToggleActive.FlatAppearance.BorderSize = 0;
            this.btnToggleActive.FlatStyle = FlatStyle.Flat;
            this.btnToggleActive.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnToggleActive.ForeColor = Color.White;
            this.btnToggleActive.Location = new Point(321, 3);
            this.btnToggleActive.Name = "btnToggleActive";
            this.btnToggleActive.Size = new Size(130, 40);
            this.btnToggleActive.TabIndex = 3;
            this.btnToggleActive.Text = "🔄 تغییر وضعیت";
            this.btnToggleActive.UseVisualStyleBackColor = false;
            this.btnToggleActive.Click += this.btnToggleActive_Click;

            // ===== btnSendNow =====
            this.btnSendNow.BackColor = Color.FromArgb(156, 39, 176);
            this.btnSendNow.FlatAppearance.BorderSize = 0;
            this.btnSendNow.FlatStyle = FlatStyle.Flat;
            this.btnSendNow.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSendNow.ForeColor = Color.White;
            this.btnSendNow.Location = new Point(457, 3);
            this.btnSendNow.Name = "btnSendNow";
            this.btnSendNow.Size = new Size(130, 40);
            this.btnSendNow.TabIndex = 4;
            this.btnSendNow.Text = "📤 ارسال فوری";
            this.btnSendNow.UseVisualStyleBackColor = false;
            this.btnSendNow.Click += this.btnSendNow_Click;

            // ===== btnClose =====
            this.btnClose.BackColor = Color.FromArgb(100, 100, 100);
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.Location = new Point(670, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(100, 40);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "✖ بستن";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += this.btnClose_Click;

            // ===== PeriodicRequestsForm =====
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(800, 430);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.dgvRequests);
            this.Font = new Font("Segoe UI", 9F);
            this.MinimumSize = new Size(800, 450);
            this.Name = "PeriodicRequestsForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "⏱️ مدیریت درخواست‌های دوره‌ای";
            this.Load += this.PeriodicRequestsForm_Load;

            ((System.ComponentModel.ISupportInitialize)this.dgvRequests).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvRequests;
        private Panel panelButtons;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnToggleActive;
        private Button btnSendNow;
        private Button btnClose;
    }

}
