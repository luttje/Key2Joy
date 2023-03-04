namespace Key2Joy.Setup
{
    partial class SetupForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtInstallPath = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnBrowseInstallPath = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmbVersions = new System.Windows.Forms.ComboBox();
            this.rdoUpdateAndInstall = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.rdoUpdateAndInstallPreRelease = new System.Windows.Forms.RadioButton();
            this.rdoDownloadOnly = new System.Windows.Forms.RadioButton();
            this.rdoManualInstallation = new System.Windows.Forms.RadioButton();
            this.btnInstallUpdate = new System.Windows.Forms.Button();
            this.progressUpdate = new System.Windows.Forms.ProgressBar();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Install Path:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtInstallPath
            // 
            this.txtInstallPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInstallPath.Location = new System.Drawing.Point(78, 5);
            this.txtInstallPath.Name = "txtInstallPath";
            this.txtInstallPath.Size = new System.Drawing.Size(617, 20);
            this.txtInstallPath.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtInstallPath);
            this.panel1.Controls.Add(this.btnBrowseInstallPath);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(800, 29);
            this.panel1.TabIndex = 2;
            // 
            // btnBrowseInstallPath
            // 
            this.btnBrowseInstallPath.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnBrowseInstallPath.Location = new System.Drawing.Point(695, 5);
            this.btnBrowseInstallPath.Name = "btnBrowseInstallPath";
            this.btnBrowseInstallPath.Size = new System.Drawing.Size(100, 19);
            this.btnBrowseInstallPath.TabIndex = 2;
            this.btnBrowseInstallPath.Text = "Browse...";
            this.btnBrowseInstallPath.UseVisualStyleBackColor = true;
            this.btnBrowseInstallPath.Click += new System.EventHandler(this.btnBrowseInstallPath_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cmbVersions);
            this.panel2.Controls.Add(this.rdoUpdateAndInstall);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.rdoUpdateAndInstallPreRelease);
            this.panel2.Controls.Add(this.rdoDownloadOnly);
            this.panel2.Controls.Add(this.rdoManualInstallation);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 29);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(800, 29);
            this.panel2.TabIndex = 3;
            // 
            // cmbVersions
            // 
            this.cmbVersions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbVersions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVersions.FormattingEnabled = true;
            this.cmbVersions.Location = new System.Drawing.Point(60, 5);
            this.cmbVersions.Name = "cmbVersions";
            this.cmbVersions.Size = new System.Drawing.Size(179, 21);
            this.cmbVersions.TabIndex = 1;
            // 
            // rdoUpdateAndInstall
            // 
            this.rdoUpdateAndInstall.AutoSize = true;
            this.rdoUpdateAndInstall.Checked = true;
            this.rdoUpdateAndInstall.Dock = System.Windows.Forms.DockStyle.Right;
            this.rdoUpdateAndInstall.Location = new System.Drawing.Point(239, 5);
            this.rdoUpdateAndInstall.Name = "rdoUpdateAndInstall";
            this.rdoUpdateAndInstall.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.rdoUpdateAndInstall.Size = new System.Drawing.Size(106, 19);
            this.rdoUpdateAndInstall.TabIndex = 4;
            this.rdoUpdateAndInstall.TabStop = true;
            this.rdoUpdateAndInstall.Text = "Keep up to date";
            this.rdoUpdateAndInstall.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(5, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 19);
            this.label2.TabIndex = 0;
            this.label2.Text = "Version:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rdoUpdateAndInstallPreRelease
            // 
            this.rdoUpdateAndInstallPreRelease.AutoSize = true;
            this.rdoUpdateAndInstallPreRelease.Dock = System.Windows.Forms.DockStyle.Right;
            this.rdoUpdateAndInstallPreRelease.Location = new System.Drawing.Point(345, 5);
            this.rdoUpdateAndInstallPreRelease.Name = "rdoUpdateAndInstallPreRelease";
            this.rdoUpdateAndInstallPreRelease.Size = new System.Drawing.Size(167, 19);
            this.rdoUpdateAndInstallPreRelease.TabIndex = 2;
            this.rdoUpdateAndInstallPreRelease.Text = "Keep up to date (pre-releases)";
            this.rdoUpdateAndInstallPreRelease.UseVisualStyleBackColor = true;
            // 
            // rdoOnlyDownload
            // 
            this.rdoDownloadOnly.AutoSize = true;
            this.rdoDownloadOnly.Dock = System.Windows.Forms.DockStyle.Right;
            this.rdoDownloadOnly.Location = new System.Drawing.Point(512, 5);
            this.rdoDownloadOnly.Name = "rdoOnlyDownload";
            this.rdoDownloadOnly.Size = new System.Drawing.Size(136, 19);
            this.rdoDownloadOnly.TabIndex = 3;
            this.rdoDownloadOnly.Text = "Only download updates";
            this.rdoDownloadOnly.UseVisualStyleBackColor = true;
            // 
            // rdoManualInstallation
            // 
            this.rdoManualInstallation.AutoSize = true;
            this.rdoManualInstallation.Dock = System.Windows.Forms.DockStyle.Right;
            this.rdoManualInstallation.Location = new System.Drawing.Point(648, 5);
            this.rdoManualInstallation.Name = "rdoManualInstallation";
            this.rdoManualInstallation.Size = new System.Drawing.Size(147, 19);
            this.rdoManualInstallation.TabIndex = 5;
            this.rdoManualInstallation.Text = "Do not download updates";
            this.rdoManualInstallation.UseVisualStyleBackColor = true;
            // 
            // btnInstallUpdate
            // 
            this.btnInstallUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInstallUpdate.Location = new System.Drawing.Point(0, 58);
            this.btnInstallUpdate.Name = "btnInstallUpdate";
            this.btnInstallUpdate.Size = new System.Drawing.Size(800, 32);
            this.btnInstallUpdate.TabIndex = 4;
            this.btnInstallUpdate.Text = "Install";
            this.btnInstallUpdate.UseVisualStyleBackColor = true;
            this.btnInstallUpdate.Click += new System.EventHandler(this.btnInstallUpdate_Click);
            // 
            // progressUpdate
            // 
            this.progressUpdate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressUpdate.Location = new System.Drawing.Point(0, 90);
            this.progressUpdate.Name = "progressUpdate";
            this.progressUpdate.Size = new System.Drawing.Size(800, 27);
            this.progressUpdate.TabIndex = 5;
            // 
            // SetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 117);
            this.Controls.Add(this.btnInstallUpdate);
            this.Controls.Add(this.progressUpdate);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "SetupForm";
            this.Text = "Key2Joy Setup";
            this.Load += new System.EventHandler(this.SetupForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInstallPath;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cmbVersions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdoUpdateAndInstall;
        private System.Windows.Forms.RadioButton rdoUpdateAndInstallPreRelease;
        private System.Windows.Forms.RadioButton rdoDownloadOnly;
        private System.Windows.Forms.RadioButton rdoManualInstallation;
        private System.Windows.Forms.Button btnInstallUpdate;
        private System.Windows.Forms.ProgressBar progressUpdate;
        private System.Windows.Forms.Button btnBrowseInstallPath;
    }
}

