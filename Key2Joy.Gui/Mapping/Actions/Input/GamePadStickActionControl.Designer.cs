namespace Key2Joy.Gui.Mapping
{
    partial class GamePadStickActionControl
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
            this.cmbSide = new System.Windows.Forms.ComboBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.cmbGamePadIndex = new System.Windows.Forms.ComboBox();
            this.lblInfoSide = new System.Windows.Forms.Label();
            this.pnlGamePad = new System.Windows.Forms.Panel();
            this.pnlSide = new System.Windows.Forms.Panel();
            this.pnlDelta = new System.Windows.Forms.Panel();
            this.pnlDeltaConfig = new System.Windows.Forms.Panel();
            this.nudExactY = new System.Windows.Forms.NumericUpDown();
            this.lblExactY = new System.Windows.Forms.Label();
            this.nudExactX = new System.Windows.Forms.NumericUpDown();
            this.lblExactX = new System.Windows.Forms.Label();
            this.pnlTriggerInputScale = new System.Windows.Forms.Panel();
            this.nudTriggerInputScaleY = new System.Windows.Forms.NumericUpDown();
            this.lblTriggerInputScaleY = new System.Windows.Forms.Label();
            this.nudTriggerInputScaleX = new System.Windows.Forms.NumericUpDown();
            this.lblTriggerInputScaleX = new System.Windows.Forms.Label();
            this.chkDeltaFromInput = new System.Windows.Forms.CheckBox();
            this.pnlReset = new System.Windows.Forms.Panel();
            this.nudResetAfterMs = new System.Windows.Forms.NumericUpDown();
            this.lblReset = new System.Windows.Forms.Label();
            this.pnlGamePad.SuspendLayout();
            this.pnlSide.SuspendLayout();
            this.pnlDelta.SuspendLayout();
            this.pnlDeltaConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudExactY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExactX)).BeginInit();
            this.pnlTriggerInputScale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTriggerInputScaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTriggerInputScaleX)).BeginInit();
            this.pnlReset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudResetAfterMs)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbSide
            // 
            this.cmbSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSide.FormattingEnabled = true;
            this.cmbSide.Location = new System.Drawing.Point(44, 5);
            this.cmbSide.Name = "cmbSide";
            this.cmbSide.Size = new System.Drawing.Size(368, 21);
            this.cmbSide.TabIndex = 9;
            this.cmbSide.SelectedIndexChanged += new System.EventHandler(this.CmbGamePad_SelectedIndexChanged);
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(65, 24);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.Text = "GamePad #";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbGamePadIndex
            // 
            this.cmbGamePadIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbGamePadIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGamePadIndex.FormattingEnabled = true;
            this.cmbGamePadIndex.Location = new System.Drawing.Point(65, 0);
            this.cmbGamePadIndex.Name = "cmbGamePadIndex";
            this.cmbGamePadIndex.Size = new System.Drawing.Size(347, 21);
            this.cmbGamePadIndex.TabIndex = 13;
            this.cmbGamePadIndex.SelectedIndexChanged += new System.EventHandler(this.CmbGamePadIndex_SelectedIndexChanged);
            // 
            // lblInfoSide
            // 
            this.lblInfoSide.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfoSide.Location = new System.Drawing.Point(0, 5);
            this.lblInfoSide.Name = "lblInfoSide";
            this.lblInfoSide.Size = new System.Drawing.Size(44, 17);
            this.lblInfoSide.TabIndex = 14;
            this.lblInfoSide.Text = "Stick:";
            this.lblInfoSide.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlGamePad
            // 
            this.pnlGamePad.Controls.Add(this.cmbGamePadIndex);
            this.pnlGamePad.Controls.Add(this.lblInfo);
            this.pnlGamePad.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGamePad.Location = new System.Drawing.Point(5, 5);
            this.pnlGamePad.Name = "pnlGamePad";
            this.pnlGamePad.Size = new System.Drawing.Size(412, 24);
            this.pnlGamePad.TabIndex = 15;
            // 
            // pnlSide
            // 
            this.pnlSide.Controls.Add(this.cmbSide);
            this.pnlSide.Controls.Add(this.lblInfoSide);
            this.pnlSide.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSide.Location = new System.Drawing.Point(5, 29);
            this.pnlSide.Name = "pnlSide";
            this.pnlSide.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.pnlSide.Size = new System.Drawing.Size(412, 27);
            this.pnlSide.TabIndex = 16;
            // 
            // pnlDelta
            // 
            this.pnlDelta.Controls.Add(this.pnlDeltaConfig);
            this.pnlDelta.Controls.Add(this.pnlTriggerInputScale);
            this.pnlDelta.Controls.Add(this.chkDeltaFromInput);
            this.pnlDelta.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDelta.Location = new System.Drawing.Point(5, 56);
            this.pnlDelta.Name = "pnlDelta";
            this.pnlDelta.Size = new System.Drawing.Size(412, 74);
            this.pnlDelta.TabIndex = 17;
            // 
            // pnlDeltaConfig
            // 
            this.pnlDeltaConfig.Controls.Add(this.nudExactY);
            this.pnlDeltaConfig.Controls.Add(this.lblExactY);
            this.pnlDeltaConfig.Controls.Add(this.nudExactX);
            this.pnlDeltaConfig.Controls.Add(this.lblExactX);
            this.pnlDeltaConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDeltaConfig.Location = new System.Drawing.Point(0, 47);
            this.pnlDeltaConfig.Name = "pnlDeltaConfig";
            this.pnlDeltaConfig.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.pnlDeltaConfig.Size = new System.Drawing.Size(412, 27);
            this.pnlDeltaConfig.TabIndex = 18;
            // 
            // nudExactY
            // 
            this.nudExactY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudExactY.Location = new System.Drawing.Point(241, 5);
            this.nudExactY.Name = "nudExactY";
            this.nudExactY.Size = new System.Drawing.Size(171, 20);
            this.nudExactY.TabIndex = 18;
            this.nudExactY.ValueChanged += new System.EventHandler(this.NudExactDeltaY_ValueChanged);
            // 
            // lblExactY
            // 
            this.lblExactY.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblExactY.Location = new System.Drawing.Point(214, 5);
            this.lblExactY.Name = "lblExactY";
            this.lblExactY.Size = new System.Drawing.Size(27, 22);
            this.lblExactY.TabIndex = 17;
            this.lblExactY.Text = "Y:";
            this.lblExactY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudExactX
            // 
            this.nudExactX.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudExactX.Location = new System.Drawing.Point(65, 5);
            this.nudExactX.Name = "nudExactX";
            this.nudExactX.Size = new System.Drawing.Size(149, 20);
            this.nudExactX.TabIndex = 16;
            this.nudExactX.ValueChanged += new System.EventHandler(this.NudExactDeltaX_ValueChanged);
            // 
            // lblExactX
            // 
            this.lblExactX.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblExactX.Location = new System.Drawing.Point(0, 5);
            this.lblExactX.Name = "lblExactX";
            this.lblExactX.Size = new System.Drawing.Size(65, 22);
            this.lblExactX.TabIndex = 15;
            this.lblExactX.Text = "Exact X:";
            this.lblExactX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlTriggerInputScale
            // 
            this.pnlTriggerInputScale.Controls.Add(this.nudTriggerInputScaleY);
            this.pnlTriggerInputScale.Controls.Add(this.lblTriggerInputScaleY);
            this.pnlTriggerInputScale.Controls.Add(this.nudTriggerInputScaleX);
            this.pnlTriggerInputScale.Controls.Add(this.lblTriggerInputScaleX);
            this.pnlTriggerInputScale.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTriggerInputScale.Location = new System.Drawing.Point(0, 27);
            this.pnlTriggerInputScale.Name = "pnlTriggerInputScale";
            this.pnlTriggerInputScale.Size = new System.Drawing.Size(412, 20);
            this.pnlTriggerInputScale.TabIndex = 20;
            // 
            // nudTriggerInputScaleY
            // 
            this.nudTriggerInputScaleY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudTriggerInputScaleY.Location = new System.Drawing.Point(283, 0);
            this.nudTriggerInputScaleY.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudTriggerInputScaleY.Name = "nudTriggerInputScaleY";
            this.nudTriggerInputScaleY.Size = new System.Drawing.Size(129, 20);
            this.nudTriggerInputScaleY.TabIndex = 19;
            this.nudTriggerInputScaleY.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudTriggerInputScaleY.ValueChanged += new System.EventHandler(this.NudTriggerInputScaleY_ValueChanged);
            // 
            // lblTriggerInputScaleY
            // 
            this.lblTriggerInputScaleY.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTriggerInputScaleY.Location = new System.Drawing.Point(247, 0);
            this.lblTriggerInputScaleY.Name = "lblTriggerInputScaleY";
            this.lblTriggerInputScaleY.Size = new System.Drawing.Size(36, 20);
            this.lblTriggerInputScaleY.TabIndex = 18;
            this.lblTriggerInputScaleY.Text = "Y:";
            this.lblTriggerInputScaleY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudTriggerInputScaleX
            // 
            this.nudTriggerInputScaleX.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudTriggerInputScaleX.Location = new System.Drawing.Point(142, 0);
            this.nudTriggerInputScaleX.Name = "nudTriggerInputScaleX";
            this.nudTriggerInputScaleX.Size = new System.Drawing.Size(105, 20);
            this.nudTriggerInputScaleX.TabIndex = 17;
            this.nudTriggerInputScaleX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTriggerInputScaleX.ValueChanged += new System.EventHandler(this.NudTriggerInputScaleX_ValueChanged);
            // 
            // lblTriggerInputScaleX
            // 
            this.lblTriggerInputScaleX.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTriggerInputScaleX.Location = new System.Drawing.Point(0, 0);
            this.lblTriggerInputScaleX.Name = "lblTriggerInputScaleX";
            this.lblTriggerInputScaleX.Size = new System.Drawing.Size(142, 20);
            this.lblTriggerInputScaleX.TabIndex = 16;
            this.lblTriggerInputScaleX.Text = "Trigger Input Delta Scale X:";
            this.lblTriggerInputScaleX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkDeltaFromInput
            // 
            this.chkDeltaFromInput.AutoSize = true;
            this.chkDeltaFromInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkDeltaFromInput.Location = new System.Drawing.Point(0, 0);
            this.chkDeltaFromInput.Name = "chkDeltaFromInput";
            this.chkDeltaFromInput.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.chkDeltaFromInput.Size = new System.Drawing.Size(412, 27);
            this.chkDeltaFromInput.TabIndex = 19;
            this.chkDeltaFromInput.Text = "Use delta from trigger input (e.g: amount of pixels the cursor moved)";
            this.chkDeltaFromInput.UseVisualStyleBackColor = true;
            this.chkDeltaFromInput.CheckedChanged += new System.EventHandler(this.ChkDeltaFromInput_CheckedChanged);
            // 
            // pnlReset
            // 
            this.pnlReset.Controls.Add(this.nudResetAfterMs);
            this.pnlReset.Controls.Add(this.lblReset);
            this.pnlReset.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlReset.Location = new System.Drawing.Point(5, 130);
            this.pnlReset.Name = "pnlReset";
            this.pnlReset.Size = new System.Drawing.Size(412, 22);
            this.pnlReset.TabIndex = 21;
            // 
            // nudResetAfterMs
            // 
            this.nudResetAfterMs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudResetAfterMs.Location = new System.Drawing.Point(191, 0);
            this.nudResetAfterMs.Name = "nudResetAfterMs";
            this.nudResetAfterMs.Size = new System.Drawing.Size(221, 20);
            this.nudResetAfterMs.TabIndex = 19;
            this.nudResetAfterMs.ValueChanged += new System.EventHandler(this.NudResetAfterMs_ValueChanged);
            // 
            // lblReset
            // 
            this.lblReset.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblReset.Location = new System.Drawing.Point(0, 0);
            this.lblReset.Name = "lblReset";
            this.lblReset.Size = new System.Drawing.Size(191, 22);
            this.lblReset.TabIndex = 18;
            this.lblReset.Text = "Reset stick to 0, 0 after (milliseconds):";
            this.lblReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GamePadStickActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlReset);
            this.Controls.Add(this.pnlDelta);
            this.Controls.Add(this.pnlSide);
            this.Controls.Add(this.pnlGamePad);
            this.Name = "GamePadStickActionControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(422, 154);
            this.pnlGamePad.ResumeLayout(false);
            this.pnlSide.ResumeLayout(false);
            this.pnlDelta.ResumeLayout(false);
            this.pnlDelta.PerformLayout();
            this.pnlDeltaConfig.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudExactY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExactX)).EndInit();
            this.pnlTriggerInputScale.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudTriggerInputScaleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTriggerInputScaleX)).EndInit();
            this.pnlReset.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudResetAfterMs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbSide;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ComboBox cmbGamePadIndex;
        private System.Windows.Forms.Label lblInfoSide;
        private System.Windows.Forms.Panel pnlGamePad;
        private System.Windows.Forms.Panel pnlSide;
        private System.Windows.Forms.Panel pnlDelta;
        private System.Windows.Forms.Panel pnlDeltaConfig;
        private System.Windows.Forms.CheckBox chkDeltaFromInput;
        private System.Windows.Forms.NumericUpDown nudExactX;
        private System.Windows.Forms.Label lblExactX;
        private System.Windows.Forms.Label lblExactY;
        private System.Windows.Forms.NumericUpDown nudExactY;
        private System.Windows.Forms.Panel pnlTriggerInputScale;
        private System.Windows.Forms.NumericUpDown nudTriggerInputScaleX;
        private System.Windows.Forms.Label lblTriggerInputScaleX;
        private System.Windows.Forms.Label lblTriggerInputScaleY;
        private System.Windows.Forms.NumericUpDown nudTriggerInputScaleY;
        private System.Windows.Forms.Panel pnlReset;
        private System.Windows.Forms.NumericUpDown nudResetAfterMs;
        private System.Windows.Forms.Label lblReset;
    }
}
