namespace Key2Joy.Gui.Mapping
{
    partial class ActionControl
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
            this.pnlActionOptions = new System.Windows.Forms.Panel();
            this.cmbAction = new ImageComboBox();
            this.SuspendLayout();
            // 
            // pnlActionOptions
            // 
            this.pnlActionOptions.AutoSize = true;
            this.pnlActionOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlActionOptions.Location = new System.Drawing.Point(5, 26);
            this.pnlActionOptions.Name = "pnlActionOptions";
            this.pnlActionOptions.Size = new System.Drawing.Size(167, 0);
            this.pnlActionOptions.TabIndex = 3;
            // 
            // cmbAction
            // 
            this.cmbAction.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAction.FormattingEnabled = true;
            this.cmbAction.Location = new System.Drawing.Point(5, 5);
            this.cmbAction.Name = "cmbAction";
            this.cmbAction.Size = new System.Drawing.Size(167, 21);
            this.cmbAction.TabIndex = 2;
            this.cmbAction.SelectedIndexChanged += new System.EventHandler(this.CmbAction_SelectedIndexChanged);
            // 
            // ActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlActionOptions);
            this.Controls.Add(this.cmbAction);
            this.Name = "ActionControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(177, 32);
            this.Load += new System.EventHandler(this.ActionControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlActionOptions;
        private ImageComboBox cmbAction;
    }
}
