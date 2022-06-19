namespace KeyToJoy
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tmrAxisTimeout = new System.Windows.Forms.Timer(this.components);
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.pnlActionManagement = new System.Windows.Forms.Panel();
            this.btnAddAction = new System.Windows.Forms.Button();
            this.dgvMappings = new System.Windows.Forms.DataGridView();
            this.colContext = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colControl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrigger = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlPresetName = new System.Windows.Forms.Panel();
            this.cmbPreset = new System.Windows.Forms.ComboBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.lblPreset = new System.Windows.Forms.Label();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.btnOpenTest = new System.Windows.Forms.Button();
            this.lblPresetInfo = new System.Windows.Forms.Label();
            this.dbgLabel = new System.Windows.Forms.Label();
            this.pctController = new System.Windows.Forms.PictureBox();
            this.btnAbout = new System.Windows.Forms.Button();
            this.lblPresetName = new System.Windows.Forms.Label();
            this.txtPresetName = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.pnlActionManagement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMappings)).BeginInit();
            this.pnlPresetName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctController)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrAxisTimeout
            // 
            this.tmrAxisTimeout.Interval = 250;
            this.tmrAxisTimeout.Tick += new System.EventHandler(this.tmrAxisTimeout_Tick);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.pnlActionManagement);
            this.splitContainer.Panel1.Controls.Add(this.dgvMappings);
            this.splitContainer.Panel1.Controls.Add(this.panel1);
            this.splitContainer.Panel1.Controls.Add(this.pnlPresetName);
            this.splitContainer.Panel1.Controls.Add(this.chkEnabled);
            this.splitContainer.Panel1.Controls.Add(this.btnOpenTest);
            this.splitContainer.Panel1.Controls.Add(this.lblPresetInfo);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.splitContainer.Panel2.Controls.Add(this.dbgLabel);
            this.splitContainer.Panel2.Controls.Add(this.pctController);
            this.splitContainer.Size = new System.Drawing.Size(880, 557);
            this.splitContainer.SplitterDistance = 533;
            this.splitContainer.TabIndex = 56;
            // 
            // pnlActionManagement
            // 
            this.pnlActionManagement.Controls.Add(this.btnAddAction);
            this.pnlActionManagement.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActionManagement.Location = new System.Drawing.Point(0, 431);
            this.pnlActionManagement.Name = "pnlActionManagement";
            this.pnlActionManagement.Size = new System.Drawing.Size(533, 34);
            this.pnlActionManagement.TabIndex = 0;
            // 
            // btnAddAction
            // 
            this.btnAddAction.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAddAction.Location = new System.Drawing.Point(451, 0);
            this.btnAddAction.Name = "btnAddAction";
            this.btnAddAction.Size = new System.Drawing.Size(82, 34);
            this.btnAddAction.TabIndex = 0;
            this.btnAddAction.Text = "Add Action";
            this.btnAddAction.UseVisualStyleBackColor = true;
            this.btnAddAction.Click += new System.EventHandler(this.btnAddAction_Click);
            // 
            // dgvBinds
            // 
            this.dgvMappings.AllowUserToAddRows = false;
            this.dgvMappings.AllowUserToDeleteRows = false;
            this.dgvMappings.AllowUserToResizeRows = false;
            this.dgvMappings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMappings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colContext,
            this.colControl,
            this.colTrigger});
            this.dgvMappings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMappings.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvMappings.Location = new System.Drawing.Point(0, 100);
            this.dgvMappings.Name = "dgvBinds";
            this.dgvMappings.ReadOnly = true;
            this.dgvMappings.RowHeadersVisible = false;
            this.dgvMappings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMappings.Size = new System.Drawing.Size(533, 365);
            this.dgvMappings.TabIndex = 88;
            this.dgvMappings.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvMappings_CellFormatting);
            this.dgvMappings.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvMappings_CellMouseDoubleClick);
            this.dgvMappings.SelectionChanged += new System.EventHandler(this.DgvMappings_SelectionChanged);
            // 
            // colContext
            // 
            this.colContext.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colContext.HeaderText = "Context";
            this.colContext.Name = "colContext";
            this.colContext.ReadOnly = true;
            this.colContext.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colContext.Width = 49;
            // 
            // colControl
            // 
            this.colControl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colControl.HeaderText = "Action";
            this.colControl.Name = "colControl";
            this.colControl.ReadOnly = true;
            this.colControl.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colControl.Width = 43;
            // 
            // colTrigger
            // 
            this.colTrigger.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTrigger.HeaderText = "Trigger";
            this.colTrigger.Name = "colTrigger";
            this.colTrigger.ReadOnly = true;
            this.colTrigger.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // pnlPresetName
            // 
            this.pnlPresetName.Controls.Add(this.cmbPreset);
            this.pnlPresetName.Controls.Add(this.btnCreate);
            this.pnlPresetName.Controls.Add(this.lblPreset);
            this.pnlPresetName.Controls.Add(this.btnOpenFolder);
            this.pnlPresetName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPresetName.Location = new System.Drawing.Point(0, 40);
            this.pnlPresetName.Name = "pnlPresetName";
            this.pnlPresetName.Padding = new System.Windows.Forms.Padding(5);
            this.pnlPresetName.Size = new System.Drawing.Size(533, 30);
            this.pnlPresetName.TabIndex = 80;
            // 
            // cmbPreset
            // 
            this.cmbPreset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPreset.FormattingEnabled = true;
            this.cmbPreset.Location = new System.Drawing.Point(88, 5);
            this.cmbPreset.Name = "cmbPreset";
            this.cmbPreset.Size = new System.Drawing.Size(279, 21);
            this.cmbPreset.TabIndex = 87;
            this.cmbPreset.SelectedIndexChanged += new System.EventHandler(this.CmbPreset_SelectedIndexChanged);
            // 
            // btnCreate
            // 
            this.btnCreate.AutoSize = true;
            this.btnCreate.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCreate.Location = new System.Drawing.Point(367, 5);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(48, 20);
            this.btnCreate.TabIndex = 88;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
            // 
            // lblPreset
            // 
            this.lblPreset.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPreset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreset.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblPreset.Location = new System.Drawing.Point(5, 5);
            this.lblPreset.Name = "lblPreset";
            this.lblPreset.Size = new System.Drawing.Size(83, 20);
            this.lblPreset.TabIndex = 86;
            this.lblPreset.Text = "Bindings Preset:";
            this.lblPreset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.AutoSize = true;
            this.btnOpenFolder.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOpenFolder.Location = new System.Drawing.Point(415, 5);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(113, 20);
            this.btnOpenFolder.TabIndex = 89;
            this.btnOpenFolder.Text = "Open Presets Folder";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // chkEnabled
            // 
            this.chkEnabled.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chkEnabled.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkEnabled.Location = new System.Drawing.Point(0, 465);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Padding = new System.Windows.Forms.Padding(5);
            this.chkEnabled.Size = new System.Drawing.Size(533, 44);
            this.chkEnabled.TabIndex = 81;
            this.chkEnabled.Text = "Pretend keyboard and mouse input is game controller (using the key mappings confi" +
    "gured above)";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.ChkEnabled_CheckedChanged);
            // 
            // btnOpenTest
            // 
            this.btnOpenTest.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnOpenTest.Location = new System.Drawing.Point(0, 509);
            this.btnOpenTest.Name = "btnOpenTest";
            this.btnOpenTest.Size = new System.Drawing.Size(533, 48);
            this.btnOpenTest.TabIndex = 79;
            this.btnOpenTest.Text = "Test mapping using \'devicetests.com\'";
            this.btnOpenTest.UseVisualStyleBackColor = true;
            this.btnOpenTest.Click += new System.EventHandler(this.BtnOpenTest_Click);
            // 
            // lblPresetInfo
            // 
            this.lblPresetInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPresetInfo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblPresetInfo.Location = new System.Drawing.Point(0, 0);
            this.lblPresetInfo.Margin = new System.Windows.Forms.Padding(3);
            this.lblPresetInfo.Name = "lblPresetInfo";
            this.lblPresetInfo.Padding = new System.Windows.Forms.Padding(5);
            this.lblPresetInfo.Size = new System.Drawing.Size(533, 40);
            this.lblPresetInfo.TabIndex = 83;
            this.lblPresetInfo.Text = "Select a preset or type the name for a custom preset and click \'Create\'. Presets " +
    "are saved to the KeyToJoy folder in your documents automatically.";
            // 
            // dbgLabel
            // 
            this.dbgLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dbgLabel.AutoSize = true;
            this.dbgLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.dbgLabel.Location = new System.Drawing.Point(62, 431);
            this.dbgLabel.Name = "dbgLabel";
            this.dbgLabel.Size = new System.Drawing.Size(0, 13);
            this.dbgLabel.TabIndex = 78;
            // 
            // pctController
            // 
            this.pctController.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pctController.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pctController.ErrorImage = null;
            this.pctController.Image = global::KeyToJoy.Properties.Resources.XboxSeriesX_Diagram;
            this.pctController.InitialImage = null;
            this.pctController.Location = new System.Drawing.Point(0, 0);
            this.pctController.Name = "pctController";
            this.pctController.Size = new System.Drawing.Size(343, 557);
            this.pctController.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pctController.TabIndex = 79;
            this.pctController.TabStop = false;
            // 
            // btnAbout
            // 
            this.btnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbout.Location = new System.Drawing.Point(790, 530);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(90, 27);
            this.btnAbout.TabIndex = 80;
            this.btnAbout.Text = "Credits";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.BtnAbout_Click);
            // 
            // lblPresetName
            // 
            this.lblPresetName.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPresetName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPresetName.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblPresetName.Location = new System.Drawing.Point(5, 5);
            this.lblPresetName.Name = "lblPresetName";
            this.lblPresetName.Size = new System.Drawing.Size(77, 20);
            this.lblPresetName.TabIndex = 88;
            this.lblPresetName.Text = "Preset Name:";
            this.lblPresetName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPresetName
            // 
            this.txtPresetName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPresetName.Location = new System.Drawing.Point(82, 5);
            this.txtPresetName.Name = "txtPresetName";
            this.txtPresetName.Size = new System.Drawing.Size(446, 20);
            this.txtPresetName.TabIndex = 85;
            this.txtPresetName.TextChanged += new System.EventHandler(this.TxtPresetName_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtPresetName);
            this.panel1.Controls.Add(this.lblPresetName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 70);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(533, 30);
            this.panel1.TabIndex = 82;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(880, 557);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.splitContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "KeyToJoy - Prototype";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.pnlActionManagement.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMappings)).EndInit();
            this.pnlPresetName.ResumeLayout(false);
            this.pnlPresetName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctController)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmrAxisTimeout;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.Label dbgLabel;
        private System.Windows.Forms.PictureBox pctController;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Panel pnlPresetName;
        private System.Windows.Forms.DataGridView dgvMappings;
        private System.Windows.Forms.ComboBox cmbPreset;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label lblPreset;
        private System.Windows.Forms.Button btnOpenTest;
        private System.Windows.Forms.Label lblPresetInfo;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContext;
        private System.Windows.Forms.DataGridViewTextBoxColumn colControl;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrigger;
        private System.Windows.Forms.Panel pnlActionManagement;
        private System.Windows.Forms.Button btnAddAction;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtPresetName;
        private System.Windows.Forms.Label lblPresetName;
    }
}

