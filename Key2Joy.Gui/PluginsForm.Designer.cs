namespace Key2Joy.Gui
{
    partial class PluginsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginsForm));
            this.dgvPlugins = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlugins)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPlugins
            // 
            this.dgvPlugins.AllowUserToAddRows = false;
            this.dgvPlugins.AllowUserToDeleteRows = false;
            this.dgvPlugins.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPlugins.Location = new System.Drawing.Point(0, 0);
            this.dgvPlugins.Name = "dgvPlugins";
            this.dgvPlugins.ReadOnly = true;
            this.dgvPlugins.Size = new System.Drawing.Size(614, 361);
            this.dgvPlugins.TabIndex = 0;
            // 
            // PluginsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 361);
            this.Controls.Add(this.dgvPlugins);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(630, 400);
            this.Name = "PluginsForm";
            this.Text = "Manage Plugins";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlugins)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPlugins;
    }
}