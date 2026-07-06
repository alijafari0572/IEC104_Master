namespace IEC104.Master.WinForms.Form
{
    partial class MainForm
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
            btnConnect = new Button();
            btnDisconnect = new Button();
            btnGI = new Button();
            lstLog = new ListBox();
            lblStatus = new Label();
            SuspendLayout();
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(183, 76);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(94, 29);
            btnConnect.TabIndex = 0;
            btnConnect.Text = "btnConnect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click_1;
            // 
            // btnDisconnect
            // 
            btnDisconnect.Location = new Point(328, 90);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(94, 29);
            btnDisconnect.TabIndex = 1;
            btnDisconnect.Text = "btnDisconnect";
            btnDisconnect.UseVisualStyleBackColor = true;
            // 
            // btnGI
            // 
            btnGI.Location = new Point(467, 90);
            btnGI.Name = "btnGI";
            btnGI.Size = new Size(94, 29);
            btnGI.TabIndex = 2;
            btnGI.Text = "btnGI";
            btnGI.UseVisualStyleBackColor = true;
            // 
            // lstLog
            // 
            lstLog.FormattingEnabled = true;
            lstLog.Location = new Point(396, 304);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(150, 104);
            lstLog.TabIndex = 3;
            lstLog.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(434, 201);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(66, 20);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "lblStatus";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblStatus);
            Controls.Add(lstLog);
            Controls.Add(btnGI);
            Controls.Add(btnDisconnect);
            Controls.Add(btnConnect);
            Name = "MainForm";
            Text = "IEC 104 Master";
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnConnect;
        private Button btnDisconnect;
        private Button btnGI;
        private ListBox lstLog;
        private Label lblStatus;
    }
}
