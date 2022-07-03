namespace Key2Joy.Mapping
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
            this.nudWaitTime = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudWaitTime)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfo.Location = new System.Drawing.Point(5, 5);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(140, 15);
            this.lblInfo.TabIndex = 12;
            this.lblInfo.Text = "Time to wait in milliseconds:";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudWaitTime
            // 
            this.nudWaitTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudWaitTime.Location = new System.Drawing.Point(145, 5);
            this.nudWaitTime.Name = "nudWaitTime";
            this.nudWaitTime.Size = new System.Drawing.Size(169, 20);
            this.nudWaitTime.TabIndex = 11;
            this.nudWaitTime.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudWaitTime.ValueChanged += new System.EventHandler(this.nudWaitTime_ValueChanged);
            this.nudWaitTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtKeyBind_KeyPress);
            // 
            // WaitActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nudWaitTime);
            this.Controls.Add(this.lblInfo);
            this.Name = "WaitActionControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(319, 25);
            ((System.ComponentModel.ISupportInitialize)(this.nudWaitTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.NumericUpDown nudWaitTime;
    }
}
