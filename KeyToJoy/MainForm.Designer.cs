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
            this.timerAxisTimeout = new System.Windows.Forms.Timer(this.components);
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dgvBinds = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtPresetName = new System.Windows.Forms.TextBox();
            this.lblInfoName = new System.Windows.Forms.Label();
            this.pnlPreset = new System.Windows.Forms.Panel();
            this.lblPresetInfo = new System.Windows.Forms.Label();
            this.cmbPreset = new System.Windows.Forms.ComboBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.lblPreset = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.btnOpenTest = new System.Windows.Forms.Button();
            this.dbgLabel = new System.Windows.Forms.Label();
            this.pctController = new System.Windows.Forms.PictureBox();
            this.btnAbout = new System.Windows.Forms.Button();
            this.colControl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBind = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBinds)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlPreset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctController)).BeginInit();
            this.SuspendLayout();
            // 
            // timerAxisTimeout
            // 
            this.timerAxisTimeout.Interval = 250;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.dgvBinds);
            this.splitContainer.Panel1.Controls.Add(this.panel1);
            this.splitContainer.Panel1.Controls.Add(this.pnlPreset);
            this.splitContainer.Panel1.Controls.Add(this.chkEnabled);
            this.splitContainer.Panel1.Controls.Add(this.btnOpenTest);
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
            // dgvBinds
            // 
            this.dgvBinds.AllowUserToAddRows = false;
            this.dgvBinds.AllowUserToDeleteRows = false;
            this.dgvBinds.AllowUserToResizeRows = false;
            this.dgvBinds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBinds.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colControl,
            this.colBind});
            this.dgvBinds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBinds.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvBinds.Location = new System.Drawing.Point(0, 108);
            this.dgvBinds.Name = "dgvBinds";
            this.dgvBinds.ReadOnly = true;
            this.dgvBinds.RowHeadersVisible = false;
            this.dgvBinds.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBinds.Size = new System.Drawing.Size(533, 357);
            this.dgvBinds.TabIndex = 86;
            this.dgvBinds.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvBinds_CellFormatting);
            this.dgvBinds.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvBinds_CellMouseDoubleClick);
            this.dgvBinds.SelectionChanged += new System.EventHandler(this.DgvBinds_SelectionChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtPresetName);
            this.panel1.Controls.Add(this.lblInfoName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 78);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(533, 30);
            this.panel1.TabIndex = 80;
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
            // lblInfoName
            // 
            this.lblInfoName.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfoName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoName.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblInfoName.Location = new System.Drawing.Point(5, 5);
            this.lblInfoName.Name = "lblInfoName";
            this.lblInfoName.Size = new System.Drawing.Size(77, 20);
            this.lblInfoName.TabIndex = 88;
            this.lblInfoName.Text = "Preset Name:";
            this.lblInfoName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlPreset
            // 
            this.pnlPreset.Controls.Add(this.lblPresetInfo);
            this.pnlPreset.Controls.Add(this.cmbPreset);
            this.pnlPreset.Controls.Add(this.btnCreate);
            this.pnlPreset.Controls.Add(this.lblPreset);
            this.pnlPreset.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPreset.Location = new System.Drawing.Point(0, 0);
            this.pnlPreset.Name = "pnlPreset";
            this.pnlPreset.Padding = new System.Windows.Forms.Padding(5);
            this.pnlPreset.Size = new System.Drawing.Size(533, 78);
            this.pnlPreset.TabIndex = 80;
            // 
            // lblPresetInfo
            // 
            this.lblPresetInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPresetInfo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblPresetInfo.Location = new System.Drawing.Point(5, 5);
            this.lblPresetInfo.Name = "lblPresetInfo";
            this.lblPresetInfo.Size = new System.Drawing.Size(523, 43);
            this.lblPresetInfo.TabIndex = 80;
            this.lblPresetInfo.Text = "Select a preset or type the name for a custom preset and click \'Create\'. Presets " +
    "are saved to the KeyToJoy folder in your documents automatically.";
            // 
            // cmbPreset
            // 
            this.cmbPreset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPreset.FormattingEnabled = true;
            this.cmbPreset.Location = new System.Drawing.Point(88, 52);
            this.cmbPreset.Name = "cmbPreset";
            this.cmbPreset.Size = new System.Drawing.Size(392, 21);
            this.cmbPreset.TabIndex = 84;
            this.cmbPreset.SelectedIndexChanged += new System.EventHandler(this.CmbPreset_SelectedIndexChanged);
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.Location = new System.Drawing.Point(480, 52);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(48, 21);
            this.btnCreate.TabIndex = 80;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
            // 
            // lblPreset
            // 
            this.lblPreset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPreset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreset.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblPreset.Location = new System.Drawing.Point(5, 52);
            this.lblPreset.Name = "lblPreset";
            this.lblPreset.Size = new System.Drawing.Size(83, 21);
            this.lblPreset.TabIndex = 82;
            this.lblPreset.Text = "Bindings Preset:";
            this.lblPreset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.chkEnabled.Text = "Pretend keyboard and mouse input is game controller (using the binds configured a" +
    "bove)";
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
            this.btnOpenTest.Text = "Test input translation using \'devicetests.com\'";
            this.btnOpenTest.UseVisualStyleBackColor = true;
            this.btnOpenTest.Click += new System.EventHandler(this.BtnOpenTest_Click);
            // 
            // dbgLabel
            // 
            this.dbgLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dbgLabel.AutoSize = true;
            this.dbgLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.dbgLabel.Location = new System.Drawing.Point(61, 431);
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
            // colControl
            // 
            this.colControl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colControl.HeaderText = "Action";
            this.colControl.Name = "colControl";
            this.colControl.ReadOnly = true;
            this.colControl.Width = 62;
            // 
            // colBind
            // 
            this.colBind.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBind.HeaderText = "Bind";
            this.colBind.Name = "colBind";
            this.colBind.ReadOnly = true;
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvBinds)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlPreset.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctController)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timerAxisTimeout;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button btnOpenTest;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.Label dbgLabel;
        private System.Windows.Forms.PictureBox pctController;
        private System.Windows.Forms.Panel pnlPreset;
        private System.Windows.Forms.Label lblPreset;
        private System.Windows.Forms.ComboBox cmbPreset;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label lblPresetInfo;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.TextBox txtPresetName;
        private System.Windows.Forms.DataGridView dgvBinds;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblInfoName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colControl;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBind;
    }
}

