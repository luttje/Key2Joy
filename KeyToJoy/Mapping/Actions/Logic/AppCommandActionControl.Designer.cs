namespace KeyToJoy.Mapping
{
    partial class AppCommandActionControl
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
            this.cmbAppCommand = new System.Windows.Forms.ComboBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbAppCommand
            // 
            this.cmbAppCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbAppCommand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAppCommand.FormattingEnabled = true;
            this.cmbAppCommand.Location = new System.Drawing.Point(84, 0);
            this.cmbAppCommand.Name = "cmbAppCommand";
            this.cmbAppCommand.Size = new System.Drawing.Size(221, 21);
            this.cmbAppCommand.TabIndex = 9;
            this.cmbAppCommand.SelectedIndexChanged += new System.EventHandler(this.cmbAppCommand_SelectedIndexChanged);
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(84, 21);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.Text = "App Command:";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AppCommandActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbAppCommand);
            this.Controls.Add(this.lblInfo);
            this.Name = "AppCommandActionControl";
            this.Size = new System.Drawing.Size(305, 21);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbAppCommand;
        private System.Windows.Forms.Label lblInfo;
    }
}
