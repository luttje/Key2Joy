namespace KeyToJoy
{
    partial class MouseButtonTriggerOptionsControl
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
            this.SuspendLayout();
            // 
            // txtKeyBind
            // 
            this.txtKeyBind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKeyBind.Enabled = false;
            this.txtKeyBind.Location = new System.Drawing.Point(87, 5);
            this.txtKeyBind.Name = "txtKeyBind";
            this.txtKeyBind.Size = new System.Drawing.Size(362, 20);
            this.txtKeyBind.TabIndex = 6;
            this.txtKeyBind.Text = "(click this field with any mouse button to select it as the trigger)";
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblInfo.Location = new System.Drawing.Point(5, 5);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(82, 16);
            this.lblInfo.TabIndex = 9;
            this.lblInfo.Text = "Mouse Button:";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MouseButtonTriggerOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.txtKeyBind);
            this.Controls.Add(this.lblInfo);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Name = "MouseButtonTriggerOptionsControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(454, 26);
            this.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.MouseButtonTriggerOptionsControl_ControlRemoved);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtKeyBind;
        private System.Windows.Forms.Label lblInfo;
    }
}