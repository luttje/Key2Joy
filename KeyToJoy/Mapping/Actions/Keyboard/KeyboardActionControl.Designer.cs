namespace KeyToJoy.Mapping
{
    partial class KeyboardActionControl
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
            this.cmbKeyboard = new System.Windows.Forms.ComboBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.cmbPressState = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cmbKeyboard
            // 
            this.cmbKeyboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbKeyboard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeyboard.FormattingEnabled = true;
            this.cmbKeyboard.Location = new System.Drawing.Point(100, 5);
            this.cmbKeyboard.Name = "cmbKeyboard";
            this.cmbKeyboard.Size = new System.Drawing.Size(228, 21);
            this.cmbKeyboard.TabIndex = 9;
            this.cmbKeyboard.SelectedIndexChanged += new System.EventHandler(this.cmbKeyboard_SelectedIndexChanged);
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfo.Location = new System.Drawing.Point(5, 5);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(95, 18);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.Text = "Keyboard Button:";
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
            // KeyboardActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbKeyboard);
            this.Controls.Add(this.cmbPressState);
            this.Controls.Add(this.lblInfo);
            this.Name = "KeyboardActionControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(422, 28);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbKeyboard;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ComboBox cmbPressState;
    }
}
