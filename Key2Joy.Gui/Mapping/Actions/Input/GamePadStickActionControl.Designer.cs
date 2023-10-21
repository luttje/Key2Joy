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
            this.nudExactDeltaY = new System.Windows.Forms.NumericUpDown();
            this.lblExactDeltaY = new System.Windows.Forms.Label();
            this.nudExactDeltaX = new System.Windows.Forms.NumericUpDown();
            this.lblExactDeltaX = new System.Windows.Forms.Label();
            this.pnlTriggerInputScale = new System.Windows.Forms.Panel();
            this.nudTriggerInputScaleY = new System.Windows.Forms.NumericUpDown();
            this.lblTriggerInputScaleY = new System.Windows.Forms.Label();
            this.nudTriggerInputScaleX = new System.Windows.Forms.NumericUpDown();
            this.lblTriggerInputScaleX = new System.Windows.Forms.Label();
            this.chkDeltaFromInput = new System.Windows.Forms.CheckBox();
            this.pnlGamePad.SuspendLayout();
            this.pnlSide.SuspendLayout();
            this.pnlDelta.SuspendLayout();
            this.pnlDeltaConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudExactDeltaY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExactDeltaX)).BeginInit();
            this.pnlTriggerInputScale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTriggerInputScaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTriggerInputScaleX)).BeginInit();
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
            this.pnlDeltaConfig.Controls.Add(this.nudExactDeltaY);
            this.pnlDeltaConfig.Controls.Add(this.lblExactDeltaY);
            this.pnlDeltaConfig.Controls.Add(this.nudExactDeltaX);
            this.pnlDeltaConfig.Controls.Add(this.lblExactDeltaX);
            this.pnlDeltaConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDeltaConfig.Location = new System.Drawing.Point(0, 47);
            this.pnlDeltaConfig.Name = "pnlDeltaConfig";
            this.pnlDeltaConfig.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.pnlDeltaConfig.Size = new System.Drawing.Size(412, 27);
            this.pnlDeltaConfig.TabIndex = 18;
            // 
            // nudExactDeltaY
            // 
            this.nudExactDeltaY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudExactDeltaY.Location = new System.Drawing.Point(264, 5);
            this.nudExactDeltaY.Name = "nudExactDeltaY";
            this.nudExactDeltaY.Size = new System.Drawing.Size(148, 20);
            this.nudExactDeltaY.TabIndex = 18;
            this.nudExactDeltaY.ValueChanged += new System.EventHandler(this.NudExactDeltaY_ValueChanged);
            // 
            // lblExactDeltaY
            // 
            this.lblExactDeltaY.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblExactDeltaY.Location = new System.Drawing.Point(237, 5);
            this.lblExactDeltaY.Name = "lblExactDeltaY";
            this.lblExactDeltaY.Size = new System.Drawing.Size(27, 22);
            this.lblExactDeltaY.TabIndex = 17;
            this.lblExactDeltaY.Text = "Y:";
            this.lblExactDeltaY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudExactDeltaX
            // 
            this.nudExactDeltaX.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudExactDeltaX.Location = new System.Drawing.Point(88, 5);
            this.nudExactDeltaX.Name = "nudExactDeltaX";
            this.nudExactDeltaX.Size = new System.Drawing.Size(149, 20);
            this.nudExactDeltaX.TabIndex = 16;
            this.nudExactDeltaX.ValueChanged += new System.EventHandler(this.NudExactDeltaX_ValueChanged);
            // 
            // lblExactDeltaX
            // 
            this.lblExactDeltaX.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblExactDeltaX.Location = new System.Drawing.Point(0, 5);
            this.lblExactDeltaX.Name = "lblExactDeltaX";
            this.lblExactDeltaX.Size = new System.Drawing.Size(88, 22);
            this.lblExactDeltaX.TabIndex = 15;
            this.lblExactDeltaX.Text = "Exact Delta X:";
            this.lblExactDeltaX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.nudTriggerInputScaleY.Name = "nudTriggerInputScaleY";
            this.nudTriggerInputScaleY.Size = new System.Drawing.Size(129, 20);
            this.nudTriggerInputScaleY.TabIndex = 19;
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
            // GamePadStickActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlDelta);
            this.Controls.Add(this.pnlSide);
            this.Controls.Add(this.pnlGamePad);
            this.Name = "GamePadStickActionControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(422, 129);
            this.pnlGamePad.ResumeLayout(false);
            this.pnlSide.ResumeLayout(false);
            this.pnlDelta.ResumeLayout(false);
            this.pnlDelta.PerformLayout();
            this.pnlDeltaConfig.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudExactDeltaY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExactDeltaX)).EndInit();
            this.pnlTriggerInputScale.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudTriggerInputScaleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTriggerInputScaleX)).EndInit();
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
        private System.Windows.Forms.NumericUpDown nudExactDeltaX;
        private System.Windows.Forms.Label lblExactDeltaX;
        private System.Windows.Forms.Label lblExactDeltaY;
        private System.Windows.Forms.NumericUpDown nudExactDeltaY;
        private System.Windows.Forms.Panel pnlTriggerInputScale;
        private System.Windows.Forms.NumericUpDown nudTriggerInputScaleX;
        private System.Windows.Forms.Label lblTriggerInputScaleX;
        private System.Windows.Forms.Label lblTriggerInputScaleY;
        private System.Windows.Forms.NumericUpDown nudTriggerInputScaleY;
    }
}
