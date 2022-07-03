namespace Key2Joy
{
    partial class MappingForm
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
            this.pnlAction = new System.Windows.Forms.Panel();
            this.grpAction = new System.Windows.Forms.GroupBox();
            this.btnSaveMapping = new System.Windows.Forms.Button();
            this.grpTrigger = new System.Windows.Forms.GroupBox();
            this.pnlTrigger = new System.Windows.Forms.Panel();
            this.actionControl = new Key2Joy.Mapping.ActionControl();
            this.triggerControl = new Key2Joy.Mapping.TriggerControl();
            this.pnlAction.SuspendLayout();
            this.grpAction.SuspendLayout();
            this.grpTrigger.SuspendLayout();
            this.pnlTrigger.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlAction
            // 
            this.pnlAction.AutoSize = true;
            this.pnlAction.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlAction.Controls.Add(this.grpAction);
            this.pnlAction.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAction.Location = new System.Drawing.Point(5, 69);
            this.pnlAction.Name = "pnlAction";
            this.pnlAction.Padding = new System.Windows.Forms.Padding(5, 5, 5, 10);
            this.pnlAction.Size = new System.Drawing.Size(486, 70);
            this.pnlAction.TabIndex = 90;
            // 
            // grpAction
            // 
            this.grpAction.AutoSize = true;
            this.grpAction.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpAction.Controls.Add(this.actionControl);
            this.grpAction.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpAction.Location = new System.Drawing.Point(5, 5);
            this.grpAction.Name = "grpAction";
            this.grpAction.Padding = new System.Windows.Forms.Padding(5);
            this.grpAction.Size = new System.Drawing.Size(476, 55);
            this.grpAction.TabIndex = 88;
            this.grpAction.TabStop = false;
            this.grpAction.Text = "Actions that start at the trigger";
            // 
            // btnSaveMapping
            // 
            this.btnSaveMapping.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSaveMapping.Location = new System.Drawing.Point(5, 139);
            this.btnSaveMapping.Name = "btnSaveMapping";
            this.btnSaveMapping.Size = new System.Drawing.Size(486, 44);
            this.btnSaveMapping.TabIndex = 91;
            this.btnSaveMapping.Text = "Save Mapping";
            this.btnSaveMapping.UseVisualStyleBackColor = true;
            this.btnSaveMapping.Click += new System.EventHandler(this.btnSaveMapping_Click);
            // 
            // grpTrigger
            // 
            this.grpTrigger.AutoSize = true;
            this.grpTrigger.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpTrigger.Controls.Add(this.triggerControl);
            this.grpTrigger.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTrigger.Location = new System.Drawing.Point(5, 5);
            this.grpTrigger.Name = "grpTrigger";
            this.grpTrigger.Padding = new System.Windows.Forms.Padding(5);
            this.grpTrigger.Size = new System.Drawing.Size(476, 54);
            this.grpTrigger.TabIndex = 86;
            this.grpTrigger.TabStop = false;
            this.grpTrigger.Text = "Trigger that starts the action(s)";
            // 
            // pnlTrigger
            // 
            this.pnlTrigger.AutoSize = true;
            this.pnlTrigger.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlTrigger.Controls.Add(this.grpTrigger);
            this.pnlTrigger.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTrigger.Location = new System.Drawing.Point(5, 5);
            this.pnlTrigger.Name = "pnlTrigger";
            this.pnlTrigger.Padding = new System.Windows.Forms.Padding(5);
            this.pnlTrigger.Size = new System.Drawing.Size(486, 64);
            this.pnlTrigger.TabIndex = 89;
            // 
            // actionControl
            // 
            this.actionControl.AutoSize = true;
            this.actionControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.actionControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.actionControl.IsTopLevel = false;
            this.actionControl.Location = new System.Drawing.Point(5, 18);
            this.actionControl.MinimumSize = new System.Drawing.Size(300, 32);
            this.actionControl.Name = "actionControl";
            this.actionControl.Padding = new System.Windows.Forms.Padding(5);
            this.actionControl.Size = new System.Drawing.Size(466, 32);
            this.actionControl.TabIndex = 0;
            // 
            // triggerControl
            // 
            this.triggerControl.AutoSize = true;
            this.triggerControl.BackColor = System.Drawing.SystemColors.Control;
            this.triggerControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.triggerControl.Location = new System.Drawing.Point(5, 18);
            this.triggerControl.Name = "triggerControl";
            this.triggerControl.Padding = new System.Windows.Forms.Padding(5);
            this.triggerControl.Size = new System.Drawing.Size(466, 31);
            this.triggerControl.TabIndex = 0;
            // 
            // MappingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(496, 188);
            this.Controls.Add(this.btnSaveMapping);
            this.Controls.Add(this.pnlAction);
            this.Controls.Add(this.pnlTrigger);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(1024, 1024);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(512, 39);
            this.Name = "MappingForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Map triggers to actions";
            this.pnlAction.ResumeLayout(false);
            this.pnlAction.PerformLayout();
            this.grpAction.ResumeLayout(false);
            this.grpAction.PerformLayout();
            this.grpTrigger.ResumeLayout(false);
            this.grpTrigger.PerformLayout();
            this.pnlTrigger.ResumeLayout(false);
            this.pnlTrigger.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel pnlAction;
        private System.Windows.Forms.Button btnSaveMapping;
        private System.Windows.Forms.GroupBox grpTrigger;
        private System.Windows.Forms.Panel pnlTrigger;
        private System.Windows.Forms.GroupBox grpAction;
        private Mapping.ActionControl actionControl;
        private Mapping.TriggerControl triggerControl;
    }
}