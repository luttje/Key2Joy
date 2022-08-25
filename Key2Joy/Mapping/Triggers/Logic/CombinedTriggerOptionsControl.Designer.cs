namespace Key2Joy.Mapping
{
    partial class CombinedTriggerOptionsControl
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
            this.grpTriggers = new System.Windows.Forms.GroupBox();
            this.btnAddTrigger = new System.Windows.Forms.Button();
            this.pnlTriggers = new System.Windows.Forms.Panel();
            this.grpTriggers.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpTriggers
            // 
            this.grpTriggers.AutoSize = true;
            this.grpTriggers.Controls.Add(this.btnAddTrigger);
            this.grpTriggers.Controls.Add(this.pnlTriggers);
            this.grpTriggers.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTriggers.Location = new System.Drawing.Point(0, 0);
            this.grpTriggers.Name = "grpTriggers";
            this.grpTriggers.Size = new System.Drawing.Size(347, 46);
            this.grpTriggers.TabIndex = 1;
            this.grpTriggers.TabStop = false;
            this.grpTriggers.Text = "Triggers";
            // 
            // btnAddTrigger
            // 
            this.btnAddTrigger.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddTrigger.Location = new System.Drawing.Point(3, 16);
            this.btnAddTrigger.Name = "btnAddTrigger";
            this.btnAddTrigger.Size = new System.Drawing.Size(341, 27);
            this.btnAddTrigger.TabIndex = 1;
            this.btnAddTrigger.Text = "Add Trigger";
            this.btnAddTrigger.UseVisualStyleBackColor = true;
            this.btnAddTrigger.Click += new System.EventHandler(this.btnAddTrigger_Click);
            // 
            // pnlTriggers
            // 
            this.pnlTriggers.AutoSize = true;
            this.pnlTriggers.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTriggers.Location = new System.Drawing.Point(3, 16);
            this.pnlTriggers.Name = "pnlTriggers";
            this.pnlTriggers.Size = new System.Drawing.Size(341, 0);
            this.pnlTriggers.TabIndex = 2;
            // 
            // CombinedTriggerOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.grpTriggers);
            this.Name = "CombinedTriggerOptionsControl";
            this.Size = new System.Drawing.Size(347, 46);
            this.grpTriggers.ResumeLayout(false);
            this.grpTriggers.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox grpTriggers;
        private System.Windows.Forms.Panel pnlTriggers;
        private System.Windows.Forms.Button btnAddTrigger;
    }
}
