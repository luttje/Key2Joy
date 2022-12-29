namespace Key2Joy.Gui.Mapping
{
    partial class GamePadActionControl
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
            this.cmbGamePad = new System.Windows.Forms.ComboBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.cmbPressState = new System.Windows.Forms.ComboBox();
            this.cmbGamePadIndex = new System.Windows.Forms.ComboBox();
            this.lblInfoButton = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbGamePad
            // 
            this.cmbGamePad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbGamePad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGamePad.FormattingEnabled = true;
            this.cmbGamePad.Location = new System.Drawing.Point(163, 5);
            this.cmbGamePad.Name = "cmbGamePad";
            this.cmbGamePad.Size = new System.Drawing.Size(165, 21);
            this.cmbGamePad.TabIndex = 9;
            this.cmbGamePad.SelectedIndexChanged += new System.EventHandler(this.cmbGamePad_SelectedIndexChanged);
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfo.Location = new System.Drawing.Point(5, 5);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(65, 18);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.Text = "GamePad #";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPressState
            // 
            this.cmbPressState.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmbPressState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPressState.FormattingEnabled = true;
            this.cmbPressState.Location = new System.Drawing.Point(328, 5);
            this.cmbPressState.Name = "cmbPressState";
            this.cmbPressState.Size = new System.Drawing.Size(89, 21);
            this.cmbPressState.TabIndex = 12;
            this.cmbPressState.SelectedIndexChanged += new System.EventHandler(this.cmbPressState_SelectedIndexChanged);
            // 
            // cmbGamePadIndex
            // 
            this.cmbGamePadIndex.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbGamePadIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGamePadIndex.FormattingEnabled = true;
            this.cmbGamePadIndex.Location = new System.Drawing.Point(70, 5);
            this.cmbGamePadIndex.Name = "cmbGamePadIndex";
            this.cmbGamePadIndex.Size = new System.Drawing.Size(51, 21);
            this.cmbGamePadIndex.TabIndex = 13;
            this.cmbGamePadIndex.SelectedIndexChanged += new System.EventHandler(this.cmbGamePadIndex_SelectedIndexChanged);
            // 
            // lblInfoButton
            // 
            this.lblInfoButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfoButton.Location = new System.Drawing.Point(121, 5);
            this.lblInfoButton.Name = "lblInfoButton";
            this.lblInfoButton.Size = new System.Drawing.Size(42, 18);
            this.lblInfoButton.TabIndex = 14;
            this.lblInfoButton.Text = "Button:";
            this.lblInfoButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GamePadActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbGamePad);
            this.Controls.Add(this.lblInfoButton);
            this.Controls.Add(this.cmbPressState);
            this.Controls.Add(this.cmbGamePadIndex);
            this.Controls.Add(this.lblInfo);
            this.Name = "GamePadActionControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(422, 28);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbGamePad;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ComboBox cmbPressState;
        private System.Windows.Forms.ComboBox cmbGamePadIndex;
        private System.Windows.Forms.Label lblInfoButton;
    }
}
