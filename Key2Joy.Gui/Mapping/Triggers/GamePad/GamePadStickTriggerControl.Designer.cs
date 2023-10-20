namespace Key2Joy.Gui.Mapping
{
    partial class GamePadStickTriggerControl
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
            this.nudDeadzoneY = new System.Windows.Forms.NumericUpDown();
            this.lblInfoDeadzoneY = new System.Windows.Forms.Label();
            this.nudDeadzoneX = new System.Windows.Forms.NumericUpDown();
            this.lblInfoDeadzoneX = new System.Windows.Forms.Label();
            this.chkOverrideDeadzone = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlStickSide.SuspendLayout();
            this.pnlGamePadIndex.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGamePadIndex)).BeginInit();
            this.pnlDeadzone.SuspendLayout();
            this.pnlDeadzoneConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeadzoneY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeadzoneX)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInfoSide
            // 
            this.lblInfoSide.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfoSide.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblInfoSide.Location = new System.Drawing.Point(0, 0);
            this.lblInfoSide.Name = "lblInfoSide";
            this.lblInfoSide.Size = new System.Drawing.Size(75, 24);
            this.lblInfoSide.TabIndex = 8;
            this.lblInfoSide.Text = "Stick Side:";
            this.lblInfoSide.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlStickSide
            // 
            this.pnlStickSide.Controls.Add(this.cmbStickSide);
            this.pnlStickSide.Controls.Add(this.lblInfoSide);
            this.pnlStickSide.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStickSide.Location = new System.Drawing.Point(5, 75);
            this.pnlStickSide.Name = "pnlStickSide";
            this.pnlStickSide.Size = new System.Drawing.Size(297, 24);
            this.pnlStickSide.TabIndex = 9;
            // 
            // cmbStickSide
            // 
            this.cmbStickSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStickSide.FormattingEnabled = true;
            this.cmbStickSide.Location = new System.Drawing.Point(75, 0);
            this.cmbStickSide.Name = "cmbStickSide";
            this.cmbStickSide.Size = new System.Drawing.Size(222, 21);
            this.cmbStickSide.TabIndex = 9;
            this.cmbStickSide.SelectedIndexChanged += new System.EventHandler(this.cmbStickSide_SelectedIndexChanged);
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
            this.nudGamePadIndex.Location = new System.Drawing.Point(75, 0);
            this.nudGamePadIndex.Name = "nudGamePadIndex";
            this.nudGamePadIndex.Size = new System.Drawing.Size(222, 20);
            this.nudGamePadIndex.TabIndex = 10;
            this.nudGamePadIndex.ValueChanged += new System.EventHandler(this.nudGamePadIndex_ValueChanged);
            // 
            // lblInfoIndex
            // 
            this.lblInfoIndex.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfoIndex.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblInfoIndex.Location = new System.Drawing.Point(0, 0);
            this.lblInfoIndex.Name = "lblInfoIndex";
            this.lblInfoIndex.Size = new System.Drawing.Size(75, 20);
            this.lblInfoIndex.TabIndex = 9;
            this.lblInfoIndex.Text = "GamePad #:";
            this.lblInfoIndex.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlDeadzone
            // 
            this.pnlDeadzone.Controls.Add(this.pnlDeadzoneConfig);
            this.pnlDeadzone.Controls.Add(this.chkOverrideDeadzone);
            this.pnlDeadzone.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDeadzone.Location = new System.Drawing.Point(5, 99);
            this.pnlDeadzone.Name = "pnlDeadzone";
            this.pnlDeadzone.Size = new System.Drawing.Size(297, 40);
            this.pnlDeadzone.TabIndex = 11;
            // 
            // pnlDeadzoneConfig
            // 
            this.pnlDeadzoneConfig.Controls.Add(this.nudDeadzoneY);
            this.pnlDeadzoneConfig.Controls.Add(this.lblInfoDeadzoneY);
            this.pnlDeadzoneConfig.Controls.Add(this.nudDeadzoneX);
            this.pnlDeadzoneConfig.Controls.Add(this.lblInfoDeadzoneX);
            this.pnlDeadzoneConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDeadzoneConfig.Location = new System.Drawing.Point(0, 17);
            this.pnlDeadzoneConfig.Name = "pnlDeadzoneConfig";
            this.pnlDeadzoneConfig.Size = new System.Drawing.Size(297, 20);
            this.pnlDeadzoneConfig.TabIndex = 12;
            // 
            // nudDeadzoneY
            // 
            this.nudDeadzoneY.DecimalPlaces = 4;
            this.nudDeadzoneY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudDeadzoneY.Location = new System.Drawing.Point(177, 0);
            this.nudDeadzoneY.Name = "nudDeadzoneY";
            this.nudDeadzoneY.Size = new System.Drawing.Size(120, 20);
            this.nudDeadzoneY.TabIndex = 11;
            this.nudDeadzoneY.ValueChanged += new System.EventHandler(this.nudDeadzoneY_ValueChanged);
            // 
            // lblInfoDeadzoneY
            // 
            this.lblInfoDeadzoneY.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfoDeadzoneY.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblInfoDeadzoneY.Location = new System.Drawing.Point(139, 0);
            this.lblInfoDeadzoneY.Name = "lblInfoDeadzoneY";
            this.lblInfoDeadzoneY.Size = new System.Drawing.Size(38, 20);
            this.lblInfoDeadzoneY.TabIndex = 10;
            this.lblInfoDeadzoneY.Text = "Y:";
            this.lblInfoDeadzoneY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudDeadzoneX
            // 
            this.nudDeadzoneX.DecimalPlaces = 4;
            this.nudDeadzoneX.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudDeadzoneX.Location = new System.Drawing.Point(26, 0);
            this.nudDeadzoneX.Name = "nudDeadzoneX";
            this.nudDeadzoneX.Size = new System.Drawing.Size(113, 20);
            this.nudDeadzoneX.TabIndex = 1;
            this.nudDeadzoneX.ValueChanged += new System.EventHandler(this.nudDeadzoneX_ValueChanged);
            // 
            // lblInfoDeadzoneX
            // 
            this.lblInfoDeadzoneX.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfoDeadzoneX.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblInfoDeadzoneX.Location = new System.Drawing.Point(0, 0);
            this.lblInfoDeadzoneX.Name = "lblInfoDeadzoneX";
            this.lblInfoDeadzoneX.Size = new System.Drawing.Size(26, 20);
            this.lblInfoDeadzoneX.TabIndex = 9;
            this.lblInfoDeadzoneX.Text = "X:";
            this.lblInfoDeadzoneX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.chkOverrideDeadzone.CheckedChanged += new System.EventHandler(this.chkOverrideDeadzone_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(5, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(297, 50);
            this.label1.TabIndex = 11;
            this.label1.Text = "Beware that GamePad Actions (that simulate gamepad input) can trigger gamepads wi" +
    "th the same #. Be careful not to create an infinite loop.";
            // 
            // GamePadStickTriggerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.pnlDeadzone);
            this.Controls.Add(this.pnlStickSide);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlGamePadIndex);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "GamePadStickTriggerControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(307, 139);
            this.pnlStickSide.ResumeLayout(false);
            this.pnlGamePadIndex.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudGamePadIndex)).EndInit();
            this.pnlDeadzone.ResumeLayout(false);
            this.pnlDeadzone.PerformLayout();
            this.pnlDeadzoneConfig.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudDeadzoneY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeadzoneX)).EndInit();
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
        private System.Windows.Forms.NumericUpDown nudDeadzoneY;
        private System.Windows.Forms.Label lblInfoDeadzoneY;
        private System.Windows.Forms.NumericUpDown nudDeadzoneX;
        private System.Windows.Forms.Label lblInfoDeadzoneX;
        private System.Windows.Forms.CheckBox chkOverrideDeadzone;
        private System.Windows.Forms.Panel pnlDeadzoneConfig;
        private System.Windows.Forms.Label label1;
    }
}
