namespace IEC104.Master.WinForms.Form
{
    partial class ConnectionSettingsForm
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtHost = new TextBox();
            txtPort = new TextBox();
            txtCommonAddress = new TextBox();
            txtTimeout = new TextBox();
            chkTls = new CheckBox();
            btnOk = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // txtHost
            // 
            txtHost.Location = new Point(64, 58);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(125, 27);
            txtHost.TabIndex = 0;
            txtHost.Text = "txtHost";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(231, 58);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(125, 27);
            txtPort.TabIndex = 1;
            txtPort.Text = "txtPort";
            // 
            // txtCommonAddress
            // 
            txtCommonAddress.Location = new Point(382, 58);
            txtCommonAddress.Name = "txtCommonAddress";
            txtCommonAddress.Size = new Size(125, 27);
            txtCommonAddress.TabIndex = 2;
            txtCommonAddress.Text = "txtCommonAddress";
            // 
            // txtTimeout
            // 
            txtTimeout.Location = new Point(535, 58);
            txtTimeout.Name = "txtTimeout";
            txtTimeout.Size = new Size(125, 27);
            txtTimeout.TabIndex = 3;
            txtTimeout.Text = "txtTimeout";
            // 
            // chkTls
            // 
            chkTls.AutoSize = true;
            chkTls.Location = new Point(693, 61);
            chkTls.Name = "chkTls";
            chkTls.Size = new Size(71, 24);
            chkTls.TabIndex = 4;
            chkTls.Text = "chkTls";
            chkTls.UseVisualStyleBackColor = true;
            chkTls.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // btnOk
            // 
            btnOk.DialogResult = DialogResult.OK;
            btnOk.Location = new Point(192, 219);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(94, 29);
            btnOk.TabIndex = 5;
            btnOk.Text = "btnOk";
            btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(402, 219);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "btnCancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // ConnectionSettingsForm
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(chkTls);
            Controls.Add(txtTimeout);
            Controls.Add(txtCommonAddress);
            Controls.Add(txtPort);
            Controls.Add(txtHost);
            Name = "ConnectionSettingsForm";
            Text = "ConnectionSettingsForm";
            Load += ConnectionSettingsForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtHost;
        private TextBox txtPort;
        private TextBox txtCommonAddress;
        private TextBox txtTimeout;
        private CheckBox chkTls;
        private Button btnOk;
        private Button btnCancel;
    }
}