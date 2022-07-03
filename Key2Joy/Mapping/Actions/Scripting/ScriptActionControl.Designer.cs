namespace Key2Joy.Mapping
{
    partial class ScriptActionControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtScript = new System.Windows.Forms.TextBox();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.chkDirectInput = new System.Windows.Forms.CheckBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.pnlFileInput = new System.Windows.Forms.Panel();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowseFile = new System.Windows.Forms.Button();
            this.pnlTop.SuspendLayout();
            this.pnlFileInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtScript
            // 
            this.txtScript.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtScript.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtScript.Location = new System.Drawing.Point(5, 25);
            this.txtScript.MinimumSize = new System.Drawing.Size(254, 132);
            this.txtScript.Multiline = true;
            this.txtScript.Name = "txtScript";
            this.txtScript.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtScript.Size = new System.Drawing.Size(254, 132);
            this.txtScript.TabIndex = 13;
            this.txtScript.TextChanged += new System.EventHandler(this.txtScript_TextChanged);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.chkDirectInput);
            this.pnlTop.Controls.Add(this.lblInfo);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(5, 5);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(254, 20);
            this.pnlTop.TabIndex = 15;
            // 
            // chkDirectInput
            // 
            this.chkDirectInput.AutoSize = true;
            this.chkDirectInput.Checked = true;
            this.chkDirectInput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDirectInput.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkDirectInput.Location = new System.Drawing.Point(173, 0);
            this.chkDirectInput.Name = "chkDirectInput";
            this.chkDirectInput.Size = new System.Drawing.Size(81, 20);
            this.chkDirectInput.TabIndex = 15;
            this.chkDirectInput.Text = "Direct Input";
            this.chkDirectInput.UseVisualStyleBackColor = true;
            this.chkDirectInput.CheckedChanged += new System.EventHandler(this.chkDirectInput_CheckedChanged);
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(254, 20);
            this.lblInfo.TabIndex = 13;
            this.lblInfo.Text = "X Script:";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlFileInput
            // 
            this.pnlFileInput.Controls.Add(this.txtFilePath);
            this.pnlFileInput.Controls.Add(this.btnBrowseFile);
            this.pnlFileInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFileInput.Location = new System.Drawing.Point(5, 157);
            this.pnlFileInput.Name = "pnlFileInput";
            this.pnlFileInput.Size = new System.Drawing.Size(254, 21);
            this.pnlFileInput.TabIndex = 16;
            // 
            // txtFilePath
            // 
            this.txtFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilePath.Location = new System.Drawing.Point(0, 0);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(190, 20);
            this.txtFilePath.TabIndex = 0;
            // 
            // btnBrowseFile
            // 
            this.btnBrowseFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnBrowseFile.Location = new System.Drawing.Point(190, 0);
            this.btnBrowseFile.Name = "btnBrowseFile";
            this.btnBrowseFile.Size = new System.Drawing.Size(64, 21);
            this.btnBrowseFile.TabIndex = 1;
            this.btnBrowseFile.Text = "Browse...";
            this.btnBrowseFile.UseVisualStyleBackColor = true;
            this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
            // 
            // ScriptActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.pnlFileInput);
            this.Controls.Add(this.txtScript);
            this.Controls.Add(this.pnlTop);
            this.Name = "ScriptActionControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(264, 183);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlFileInput.ResumeLayout(false);
            this.pnlFileInput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtScript;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.CheckBox chkDirectInput;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Panel pnlFileInput;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowseFile;
    }
}
