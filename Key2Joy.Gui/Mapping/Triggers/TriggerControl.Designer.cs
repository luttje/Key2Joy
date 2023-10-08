namespace Key2Joy.Gui.Mapping
{
    partial class TriggerControl
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
            this.pnlTriggerOptions = new System.Windows.Forms.Panel();
            this.cmbTrigger = new ImageComboBox();
            this.SuspendLayout();
            // 
            // pnlTriggerOptions
            // 
            this.pnlTriggerOptions.AutoSize = true;
            this.pnlTriggerOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTriggerOptions.Location = new System.Drawing.Point(5, 26);
            this.pnlTriggerOptions.Name = "pnlTriggerOptions";
            this.pnlTriggerOptions.Size = new System.Drawing.Size(167, 0);
            this.pnlTriggerOptions.TabIndex = 3;
            // 
            // cmbTrigger
            // 
            this.cmbTrigger.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbTrigger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTrigger.FormattingEnabled = true;
            this.cmbTrigger.Location = new System.Drawing.Point(5, 5);
            this.cmbTrigger.Name = "cmbTrigger";
            this.cmbTrigger.Size = new System.Drawing.Size(167, 21);
            this.cmbTrigger.TabIndex = 2;
            this.cmbTrigger.SelectedIndexChanged += new System.EventHandler(this.CmbTrigger_SelectedIndexChanged);
            // 
            // TriggerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.pnlTriggerOptions);
            this.Controls.Add(this.cmbTrigger);
            this.Name = "TriggerControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(177, 32);
            this.Load += new System.EventHandler(this.TriggerControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTriggerOptions;
        private ImageComboBox cmbTrigger;
    }
}
