namespace Key2Joy.Gui.Mapping
{
    partial class GamePadButtonTriggerControl
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
            this.txtButtonBind = new System.Windows.Forms.TextBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.cmbPressState = new System.Windows.Forms.ComboBox();
            this.pnlGamePadIndex = new System.Windows.Forms.Panel();
            this.nudGamePadIndex = new System.Windows.Forms.NumericUpDown();
            this.lblInfoIndex = new System.Windows.Forms.Label();
            this.pnlButton = new System.Windows.Forms.Panel();
            this.lblLastGamePadLabel = new System.Windows.Forms.Label();
            this.pnlGamePadIndex.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGamePadIndex)).BeginInit();
            this.pnlButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtButtonBind
            // 
            this.txtButtonBind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtButtonBind.Location = new System.Drawing.Point(43, 5);
            this.txtButtonBind.Name = "txtButtonBind";
            this.txtButtonBind.ReadOnly = true;
            this.txtButtonBind.Size = new System.Drawing.Size(249, 20);
            this.txtButtonBind.TabIndex = 7;
            this.txtButtonBind.Text = "(click here, then press any button to select it as the trigger)";
            this.txtButtonBind.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TxtKeyBind_MouseUp);
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblInfo.Location = new System.Drawing.Point(0, 5);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(43, 16);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.Text = "Button:";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPressState
            // 
            this.cmbPressState.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmbPressState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPressState.FormattingEnabled = true;
            this.cmbPressState.Location = new System.Drawing.Point(292, 5);
            this.cmbPressState.Name = "cmbPressState";
            this.cmbPressState.Size = new System.Drawing.Size(74, 21);
            this.cmbPressState.TabIndex = 11;
            this.cmbPressState.SelectedIndexChanged += new System.EventHandler(this.CmbPressedState_SelectedIndexChanged);
            // 
            // pnlGamePadIndex
            // 
            this.pnlGamePadIndex.Controls.Add(this.nudGamePadIndex);
            this.pnlGamePadIndex.Controls.Add(this.lblInfoIndex);
            this.pnlGamePadIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGamePadIndex.Location = new System.Drawing.Point(5, 5);
            this.pnlGamePadIndex.Name = "pnlGamePadIndex";
            this.pnlGamePadIndex.Size = new System.Drawing.Size(366, 20);
            this.pnlGamePadIndex.TabIndex = 12;
            // 
            // nudGamePadIndex
            // 
            this.nudGamePadIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudGamePadIndex.Location = new System.Drawing.Point(69, 0);
            this.nudGamePadIndex.Name = "nudGamePadIndex";
            this.nudGamePadIndex.Size = new System.Drawing.Size(297, 20);
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
            // pnlButton
            // 
            this.pnlButton.Controls.Add(this.txtButtonBind);
            this.pnlButton.Controls.Add(this.cmbPressState);
            this.pnlButton.Controls.Add(this.lblInfo);
            this.pnlButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlButton.Location = new System.Drawing.Point(5, 25);
            this.pnlButton.Name = "pnlButton";
            this.pnlButton.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.pnlButton.Size = new System.Drawing.Size(366, 26);
            this.pnlButton.TabIndex = 14;
            // 
            // lblLastGamePadLabel
            // 
            this.lblLastGamePadLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLastGamePadLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastGamePadLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblLastGamePadLabel.Location = new System.Drawing.Point(5, 51);
            this.lblLastGamePadLabel.Name = "lblLastGamePadLabel";
            this.lblLastGamePadLabel.Size = new System.Drawing.Size(366, 15);
            this.lblLastGamePadLabel.TabIndex = 15;
            this.lblLastGamePadLabel.Text = "Last GamePad used was #: <none>";
            // 
            // GamePadButtonTriggerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.lblLastGamePadLabel);
            this.Controls.Add(this.pnlButton);
            this.Controls.Add(this.pnlGamePadIndex);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "GamePadButtonTriggerControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(376, 71);
            this.pnlGamePadIndex.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudGamePadIndex)).EndInit();
            this.pnlButton.ResumeLayout(false);
            this.pnlButton.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtButtonBind;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ComboBox cmbPressState;
        private System.Windows.Forms.Panel pnlGamePadIndex;
        private System.Windows.Forms.NumericUpDown nudGamePadIndex;
        private System.Windows.Forms.Label lblInfoIndex;
        private System.Windows.Forms.Panel pnlButton;
        private System.Windows.Forms.Label lblLastGamePadLabel;
    }
}
