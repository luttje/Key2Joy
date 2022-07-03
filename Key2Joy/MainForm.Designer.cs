namespace Key2Joy
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
            this.olvMappings = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnAction = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnTrigger = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pnlActionManagement = new System.Windows.Forms.Panel();
            this.btnAddAction = new System.Windows.Forms.Button();
            this.pnlPresetManagement = new System.Windows.Forms.Panel();
            this.txtPresetName = new System.Windows.Forms.TextBox();
            this.lblPresetName = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.menMainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newPresetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPresetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.savePresetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openPresetFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fillPresetWithToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allGamePadJoystickActionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pressAndReleaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.releaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allKeyboardActionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pressAndReleaseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pressToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.releaseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.testMappingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testGamePadJoystickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testKeyboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testMouseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.userConfigurationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportAProblemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewSourceCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ntfIndicator = new System.Windows.Forms.NotifyIcon(this.components);
            this.pnlMainMenu = new System.Windows.Forms.Panel();
            this.lblStatusInactive = new System.Windows.Forms.Label();
            this.lblStatusActive = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.olvMappings)).BeginInit();
            this.pnlActionManagement.SuspendLayout();
            this.pnlPresetManagement.SuspendLayout();
            this.menMainMenu.SuspendLayout();
            this.pnlMainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // olvMappings
            // 
            this.olvMappings.AllColumns.Add(this.olvColumnAction);
            this.olvMappings.AllColumns.Add(this.olvColumnTrigger);
            this.olvMappings.CellEditUseWholeCell = false;
            this.olvMappings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnAction,
            this.olvColumnTrigger});
            this.olvMappings.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvMappings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvMappings.FullRowSelect = true;
            this.olvMappings.HideSelection = false;
            this.olvMappings.Location = new System.Drawing.Point(0, 53);
            this.olvMappings.Name = "olvMappings";
            this.olvMappings.Size = new System.Drawing.Size(580, 370);
            this.olvMappings.TabIndex = 84;
            this.olvMappings.UseCellFormatEvents = true;
            this.olvMappings.UseCompatibleStateImageBehavior = false;
            this.olvMappings.UseHotItem = true;
            this.olvMappings.UseTranslucentHotItem = true;
            this.olvMappings.UseTranslucentSelection = true;
            this.olvMappings.View = System.Windows.Forms.View.Details;
            this.olvMappings.AboutToCreateGroups += new System.EventHandler<BrightIdeasSoftware.CreateGroupsEventArgs>(this.olvMappings_AboutToCreateGroups);
            this.olvMappings.CellClick += new System.EventHandler<BrightIdeasSoftware.CellClickEventArgs>(this.olvMappings_CellClick);
            this.olvMappings.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.olvMappings_CellRightClick);
            this.olvMappings.FormatCell += new System.EventHandler<BrightIdeasSoftware.FormatCellEventArgs>(this.olvMappings_FormatCell);
            this.olvMappings.KeyUp += new System.Windows.Forms.KeyEventHandler(this.olvMappings_KeyUp);
            // 
            // olvColumnAction
            // 
            this.olvColumnAction.AspectName = "Action";
            this.olvColumnAction.DisplayIndex = 1;
            this.olvColumnAction.Text = "Action";
            this.olvColumnAction.UseInitialLetterForGroup = true;
            // 
            // olvColumnTrigger
            // 
            this.olvColumnTrigger.AspectName = "Trigger";
            this.olvColumnTrigger.DisplayIndex = 0;
            this.olvColumnTrigger.Groupable = false;
            this.olvColumnTrigger.Text = "Trigger";
            // 
            // pnlActionManagement
            // 
            this.pnlActionManagement.Controls.Add(this.btnAddAction);
            this.pnlActionManagement.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActionManagement.Location = new System.Drawing.Point(0, 423);
            this.pnlActionManagement.Name = "pnlActionManagement";
            this.pnlActionManagement.Size = new System.Drawing.Size(580, 34);
            this.pnlActionManagement.TabIndex = 0;
            // 
            // btnAddAction
            // 
            this.btnAddAction.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAddAction.Location = new System.Drawing.Point(431, 0);
            this.btnAddAction.Name = "btnAddAction";
            this.btnAddAction.Size = new System.Drawing.Size(149, 34);
            this.btnAddAction.TabIndex = 0;
            this.btnAddAction.Text = "Create New Mapping";
            this.btnAddAction.UseVisualStyleBackColor = true;
            this.btnAddAction.Click += new System.EventHandler(this.btnAddAction_Click);
            // 
            // pnlPresetManagement
            // 
            this.pnlPresetManagement.Controls.Add(this.txtPresetName);
            this.pnlPresetManagement.Controls.Add(this.lblPresetName);
            this.pnlPresetManagement.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPresetManagement.Location = new System.Drawing.Point(0, 23);
            this.pnlPresetManagement.Name = "pnlPresetManagement";
            this.pnlPresetManagement.Padding = new System.Windows.Forms.Padding(5);
            this.pnlPresetManagement.Size = new System.Drawing.Size(580, 30);
            this.pnlPresetManagement.TabIndex = 82;
            // 
            // txtPresetName
            // 
            this.txtPresetName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPresetName.Location = new System.Drawing.Point(82, 5);
            this.txtPresetName.Name = "txtPresetName";
            this.txtPresetName.Size = new System.Drawing.Size(493, 20);
            this.txtPresetName.TabIndex = 85;
            this.txtPresetName.TextChanged += new System.EventHandler(this.TxtPresetName_TextChanged);
            // 
            // lblPresetName
            // 
            this.lblPresetName.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPresetName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPresetName.Location = new System.Drawing.Point(5, 5);
            this.lblPresetName.Name = "lblPresetName";
            this.lblPresetName.Size = new System.Drawing.Size(77, 20);
            this.lblPresetName.TabIndex = 88;
            this.lblPresetName.Text = "Preset Name:";
            this.lblPresetName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkEnabled.Location = new System.Drawing.Point(320, 0);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(59, 23);
            this.chkEnabled.TabIndex = 81;
            this.chkEnabled.Text = "Enable";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.ChkEnabled_CheckedChanged);
            // 
            // menMainMenu
            // 
            this.menMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menMainMenu.Location = new System.Drawing.Point(0, 0);
            this.menMainMenu.Name = "menMainMenu";
            this.menMainMenu.Size = new System.Drawing.Size(320, 24);
            this.menMainMenu.TabIndex = 81;
            this.menMainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newPresetToolStripMenuItem,
            this.loadPresetToolStripMenuItem,
            this.toolStripSeparator3,
            this.savePresetToolStripMenuItem,
            this.openPresetFolderToolStripMenuItem,
            this.toolStripSeparator2,
            this.closeToolStripMenuItem,
            this.exitProgramToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newPresetToolStripMenuItem
            // 
            this.newPresetToolStripMenuItem.Name = "newPresetToolStripMenuItem";
            this.newPresetToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.newPresetToolStripMenuItem.Text = "New Preset";
            this.newPresetToolStripMenuItem.Click += new System.EventHandler(this.newPresetToolStripMenuItem_Click);
            // 
            // loadPresetToolStripMenuItem
            // 
            this.loadPresetToolStripMenuItem.Name = "loadPresetToolStripMenuItem";
            this.loadPresetToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.loadPresetToolStripMenuItem.Text = "Load Preset";
            this.loadPresetToolStripMenuItem.Click += new System.EventHandler(this.loadPresetToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(201, 6);
            // 
            // savePresetToolStripMenuItem
            // 
            this.savePresetToolStripMenuItem.Name = "savePresetToolStripMenuItem";
            this.savePresetToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.savePresetToolStripMenuItem.Text = "Save Preset";
            this.savePresetToolStripMenuItem.Click += new System.EventHandler(this.savePresetToolStripMenuItem_Click);
            // 
            // openPresetFolderToolStripMenuItem
            // 
            this.openPresetFolderToolStripMenuItem.Name = "openPresetFolderToolStripMenuItem";
            this.openPresetFolderToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.openPresetFolderToolStripMenuItem.Text = "Open Preset Folder";
            this.openPresetFolderToolStripMenuItem.Click += new System.EventHandler(this.openPresetFolderToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(201, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.closeToolStripMenuItem.Text = "Close to notification tray";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // exitProgramToolStripMenuItem
            // 
            this.exitProgramToolStripMenuItem.Name = "exitProgramToolStripMenuItem";
            this.exitProgramToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.exitProgramToolStripMenuItem.Text = "Exit Program";
            this.exitProgramToolStripMenuItem.Click += new System.EventHandler(this.exitProgramToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fillPresetWithToolStripMenuItem,
            this.testMappingsToolStripMenuItem,
            this.toolStripSeparator4,
            this.userConfigurationsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // fillPresetWithToolStripMenuItem
            // 
            this.fillPresetWithToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allGamePadJoystickActionsToolStripMenuItem,
            this.allKeyboardActionsToolStripMenuItem});
            this.fillPresetWithToolStripMenuItem.Name = "fillPresetWithToolStripMenuItem";
            this.fillPresetWithToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.fillPresetWithToolStripMenuItem.Text = "Fill Preset With...";
            // 
            // allGamePadJoystickActionsToolStripMenuItem
            // 
            this.allGamePadJoystickActionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pressAndReleaseToolStripMenuItem,
            this.pressToolStripMenuItem,
            this.releaseToolStripMenuItem});
            this.allGamePadJoystickActionsToolStripMenuItem.Name = "allGamePadJoystickActionsToolStripMenuItem";
            this.allGamePadJoystickActionsToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.allGamePadJoystickActionsToolStripMenuItem.Text = "All GamePad/Joystick Actions";
            // 
            // pressAndReleaseToolStripMenuItem
            // 
            this.pressAndReleaseToolStripMenuItem.Name = "pressAndReleaseToolStripMenuItem";
            this.pressAndReleaseToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.pressAndReleaseToolStripMenuItem.Text = "PressAndRelease";
            this.pressAndReleaseToolStripMenuItem.Click += new System.EventHandler(this.gamePadPressAndReleaseToolStripMenuItem_Click);
            // 
            // pressToolStripMenuItem
            // 
            this.pressToolStripMenuItem.Name = "pressToolStripMenuItem";
            this.pressToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.pressToolStripMenuItem.Text = "Press";
            this.pressToolStripMenuItem.Click += new System.EventHandler(this.gamePadPressToolStripMenuItem_Click);
            // 
            // releaseToolStripMenuItem
            // 
            this.releaseToolStripMenuItem.Name = "releaseToolStripMenuItem";
            this.releaseToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.releaseToolStripMenuItem.Text = "Release";
            this.releaseToolStripMenuItem.Click += new System.EventHandler(this.gamePadReleaseToolStripMenuItem_Click);
            // 
            // allKeyboardActionsToolStripMenuItem
            // 
            this.allKeyboardActionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pressAndReleaseToolStripMenuItem1,
            this.pressToolStripMenuItem1,
            this.releaseToolStripMenuItem1});
            this.allKeyboardActionsToolStripMenuItem.Name = "allKeyboardActionsToolStripMenuItem";
            this.allKeyboardActionsToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.allKeyboardActionsToolStripMenuItem.Text = "All Keyboard Actions";
            // 
            // pressAndReleaseToolStripMenuItem1
            // 
            this.pressAndReleaseToolStripMenuItem1.Name = "pressAndReleaseToolStripMenuItem1";
            this.pressAndReleaseToolStripMenuItem1.Size = new System.Drawing.Size(162, 22);
            this.pressAndReleaseToolStripMenuItem1.Text = "PressAndRelease";
            this.pressAndReleaseToolStripMenuItem1.Click += new System.EventHandler(this.keyboardPressAndReleaseToolStripMenuItem);
            // 
            // pressToolStripMenuItem1
            // 
            this.pressToolStripMenuItem1.Name = "pressToolStripMenuItem1";
            this.pressToolStripMenuItem1.Size = new System.Drawing.Size(162, 22);
            this.pressToolStripMenuItem1.Text = "Press";
            this.pressToolStripMenuItem1.Click += new System.EventHandler(this.keyboardPressToolStripMenuItem);
            // 
            // releaseToolStripMenuItem1
            // 
            this.releaseToolStripMenuItem1.Name = "releaseToolStripMenuItem1";
            this.releaseToolStripMenuItem1.Size = new System.Drawing.Size(162, 22);
            this.releaseToolStripMenuItem1.Text = "Release";
            this.releaseToolStripMenuItem1.Click += new System.EventHandler(this.keyboardReleaseToolStripMenuItem);
            // 
            // testMappingsToolStripMenuItem
            // 
            this.testMappingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testGamePadJoystickToolStripMenuItem,
            this.testKeyboardToolStripMenuItem,
            this.testMouseToolStripMenuItem});
            this.testMappingsToolStripMenuItem.Name = "testMappingsToolStripMenuItem";
            this.testMappingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.testMappingsToolStripMenuItem.Text = "Test Mappings";
            // 
            // testGamePadJoystickToolStripMenuItem
            // 
            this.testGamePadJoystickToolStripMenuItem.Name = "testGamePadJoystickToolStripMenuItem";
            this.testGamePadJoystickToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.testGamePadJoystickToolStripMenuItem.Text = "Test GamePad / Joystick";
            this.testGamePadJoystickToolStripMenuItem.Click += new System.EventHandler(this.testGamePadJoystickToolStripMenuItem_Click);
            // 
            // testKeyboardToolStripMenuItem
            // 
            this.testKeyboardToolStripMenuItem.Name = "testKeyboardToolStripMenuItem";
            this.testKeyboardToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.testKeyboardToolStripMenuItem.Text = "Test Keyboard";
            this.testKeyboardToolStripMenuItem.Click += new System.EventHandler(this.testKeyboardToolStripMenuItem_Click);
            // 
            // testMouseToolStripMenuItem
            // 
            this.testMouseToolStripMenuItem.Name = "testMouseToolStripMenuItem";
            this.testMouseToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.testMouseToolStripMenuItem.Text = "Test Mouse";
            this.testMouseToolStripMenuItem.Click += new System.EventHandler(this.testMouseToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(177, 6);
            // 
            // userConfigurationsToolStripMenuItem
            // 
            this.userConfigurationsToolStripMenuItem.Image = global::Key2Joy.Properties.Resources.cog;
            this.userConfigurationsToolStripMenuItem.Name = "userConfigurationsToolStripMenuItem";
            this.userConfigurationsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.userConfigurationsToolStripMenuItem.Text = "User Configurations";
            this.userConfigurationsToolStripMenuItem.Click += new System.EventHandler(this.userConfigurationsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportAProblemToolStripMenuItem,
            this.viewSourceCodeToolStripMenuItem,
            this.toolStripSeparator1,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // reportAProblemToolStripMenuItem
            // 
            this.reportAProblemToolStripMenuItem.Name = "reportAProblemToolStripMenuItem";
            this.reportAProblemToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.reportAProblemToolStripMenuItem.Text = "Report a Problem";
            this.reportAProblemToolStripMenuItem.Click += new System.EventHandler(this.reportAProblemToolStripMenuItem_Click);
            // 
            // viewSourceCodeToolStripMenuItem
            // 
            this.viewSourceCodeToolStripMenuItem.Name = "viewSourceCodeToolStripMenuItem";
            this.viewSourceCodeToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.viewSourceCodeToolStripMenuItem.Text = "View Source Code";
            this.viewSourceCodeToolStripMenuItem.Click += new System.EventHandler(this.viewSourceCodeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(166, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // ntfIndicator
            // 
            this.ntfIndicator.Icon = ((System.Drawing.Icon)(resources.GetObject("ntfIndicator.Icon")));
            this.ntfIndicator.Text = "Key2Joy";
            this.ntfIndicator.Visible = true;
            this.ntfIndicator.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ntfIndicator_MouseDoubleClick);
            // 
            // pnlMainMenu
            // 
            this.pnlMainMenu.Controls.Add(this.menMainMenu);
            this.pnlMainMenu.Controls.Add(this.chkEnabled);
            this.pnlMainMenu.Controls.Add(this.lblStatusInactive);
            this.pnlMainMenu.Controls.Add(this.lblStatusActive);
            this.pnlMainMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMainMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlMainMenu.Name = "pnlMainMenu";
            this.pnlMainMenu.Size = new System.Drawing.Size(580, 23);
            this.pnlMainMenu.TabIndex = 85;
            // 
            // lblStatusInactive
            // 
            this.lblStatusInactive.BackColor = System.Drawing.Color.IndianRed;
            this.lblStatusInactive.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblStatusInactive.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblStatusInactive.Location = new System.Drawing.Point(379, 0);
            this.lblStatusInactive.Name = "lblStatusInactive";
            this.lblStatusInactive.Size = new System.Drawing.Size(109, 23);
            this.lblStatusInactive.TabIndex = 82;
            this.lblStatusInactive.Text = "(Mappings not active)";
            this.lblStatusInactive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatusActive
            // 
            this.lblStatusActive.BackColor = System.Drawing.Color.LawnGreen;
            this.lblStatusActive.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblStatusActive.Location = new System.Drawing.Point(488, 0);
            this.lblStatusActive.Name = "lblStatusActive";
            this.lblStatusActive.Size = new System.Drawing.Size(92, 23);
            this.lblStatusActive.TabIndex = 83;
            this.lblStatusActive.Text = "(Mappings active)";
            this.lblStatusActive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(580, 457);
            this.Controls.Add(this.olvMappings);
            this.Controls.Add(this.pnlActionManagement);
            this.Controls.Add(this.pnlPresetManagement);
            this.Controls.Add(this.pnlMainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "MainForm";
            this.Text = "Key2Joy - Prototype";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.olvMappings)).EndInit();
            this.pnlActionManagement.ResumeLayout(false);
            this.pnlPresetManagement.ResumeLayout(false);
            this.pnlPresetManagement.PerformLayout();
            this.menMainMenu.ResumeLayout(false);
            this.menMainMenu.PerformLayout();
            this.pnlMainMenu.ResumeLayout(false);
            this.pnlMainMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.Panel pnlActionManagement;
        private System.Windows.Forms.Panel pnlPresetManagement;
        private System.Windows.Forms.TextBox txtPresetName;
        private System.Windows.Forms.Label lblPresetName;
        private BrightIdeasSoftware.ObjectListView olvMappings;
        private BrightIdeasSoftware.OLVColumn olvColumnTrigger;
        private BrightIdeasSoftware.OLVColumn olvColumnAction;
        private System.Windows.Forms.MenuStrip menMainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadPresetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fillPresetWithToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportAProblemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewSourceCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button btnAddAction;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openPresetFolderToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon ntfIndicator;
        private System.Windows.Forms.ToolStripMenuItem newPresetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savePresetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitProgramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testMappingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Panel pnlMainMenu;
        private System.Windows.Forms.Label lblStatusInactive;
        private System.Windows.Forms.Label lblStatusActive;
        private System.Windows.Forms.ToolStripMenuItem testGamePadJoystickToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testKeyboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testMouseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allGamePadJoystickActionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pressAndReleaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem releaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allKeyboardActionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pressAndReleaseToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem pressToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem releaseToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem userConfigurationsToolStripMenuItem;
    }
}

