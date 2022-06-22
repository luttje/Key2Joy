namespace KeyToJoy.Mapping
{
    partial class WaitActionControl
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
            this.lblInfo = new System.Windows.Forms.Label();
            this.txtKeyBind = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblInfo.Location = new System.Drawing.Point(5, 5);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(140, 15);
            this.lblInfo.TabIndex = 12;
            this.lblInfo.Text = "Time to wait in milliseconds:";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtKeyBind
            // 
            this.txtKeyBind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKeyBind.Location = new System.Drawing.Point(145, 5);
            this.txtKeyBind.Name = "txtKeyBind";
            this.txtKeyBind.Size = new System.Drawing.Size(169, 20);
            this.txtKeyBind.TabIndex = 11;
            this.txtKeyBind.Text = "500";
            this.txtKeyBind.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtKeyBind_KeyPress);
            // 
            // WaitActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.txtKeyBind);
            this.Controls.Add(this.lblInfo);
            this.Name = "WaitActionControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(319, 25);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TextBox txtKeyBind;
    }
}
