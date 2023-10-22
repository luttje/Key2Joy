namespace Key2Joy.Gui
{
    partial class ConfigForm
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
            this.pnlConfigurations = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlSave = new System.Windows.Forms.Panel();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlConfigurations
            // 
            this.pnlConfigurations.AutoSize = true;
            this.pnlConfigurations.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlConfigurations.Location = new System.Drawing.Point(0, 0);
            this.pnlConfigurations.Name = "pnlConfigurations";
            this.pnlConfigurations.Size = new System.Drawing.Size(453, 0);
            this.pnlConfigurations.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSave.Location = new System.Drawing.Point(0, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(453, 35);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // pnlSave
            // 
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 75);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Size = new System.Drawing.Size(453, 41);
            this.pnlSave.TabIndex = 1;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(453, 116);
            this.Controls.Add(this.pnlConfigurations);
            this.Controls.Add(this.pnlSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(469, 600);
            this.Name = "ConfigForm";
            this.Text = "Key2Joy User Configurations";
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlConfigurations;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlSave;
    }
}