namespace Key2Joy.Mapping
{
    partial class CombinedTriggerControl
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
            this.triggerControl = new Key2Joy.Mapping.TriggerControl();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnRemove = new System.Windows.Forms.Button();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // triggerControl
            // 
            this.triggerControl.AutoSize = true;
            this.triggerControl.BackColor = System.Drawing.SystemColors.Control;
            this.triggerControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.triggerControl.IsTopLevel = false;
            this.triggerControl.Location = new System.Drawing.Point(0, 0);
            this.triggerControl.Name = "triggerControl";
            this.triggerControl.Padding = new System.Windows.Forms.Padding(5);
            this.triggerControl.Size = new System.Drawing.Size(331, 31);
            this.triggerControl.TabIndex = 2;
            this.triggerControl.TriggerChanged += new System.EventHandler<Key2Joy.Mapping.TriggerChangedEventArgs>(this.triggerControl_TriggerChanged);
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.btnRemove);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlActions.Location = new System.Drawing.Point(331, 0);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.pnlActions.Size = new System.Drawing.Size(82, 31);
            this.pnlActions.TabIndex = 3;
            // 
            // btnRemove
            // 
            this.btnRemove.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRemove.Location = new System.Drawing.Point(0, 4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(82, 23);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // CombinedTriggerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.triggerControl);
            this.Controls.Add(this.pnlActions);
            this.Name = "CombinedTriggerControl";
            this.Size = new System.Drawing.Size(413, 31);
            this.pnlActions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TriggerControl triggerControl;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnRemove;
    }
}
