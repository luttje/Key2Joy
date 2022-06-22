namespace KeyToJoy
{
    partial class KeyboardTriggerOptionsControl
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
            this.txtKeyBind = new System.Windows.Forms.TextBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.chkDown = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtKeyBind
            // 
            this.txtKeyBind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKeyBind.Enabled = false;
            this.txtKeyBind.Location = new System.Drawing.Point(88, 5);
            this.txtKeyBind.Name = "txtKeyBind";
            this.txtKeyBind.Size = new System.Drawing.Size(188, 20);
            this.txtKeyBind.TabIndex = 7;
            this.txtKeyBind.Text = "(press any key to select it as the trigger)";
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblInfo.Location = new System.Drawing.Point(5, 5);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(83, 17);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.Text = "Keyboard Key:";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkDown
            // 
            this.chkDown.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkDown.Location = new System.Drawing.Point(276, 5);
            this.chkDown.Name = "chkDown";
            this.chkDown.Size = new System.Drawing.Size(95, 17);
            this.chkDown.TabIndex = 11;
            this.chkDown.Text = "Pressed Down";
            this.chkDown.UseVisualStyleBackColor = true;
            this.chkDown.CheckedChanged += new System.EventHandler(this.chkDown_CheckedChanged);
            // 
            // KeyboardTriggerOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.txtKeyBind);
            this.Controls.Add(this.chkDown);
            this.Controls.Add(this.lblInfo);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Name = "KeyboardTriggerOptionsControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(376, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtKeyBind;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.CheckBox chkDown;
    }
}