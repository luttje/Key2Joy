namespace Key2Joy.Gui.Mapping
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
            this.nudWaitTimeInMs = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudWaitTimeInMs)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(84, 21);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.Text = "Duration (in ms):";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudWaitTimeInMs
            // 
            this.nudWaitTimeInMs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudWaitTimeInMs.Location = new System.Drawing.Point(84, 0);
            this.nudWaitTimeInMs.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudWaitTimeInMs.Name = "nudWaitTimeInMs";
            this.nudWaitTimeInMs.Size = new System.Drawing.Size(221, 20);
            this.nudWaitTimeInMs.TabIndex = 11;
            this.nudWaitTimeInMs.ValueChanged += new System.EventHandler(this.NudWaitTimeInMs_ValueChanged);
            // 
            // WaitActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nudWaitTimeInMs);
            this.Controls.Add(this.lblInfo);
            this.Name = "WaitActionControl";
            this.Size = new System.Drawing.Size(305, 21);
            ((System.ComponentModel.ISupportInitialize)(this.nudWaitTimeInMs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.NumericUpDown nudWaitTimeInMs;
    }
}
