namespace Key2Joy.Gui.Mapping
{
    partial class SequenceActionControl
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
            this.lblInfo = new System.Windows.Forms.Label();
            this.lstActions = new System.Windows.Forms.ListBox();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnRemove = new System.Windows.Forms.Button();
            this.pnlPadding = new System.Windows.Forms.Panel();
            this.grpSequenceActionOptions = new System.Windows.Forms.GroupBox();
            this.actionControl = new Key2Joy.Gui.Mapping.ActionControl();
            this.pnlActionOptions = new System.Windows.Forms.Panel();
            this.pnlPadding2 = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.pnlActions.SuspendLayout();
            this.pnlPadding.SuspendLayout();
            this.grpSequenceActionOptions.SuspendLayout();
            this.pnlPadding2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInfo.Location = new System.Drawing.Point(5, 5);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(339, 22);
            this.lblInfo.TabIndex = 13;
            this.lblInfo.Text = "Add a sequence of actions to perform:";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lstActions
            // 
            this.lstActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.lstActions.FormattingEnabled = true;
            this.lstActions.Location = new System.Drawing.Point(5, 27);
            this.lstActions.Name = "lstActions";
            this.lstActions.Size = new System.Drawing.Size(339, 82);
            this.lstActions.TabIndex = 14;
            this.lstActions.SelectedIndexChanged += new System.EventHandler(this.lstActions_SelectedIndexChanged);
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.btnRemove);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlActions.Location = new System.Drawing.Point(5, 109);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(339, 29);
            this.pnlActions.TabIndex = 17;
            // 
            // btnRemove
            // 
            this.btnRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(0, 0);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(339, 29);
            this.btnRemove.TabIndex = 18;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // pnlPadding
            // 
            this.pnlPadding.AutoSize = true;
            this.pnlPadding.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlPadding.Controls.Add(this.grpSequenceActionOptions);
            this.pnlPadding.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPadding.Location = new System.Drawing.Point(5, 138);
            this.pnlPadding.Name = "pnlPadding";
            this.pnlPadding.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.pnlPadding.Size = new System.Drawing.Size(339, 61);
            this.pnlPadding.TabIndex = 18;
            // 
            // grpSequenceActionOptions
            // 
            this.grpSequenceActionOptions.AutoSize = true;
            this.grpSequenceActionOptions.Controls.Add(this.actionControl);
            this.grpSequenceActionOptions.Controls.Add(this.pnlActionOptions);
            this.grpSequenceActionOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSequenceActionOptions.Location = new System.Drawing.Point(0, 5);
            this.grpSequenceActionOptions.Name = "grpSequenceActionOptions";
            this.grpSequenceActionOptions.Size = new System.Drawing.Size(339, 51);
            this.grpSequenceActionOptions.TabIndex = 16;
            this.grpSequenceActionOptions.TabStop = false;
            this.grpSequenceActionOptions.Text = "Action Options";
            // 
            // actionControl
            // 
            this.actionControl.AutoSize = true;
            this.actionControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.actionControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.actionControl.IsTopLevel = false;
            this.actionControl.Location = new System.Drawing.Point(3, 16);
            this.actionControl.MinimumSize = new System.Drawing.Size(300, 32);
            this.actionControl.Name = "actionControl";
            this.actionControl.Padding = new System.Windows.Forms.Padding(5);
            this.actionControl.Size = new System.Drawing.Size(333, 32);
            this.actionControl.TabIndex = 1;
            this.actionControl.ActionChanged += new System.Action<Key2Joy.Mapping.BaseAction>(this.actionControl_ActionChanged);
            // 
            // pnlActionOptions
            // 
            this.pnlActionOptions.AutoSize = true;
            this.pnlActionOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlActionOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlActionOptions.Location = new System.Drawing.Point(3, 16);
            this.pnlActionOptions.Name = "pnlActionOptions";
            this.pnlActionOptions.Size = new System.Drawing.Size(333, 0);
            this.pnlActionOptions.TabIndex = 0;
            // 
            // pnlPadding2
            // 
            this.pnlPadding2.Controls.Add(this.btnAdd);
            this.pnlPadding2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPadding2.Location = new System.Drawing.Point(5, 199);
            this.pnlPadding2.Name = "pnlPadding2";
            this.pnlPadding2.Size = new System.Drawing.Size(339, 29);
            this.pnlPadding2.TabIndex = 19;
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(339, 29);
            this.btnAdd.TabIndex = 17;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // SequenceActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.pnlPadding2);
            this.Controls.Add(this.pnlPadding);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.lstActions);
            this.Controls.Add(this.lblInfo);
            this.MinimumSize = new System.Drawing.Size(256, 64);
            this.Name = "SequenceActionControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(349, 233);
            this.pnlActions.ResumeLayout(false);
            this.pnlPadding.ResumeLayout(false);
            this.pnlPadding.PerformLayout();
            this.grpSequenceActionOptions.ResumeLayout(false);
            this.grpSequenceActionOptions.PerformLayout();
            this.pnlPadding2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ListBox lstActions;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Panel pnlPadding;
        private System.Windows.Forms.GroupBox grpSequenceActionOptions;
        private System.Windows.Forms.Panel pnlActionOptions;
        private ActionControl actionControl;
        private System.Windows.Forms.Panel pnlPadding2;
        private System.Windows.Forms.Button btnAdd;
    }
}
