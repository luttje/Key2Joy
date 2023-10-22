namespace Key2Joy.Gui.Mapping
{
    partial class GamePadTriggerActionControl
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
            this.nudExact = new System.Windows.Forms.NumericUpDown();
            this.lblExact = new System.Windows.Forms.Label();
            this.pnlTriggerInputScale = new System.Windows.Forms.Panel();
            this.nudTriggerInputScale = new System.Windows.Forms.NumericUpDown();
            this.lblTriggerInputScale = new System.Windows.Forms.Label();
            this.chkDeltaFromInput = new System.Windows.Forms.CheckBox();
            this.pnlGamePad.SuspendLayout();
            this.pnlSide.SuspendLayout();
            this.pnlDelta.SuspendLayout();
            this.pnlDeltaConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudExact)).BeginInit();
            this.pnlTriggerInputScale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTriggerInputScale)).BeginInit();
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
            this.lblInfoSide.Text = "Trigger:";
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
            this.pnlDeltaConfig.Controls.Add(this.nudExact);
            this.pnlDeltaConfig.Controls.Add(this.lblExact);
            this.pnlDeltaConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDeltaConfig.Location = new System.Drawing.Point(0, 47);
            this.pnlDeltaConfig.Name = "pnlDeltaConfig";
            this.pnlDeltaConfig.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.pnlDeltaConfig.Size = new System.Drawing.Size(412, 27);
            this.pnlDeltaConfig.TabIndex = 18;
            // 
            // nudExact
            // 
            this.nudExact.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudExact.Location = new System.Drawing.Point(44, 5);
            this.nudExact.Name = "nudExact";
            this.nudExact.Size = new System.Drawing.Size(368, 20);
            this.nudExact.TabIndex = 18;
            this.nudExact.ValueChanged += new System.EventHandler(this.NudExactDelta_ValueChanged);
            // 
            // lblExact
            // 
            this.lblExact.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblExact.Location = new System.Drawing.Point(0, 5);
            this.lblExact.Name = "lblExact";
            this.lblExact.Size = new System.Drawing.Size(44, 22);
            this.lblExact.TabIndex = 15;
            this.lblExact.Text = "Exact:";
            this.lblExact.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlTriggerInputScale
            // 
            this.pnlTriggerInputScale.Controls.Add(this.nudTriggerInputScale);
            this.pnlTriggerInputScale.Controls.Add(this.lblTriggerInputScale);
            this.pnlTriggerInputScale.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTriggerInputScale.Location = new System.Drawing.Point(0, 27);
            this.pnlTriggerInputScale.Name = "pnlTriggerInputScale";
            this.pnlTriggerInputScale.Size = new System.Drawing.Size(412, 20);
            this.pnlTriggerInputScale.TabIndex = 20;
            // 
            // nudTriggerInputScale
            // 
            this.nudTriggerInputScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudTriggerInputScale.Location = new System.Drawing.Point(136, 0);
            this.nudTriggerInputScale.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudTriggerInputScale.Name = "nudTriggerInputScale";
            this.nudTriggerInputScale.Size = new System.Drawing.Size(276, 20);
            this.nudTriggerInputScale.TabIndex = 19;
            this.nudTriggerInputScale.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudTriggerInputScale.ValueChanged += new System.EventHandler(this.NudTriggerInputScale_ValueChanged);
            // 
            // lblTriggerInputScale
            // 
            this.lblTriggerInputScale.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTriggerInputScale.Location = new System.Drawing.Point(0, 0);
            this.lblTriggerInputScale.Name = "lblTriggerInputScale";
            this.lblTriggerInputScale.Size = new System.Drawing.Size(136, 20);
            this.lblTriggerInputScale.TabIndex = 16;
            this.lblTriggerInputScale.Text = "Trigger Input Delta Scale:";
            this.lblTriggerInputScale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // GamePadTriggerActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlDelta);
            this.Controls.Add(this.pnlSide);
            this.Controls.Add(this.pnlGamePad);
            this.Name = "GamePadTriggerActionControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(422, 132);
            this.pnlGamePad.ResumeLayout(false);
            this.pnlSide.ResumeLayout(false);
            this.pnlDelta.ResumeLayout(false);
            this.pnlDelta.PerformLayout();
            this.pnlDeltaConfig.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudExact)).EndInit();
            this.pnlTriggerInputScale.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudTriggerInputScale)).EndInit();
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
        private System.Windows.Forms.Label lblExact;
        private System.Windows.Forms.NumericUpDown nudExact;
        private System.Windows.Forms.Panel pnlTriggerInputScale;
        private System.Windows.Forms.Label lblTriggerInputScale;
        private System.Windows.Forms.NumericUpDown nudTriggerInputScale;
    }
}
