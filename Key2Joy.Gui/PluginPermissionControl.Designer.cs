namespace Key2Joy.Gui
{
    partial class PluginPermissionControl
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
            this.lblPermission = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPermission
            // 
            this.lblPermission.AutoSize = true;
            this.lblPermission.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPermission.Location = new System.Drawing.Point(27, 10);
            this.lblPermission.Name = "lblPermission";
            this.lblPermission.Size = new System.Drawing.Size(57, 13);
            this.lblPermission.TabIndex = 0;
            this.lblPermission.Text = "Permission";
            this.lblPermission.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::Key2Joy.Gui.Properties.Resources.tick;
            this.pictureBox1.Location = new System.Drawing.Point(0, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(27, 13);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // PluginPermissionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.lblPermission);
            this.Controls.Add(this.pictureBox1);
            this.Name = "PluginPermissionControl";
            this.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.Size = new System.Drawing.Size(172, 33);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPermission;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
