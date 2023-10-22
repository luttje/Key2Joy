namespace Key2Joy.Gui.Mapping
{
    partial class GamePadTriggerTriggerControl
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
            this.lblInfoSide = new System.Windows.Forms.Label();
            this.pnlStickSide = new System.Windows.Forms.Panel();
            this.cmbStickSide = new System.Windows.Forms.ComboBox();
            this.pnlGamePadIndex = new System.Windows.Forms.Panel();
            this.nudGamePadIndex = new System.Windows.Forms.NumericUpDown();
            this.lblInfoIndex = new System.Windows.Forms.Label();
            this.pnlDeadzone = new System.Windows.Forms.Panel();
            this.pnlDeadzoneConfig = new System.Windows.Forms.Panel();
            this.nudDeadzone = new System.Windows.Forms.NumericUpDown();
            this.lblInfoDeadzone = new System.Windows.Forms.Label();
            this.chkOverrideDeadzone = new System.Windows.Forms.CheckBox();
            this.pnlStickSide.SuspendLayout();
            this.pnlGamePadIndex.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGamePadIndex)).BeginInit();
            this.pnlDeadzone.SuspendLayout();
            this.pnlDeadzoneConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeadzone)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInfoSide
            // 
            this.lblInfoSide.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfoSide.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblInfoSide.Location = new System.Drawing.Point(0, 5);
            this.lblInfoSide.Name = "lblInfoSide";
            this.lblInfoSide.Size = new System.Drawing.Size(59, 18);
            this.lblInfoSide.TabIndex = 8;
            this.lblInfoSide.Text = "Stick Side:";
            this.lblInfoSide.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlStickSide
            // 
            this.pnlStickSide.Controls.Add(this.cmbStickSide);
            this.pnlStickSide.Controls.Add(this.lblInfoSide);
            this.pnlStickSide.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStickSide.Location = new System.Drawing.Point(5, 25);
            this.pnlStickSide.Name = "pnlStickSide";
            this.pnlStickSide.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.pnlStickSide.Size = new System.Drawing.Size(297, 28);
            this.pnlStickSide.TabIndex = 9;
            // 
            // cmbStickSide
            // 
            this.cmbStickSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStickSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStickSide.FormattingEnabled = true;
            this.cmbStickSide.Location = new System.Drawing.Point(59, 5);
            this.cmbStickSide.Name = "cmbStickSide";
            this.cmbStickSide.Size = new System.Drawing.Size(238, 21);
            this.cmbStickSide.TabIndex = 9;
            this.cmbStickSide.SelectedIndexChanged += new System.EventHandler(this.CmbStickSide_SelectedIndexChanged);
            // 
            // pnlGamePadIndex
            // 
            this.pnlGamePadIndex.Controls.Add(this.nudGamePadIndex);
            this.pnlGamePadIndex.Controls.Add(this.lblInfoIndex);
            this.pnlGamePadIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGamePadIndex.Location = new System.Drawing.Point(5, 5);
            this.pnlGamePadIndex.Name = "pnlGamePadIndex";
            this.pnlGamePadIndex.Size = new System.Drawing.Size(297, 20);
            this.pnlGamePadIndex.TabIndex = 10;
            // 
            // nudGamePadIndex
            // 
            this.nudGamePadIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudGamePadIndex.Location = new System.Drawing.Point(69, 0);
            this.nudGamePadIndex.Name = "nudGamePadIndex";
            this.nudGamePadIndex.Size = new System.Drawing.Size(228, 20);
            this.nudGamePadIndex.TabIndex = 10;
            this.nudGamePadIndex.ValueChanged += new System.EventHandler(this.NudGamePadIndex_ValueChanged);
            // 
            // lblInfoIndex
            // 
            this.lblInfoIndex.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfoIndex.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblInfoIndex.Location = new System.Drawing.Point(0, 0);
            this.lblInfoIndex.Name = "lblInfoIndex";
            this.lblInfoIndex.Size = new System.Drawing.Size(69, 20);
            this.lblInfoIndex.TabIndex = 9;
            this.lblInfoIndex.Text = "GamePad #:";
            this.lblInfoIndex.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlDeadzone
            // 
            this.pnlDeadzone.Controls.Add(this.pnlDeadzoneConfig);
            this.pnlDeadzone.Controls.Add(this.chkOverrideDeadzone);
            this.pnlDeadzone.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDeadzone.Location = new System.Drawing.Point(5, 53);
            this.pnlDeadzone.Name = "pnlDeadzone";
            this.pnlDeadzone.Size = new System.Drawing.Size(297, 40);
            this.pnlDeadzone.TabIndex = 11;
            // 
            // pnlDeadzoneConfig
            // 
            this.pnlDeadzoneConfig.Controls.Add(this.nudDeadzone);
            this.pnlDeadzoneConfig.Controls.Add(this.lblInfoDeadzone);
            this.pnlDeadzoneConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDeadzoneConfig.Location = new System.Drawing.Point(0, 17);
            this.pnlDeadzoneConfig.Name = "pnlDeadzoneConfig";
            this.pnlDeadzoneConfig.Size = new System.Drawing.Size(297, 20);
            this.pnlDeadzoneConfig.TabIndex = 12;
            // 
            // nudDeadzone
            // 
            this.nudDeadzone.DecimalPlaces = 4;
            this.nudDeadzone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudDeadzone.Location = new System.Drawing.Point(69, 0);
            this.nudDeadzone.Name = "nudDeadzone";
            this.nudDeadzone.Size = new System.Drawing.Size(228, 20);
            this.nudDeadzone.TabIndex = 1;
            this.nudDeadzone.ValueChanged += new System.EventHandler(this.NudDeadzoneX_ValueChanged);
            // 
            // lblInfoDeadzone
            // 
            this.lblInfoDeadzone.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfoDeadzone.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblInfoDeadzone.Location = new System.Drawing.Point(0, 0);
            this.lblInfoDeadzone.Name = "lblInfoDeadzone";
            this.lblInfoDeadzone.Size = new System.Drawing.Size(69, 20);
            this.lblInfoDeadzone.TabIndex = 9;
            this.lblInfoDeadzone.Text = "Sensitivity:";
            this.lblInfoDeadzone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkOverrideDeadzone
            // 
            this.chkOverrideDeadzone.AutoSize = true;
            this.chkOverrideDeadzone.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkOverrideDeadzone.Location = new System.Drawing.Point(0, 0);
            this.chkOverrideDeadzone.Name = "chkOverrideDeadzone";
            this.chkOverrideDeadzone.Size = new System.Drawing.Size(297, 17);
            this.chkOverrideDeadzone.TabIndex = 0;
            this.chkOverrideDeadzone.Text = "Override default deadzone:";
            this.chkOverrideDeadzone.UseVisualStyleBackColor = true;
            this.chkOverrideDeadzone.CheckedChanged += new System.EventHandler(this.ChkOverrideDeadzone_CheckedChanged);
            // 
            // GamePadTriggerTriggerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.pnlDeadzone);
            this.Controls.Add(this.pnlStickSide);
            this.Controls.Add(this.pnlGamePadIndex);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "GamePadTriggerTriggerControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(307, 95);
            this.pnlStickSide.ResumeLayout(false);
            this.pnlGamePadIndex.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudGamePadIndex)).EndInit();
            this.pnlDeadzone.ResumeLayout(false);
            this.pnlDeadzone.PerformLayout();
            this.pnlDeadzoneConfig.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudDeadzone)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblInfoSide;
        private System.Windows.Forms.Panel pnlStickSide;
        private System.Windows.Forms.ComboBox cmbStickSide;
        private System.Windows.Forms.Panel pnlGamePadIndex;
        private System.Windows.Forms.Label lblInfoIndex;
        private System.Windows.Forms.NumericUpDown nudGamePadIndex;
        private System.Windows.Forms.Panel pnlDeadzone;
        private System.Windows.Forms.NumericUpDown nudDeadzone;
        private System.Windows.Forms.Label lblInfoDeadzone;
        private System.Windows.Forms.CheckBox chkOverrideDeadzone;
        private System.Windows.Forms.Panel pnlDeadzoneConfig;
    }
}
