namespace Key2Joy.Gui.Mapping
{
    partial class MouseMoveTriggerControl
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
            this.cmbMouseDirection = new System.Windows.Forms.ComboBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbMouseDirection
            // 
            this.cmbMouseDirection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbMouseDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMouseDirection.FormattingEnabled = true;
            this.cmbMouseDirection.Location = new System.Drawing.Point(127, 5);
            this.cmbMouseDirection.Name = "cmbMouseDirection";
            this.cmbMouseDirection.Size = new System.Drawing.Size(118, 21);
            this.cmbMouseDirection.TabIndex = 7;
            this.cmbMouseDirection.SelectedIndexChanged += new System.EventHandler(this.CmbMouseDirection_SelectedIndexChanged);
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblInfo.Location = new System.Drawing.Point(5, 5);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(122, 17);
            this.lblInfo.TabIndex = 8;
            this.lblInfo.Text = "Mouse Move Direction:";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MouseMoveTriggerOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.cmbMouseDirection);
            this.Controls.Add(this.lblInfo);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "MouseMoveTriggerOptionsControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(250, 27);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbMouseDirection;
        private System.Windows.Forms.Label lblInfo;
    }
}