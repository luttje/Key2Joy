namespace Key2Joy.Gui
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
            this.pnlFiltering = new System.Windows.Forms.Panel();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.txtFilterLabel = new System.Windows.Forms.Label();
            this.btnCreateMapping = new System.Windows.Forms.Button();
            this.pnlProfileManagement = new System.Windows.Forms.Panel();
            this.txtProfileName = new System.Windows.Forms.TextBox();
            this.lblProfileName = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.menMainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.saveProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProfileFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewMappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewScriptOutputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewEventViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.managePluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openPluginsFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fillProfileWithToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allGamePadJoystickActionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.releaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pressAndReleaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allKeyboardActionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pressToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.releaseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pressAndReleaseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.testMappingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testGamePadJoystickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.devicetestscomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gamepadtestercomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testKeyboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testMouseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.withSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateOppositePressStateMappingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.pnlFiltering.SuspendLayout();
            this.pnlProfileManagement.SuspendLayout();
            this.menMainMenu.SuspendLayout();
            this.pnlMainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // olvMappings
            // 
            this.olvMappings.AllColumns.Add(this.olvColumnTrigger);
            this.olvMappings.AllColumns.Add(this.olvColumnAction);
            this.olvMappings.CellEditUseWholeCell = false;
            this.olvMappings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnTrigger,
            this.olvColumnAction});
            this.olvMappings.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvMappings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvMappings.EmptyListMsg = "There are no mappings, or a search filter is applied that matched no mappings.";
            this.olvMappings.EmptyListMsgFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olvMappings.FullRowSelect = true;
            this.olvMappings.HideSelection = false;
            this.olvMappings.LabelWrap = false;
            this.olvMappings.Location = new System.Drawing.Point(0, 53);
            this.olvMappings.Name = "olvMappings";
            this.olvMappings.RowHeight = 25;
            this.olvMappings.Size = new System.Drawing.Size(684, 467);
            this.olvMappings.TabIndex = 84;
            this.olvMappings.UseCellFormatEvents = true;
            this.olvMappings.UseCompatibleStateImageBehavior = false;
            this.olvMappings.UseFiltering = true;
            this.olvMappings.UseHotItem = true;
            this.olvMappings.UseTranslucentHotItem = true;
            this.olvMappings.UseTranslucentSelection = true;
            this.olvMappings.View = System.Windows.Forms.View.Details;
            this.olvMappings.AboutToCreateGroups += new System.EventHandler<BrightIdeasSoftware.CreateGroupsEventArgs>(this.OlvMappings_AboutToCreateGroups);
            this.olvMappings.CellClick += new System.EventHandler<BrightIdeasSoftware.CellClickEventArgs>(this.OlvMappings_CellClick);
            this.olvMappings.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.OlvMappings_CellRightClick);
            this.olvMappings.FormatCell += new System.EventHandler<BrightIdeasSoftware.FormatCellEventArgs>(this.OlvMappings_FormatCell);
            this.olvMappings.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OlvMappings_KeyUp);
            // 
            // olvColumnAction
            // 
            this.olvColumnAction.AspectName = "Action";
            this.olvColumnAction.Text = "Action";
            this.olvColumnAction.UseInitialLetterForGroup = true;
            // 
            // olvColumnTrigger
            // 
            this.olvColumnTrigger.AspectName = "Trigger";
            this.olvColumnTrigger.Groupable = false;
            this.olvColumnTrigger.Text = "Trigger";
            // 
            // pnlActionManagement
            // 
            this.pnlActionManagement.Controls.Add(this.pnlFiltering);
            this.pnlActionManagement.Controls.Add(this.btnCreateMapping);
            this.pnlActionManagement.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActionManagement.Location = new System.Drawing.Point(0, 520);
            this.pnlActionManagement.Name = "pnlActionManagement";
            this.pnlActionManagement.Padding = new System.Windows.Forms.Padding(5);
            this.pnlActionManagement.Size = new System.Drawing.Size(684, 41);
            this.pnlActionManagement.TabIndex = 0;
            // 
            // pnlFiltering
            // 
            this.pnlFiltering.Controls.Add(this.txtFilter);
            this.pnlFiltering.Controls.Add(this.txtFilterLabel);
            this.pnlFiltering.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlFiltering.Location = new System.Drawing.Point(5, 5);
            this.pnlFiltering.Name = "pnlFiltering";
            this.pnlFiltering.Padding = new System.Windows.Forms.Padding(5);
            this.pnlFiltering.Size = new System.Drawing.Size(321, 31);
            this.pnlFiltering.TabIndex = 2;
            // 
            // txtFilter
            // 
            this.txtFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilter.Location = new System.Drawing.Point(82, 5);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(234, 20);
            this.txtFilter.TabIndex = 1;
            this.txtFilter.TextChanged += new System.EventHandler(this.TxtFilter_TextChanged);
            // 
            // txtFilterLabel
            // 
            this.txtFilterLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtFilterLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilterLabel.Location = new System.Drawing.Point(5, 5);
            this.txtFilterLabel.Name = "txtFilterLabel";
            this.txtFilterLabel.Size = new System.Drawing.Size(77, 21);
            this.txtFilterLabel.TabIndex = 89;
            this.txtFilterLabel.Text = "Search Filter:";
            this.txtFilterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCreateMapping
            // 
            this.btnCreateMapping.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCreateMapping.Location = new System.Drawing.Point(530, 5);
            this.btnCreateMapping.Name = "btnCreateMapping";
            this.btnCreateMapping.Size = new System.Drawing.Size(149, 31);
            this.btnCreateMapping.TabIndex = 0;
            this.btnCreateMapping.Text = "Create New Mapping";
            this.btnCreateMapping.UseVisualStyleBackColor = true;
            this.btnCreateMapping.Click += new System.EventHandler(this.BtnCreateMapping_Click);
            // 
            // pnlProfileManagement
            // 
            this.pnlProfileManagement.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pnlProfileManagement.Controls.Add(this.txtProfileName);
            this.pnlProfileManagement.Controls.Add(this.lblProfileName);
            this.pnlProfileManagement.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlProfileManagement.Location = new System.Drawing.Point(0, 23);
            this.pnlProfileManagement.Name = "pnlProfileManagement";
            this.pnlProfileManagement.Padding = new System.Windows.Forms.Padding(5);
            this.pnlProfileManagement.Size = new System.Drawing.Size(684, 30);
            this.pnlProfileManagement.TabIndex = 82;
            // 
            // txtProfileName
            // 
            this.txtProfileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProfileName.Location = new System.Drawing.Point(82, 5);
            this.txtProfileName.Name = "txtProfileName";
            this.txtProfileName.Size = new System.Drawing.Size(597, 20);
            this.txtProfileName.TabIndex = 85;
            this.txtProfileName.TextChanged += new System.EventHandler(this.TxtProfileName_TextChanged);
            // 
            // lblProfileName
            // 
            this.lblProfileName.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblProfileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfileName.Location = new System.Drawing.Point(5, 5);
            this.lblProfileName.Name = "lblProfileName";
            this.lblProfileName.Size = new System.Drawing.Size(77, 20);
            this.lblProfileName.TabIndex = 88;
            this.lblProfileName.Text = "Profile Name:";
            this.lblProfileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkEnabled.Location = new System.Drawing.Point(424, 0);
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
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menMainMenu.Location = new System.Drawing.Point(0, 0);
            this.menMainMenu.Name = "menMainMenu";
            this.menMainMenu.Size = new System.Drawing.Size(424, 24);
            this.menMainMenu.TabIndex = 81;
            this.menMainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProfileToolStripMenuItem,
            this.loadProfileToolStripMenuItem,
            this.toolStripSeparator3,
            this.saveProfileToolStripMenuItem,
            this.openProfileFolderToolStripMenuItem,
            this.toolStripSeparator2,
            this.closeToolStripMenuItem,
            this.exitProgramToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newProfileToolStripMenuItem
            // 
            this.newProfileToolStripMenuItem.Name = "newProfileToolStripMenuItem";
            this.newProfileToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.newProfileToolStripMenuItem.Text = "New Profile";
            this.newProfileToolStripMenuItem.Click += new System.EventHandler(this.NewProfileToolStripMenuItem_Click);
            // 
            // loadProfileToolStripMenuItem
            // 
            this.loadProfileToolStripMenuItem.Name = "loadProfileToolStripMenuItem";
            this.loadProfileToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.loadProfileToolStripMenuItem.Text = "Load Profile";
            this.loadProfileToolStripMenuItem.Click += new System.EventHandler(this.LoadProfileToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(201, 6);
            // 
            // saveProfileToolStripMenuItem
            // 
            this.saveProfileToolStripMenuItem.Name = "saveProfileToolStripMenuItem";
            this.saveProfileToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.saveProfileToolStripMenuItem.Text = "Save Profile";
            this.saveProfileToolStripMenuItem.Click += new System.EventHandler(this.SaveProfileToolStripMenuItem_Click);
            // 
            // openProfileFolderToolStripMenuItem
            // 
            this.openProfileFolderToolStripMenuItem.Name = "openProfileFolderToolStripMenuItem";
            this.openProfileFolderToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.openProfileFolderToolStripMenuItem.Text = "Open Profile Folder";
            this.openProfileFolderToolStripMenuItem.Click += new System.EventHandler(this.OpenProfileFolderToolStripMenuItem_Click);
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
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // exitProgramToolStripMenuItem
            // 
            this.exitProgramToolStripMenuItem.Image = global::Key2Joy.Gui.Properties.Resources.door_out;
            this.exitProgramToolStripMenuItem.Name = "exitProgramToolStripMenuItem";
            this.exitProgramToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.exitProgramToolStripMenuItem.Text = "Exit Program";
            this.exitProgramToolStripMenuItem.Click += new System.EventHandler(this.ExitProgramToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createNewMappingToolStripMenuItem,
            this.viewScriptOutputToolStripMenuItem,
            this.pluginsToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // createNewMappingToolStripMenuItem
            // 
            this.createNewMappingToolStripMenuItem.Name = "createNewMappingToolStripMenuItem";
            this.createNewMappingToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.createNewMappingToolStripMenuItem.Text = "Create New Mapping";
            this.createNewMappingToolStripMenuItem.Click += new System.EventHandler(this.CreateNewMappingToolStripMenuItem_Click);
            // 
            // viewScriptOutputToolStripMenuItem
            // 
            this.viewScriptOutputToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewLogFileToolStripMenuItem,
            this.viewEventViewerToolStripMenuItem});
            this.viewScriptOutputToolStripMenuItem.Name = "viewScriptOutputToolStripMenuItem";
            this.viewScriptOutputToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.viewScriptOutputToolStripMenuItem.Text = "View Script Output";
            // 
            // viewLogFileToolStripMenuItem
            // 
            this.viewLogFileToolStripMenuItem.Name = "viewLogFileToolStripMenuItem";
            this.viewLogFileToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.viewLogFileToolStripMenuItem.Text = "View Log File";
            this.viewLogFileToolStripMenuItem.Click += new System.EventHandler(this.ViewLogFileToolStripMenuItem_Click);
            // 
            // viewEventViewerToolStripMenuItem
            // 
            this.viewEventViewerToolStripMenuItem.Name = "viewEventViewerToolStripMenuItem";
            this.viewEventViewerToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.viewEventViewerToolStripMenuItem.Text = "View Event Viewer";
            this.viewEventViewerToolStripMenuItem.Click += new System.EventHandler(this.ViewEventViewerToolStripMenuItem_Click);
            // 
            // pluginsToolStripMenuItem
            // 
            this.pluginsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.managePluginsToolStripMenuItem,
            this.openPluginsFolderToolStripMenuItem});
            this.pluginsToolStripMenuItem.Image = global::Key2Joy.Gui.Properties.Resources.plugin;
            this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
            this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.pluginsToolStripMenuItem.Text = "Plugins";
            // 
            // managePluginsToolStripMenuItem
            // 
            this.managePluginsToolStripMenuItem.Name = "managePluginsToolStripMenuItem";
            this.managePluginsToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.managePluginsToolStripMenuItem.Text = "Manage Plugins";
            this.managePluginsToolStripMenuItem.Click += new System.EventHandler(this.ManagePluginsToolStripMenuItem_Click);
            // 
            // openPluginsFolderToolStripMenuItem
            // 
            this.openPluginsFolderToolStripMenuItem.Name = "openPluginsFolderToolStripMenuItem";
            this.openPluginsFolderToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.openPluginsFolderToolStripMenuItem.Text = "Open Plugins Folder";
            this.openPluginsFolderToolStripMenuItem.Click += new System.EventHandler(this.OpenPluginsFolderToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fillProfileWithToolStripMenuItem,
            this.testMappingsToolStripMenuItem,
            this.withSelectedToolStripMenuItem,
            this.toolStripSeparator4,
            this.userConfigurationsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // fillProfileWithToolStripMenuItem
            // 
            this.fillProfileWithToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allGamePadJoystickActionsToolStripMenuItem,
            this.allKeyboardActionsToolStripMenuItem});
            this.fillProfileWithToolStripMenuItem.Name = "fillProfileWithToolStripMenuItem";
            this.fillProfileWithToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.fillProfileWithToolStripMenuItem.Text = "Fill Profile With...";
            // 
            // allGamePadJoystickActionsToolStripMenuItem
            // 
            this.allGamePadJoystickActionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pressToolStripMenuItem,
            this.releaseToolStripMenuItem,
            this.pressAndReleaseToolStripMenuItem});
            this.allGamePadJoystickActionsToolStripMenuItem.Name = "allGamePadJoystickActionsToolStripMenuItem";
            this.allGamePadJoystickActionsToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.allGamePadJoystickActionsToolStripMenuItem.Text = "All GamePad/Joystick Actions";
            // 
            // pressToolStripMenuItem
            // 
            this.pressToolStripMenuItem.Name = "pressToolStripMenuItem";
            this.pressToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.pressToolStripMenuItem.Text = "Press";
            this.pressToolStripMenuItem.Click += new System.EventHandler(this.GamePadPressToolStripMenuItem_Click);
            // 
            // releaseToolStripMenuItem
            // 
            this.releaseToolStripMenuItem.Name = "releaseToolStripMenuItem";
            this.releaseToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.releaseToolStripMenuItem.Text = "Release";
            this.releaseToolStripMenuItem.Click += new System.EventHandler(this.GamePadReleaseToolStripMenuItem_Click);
            // 
            // pressAndReleaseToolStripMenuItem
            // 
            this.pressAndReleaseToolStripMenuItem.Name = "pressAndReleaseToolStripMenuItem";
            this.pressAndReleaseToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.pressAndReleaseToolStripMenuItem.Text = "Both Press and Release";
            this.pressAndReleaseToolStripMenuItem.Click += new System.EventHandler(this.GamePadPressAndReleaseToolStripMenuItem_Click);
            // 
            // allKeyboardActionsToolStripMenuItem
            // 
            this.allKeyboardActionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pressToolStripMenuItem1,
            this.releaseToolStripMenuItem1,
            this.pressAndReleaseToolStripMenuItem1});
            this.allKeyboardActionsToolStripMenuItem.Name = "allKeyboardActionsToolStripMenuItem";
            this.allKeyboardActionsToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.allKeyboardActionsToolStripMenuItem.Text = "All Keyboard Actions";
            // 
            // pressToolStripMenuItem1
            // 
            this.pressToolStripMenuItem1.Name = "pressToolStripMenuItem1";
            this.pressToolStripMenuItem1.Size = new System.Drawing.Size(194, 22);
            this.pressToolStripMenuItem1.Text = "Press";
            this.pressToolStripMenuItem1.Click += new System.EventHandler(this.KeyboardPressToolStripMenuItem_Click);
            // 
            // releaseToolStripMenuItem1
            // 
            this.releaseToolStripMenuItem1.Name = "releaseToolStripMenuItem1";
            this.releaseToolStripMenuItem1.Size = new System.Drawing.Size(194, 22);
            this.releaseToolStripMenuItem1.Text = "Release";
            this.releaseToolStripMenuItem1.Click += new System.EventHandler(this.KeyboardReleaseToolStripMenuItem_Click);
            // 
            // pressAndReleaseToolStripMenuItem1
            // 
            this.pressAndReleaseToolStripMenuItem1.Name = "pressAndReleaseToolStripMenuItem1";
            this.pressAndReleaseToolStripMenuItem1.Size = new System.Drawing.Size(194, 22);
            this.pressAndReleaseToolStripMenuItem1.Text = "Both Press and Release";
            this.pressAndReleaseToolStripMenuItem1.Click += new System.EventHandler(this.KeyboardPressAndReleaseToolStripMenuItem_Click);
            // 
            // testMappingsToolStripMenuItem
            // 
            this.testMappingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testGamePadJoystickToolStripMenuItem,
            this.testKeyboardToolStripMenuItem,
            this.testMouseToolStripMenuItem});
            this.testMappingsToolStripMenuItem.Name = "testMappingsToolStripMenuItem";
            this.testMappingsToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.testMappingsToolStripMenuItem.Text = "Test Mappings";
            // 
            // testGamePadJoystickToolStripMenuItem
            // 
            this.testGamePadJoystickToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.devicetestscomToolStripMenuItem,
            this.gamepadtestercomToolStripMenuItem});
            this.testGamePadJoystickToolStripMenuItem.Name = "testGamePadJoystickToolStripMenuItem";
            this.testGamePadJoystickToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.testGamePadJoystickToolStripMenuItem.Text = "Test GamePad / Joystick";
            // 
            // devicetestscomToolStripMenuItem
            // 
            this.devicetestscomToolStripMenuItem.Name = "devicetestscomToolStripMenuItem";
            this.devicetestscomToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.devicetestscomToolStripMenuItem.Text = "devicetests.com";
            this.devicetestscomToolStripMenuItem.Click += new System.EventHandler(this.DevicetestscomToolStripMenuItem_Click);
            // 
            // gamepadtestercomToolStripMenuItem
            // 
            this.gamepadtestercomToolStripMenuItem.Name = "gamepadtestercomToolStripMenuItem";
            this.gamepadtestercomToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.gamepadtestercomToolStripMenuItem.Text = "gamepad-tester.com";
            this.gamepadtestercomToolStripMenuItem.Click += new System.EventHandler(this.GamepadtestercomToolStripMenuItem_Click);
            // 
            // testKeyboardToolStripMenuItem
            // 
            this.testKeyboardToolStripMenuItem.Name = "testKeyboardToolStripMenuItem";
            this.testKeyboardToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.testKeyboardToolStripMenuItem.Text = "Test Keyboard";
            this.testKeyboardToolStripMenuItem.Click += new System.EventHandler(this.TestKeyboardToolStripMenuItem_Click);
            // 
            // testMouseToolStripMenuItem
            // 
            this.testMouseToolStripMenuItem.Name = "testMouseToolStripMenuItem";
            this.testMouseToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.testMouseToolStripMenuItem.Text = "Test Mouse";
            this.testMouseToolStripMenuItem.Click += new System.EventHandler(this.TestMouseToolStripMenuItem_Click);
            // 
            // withSelectedToolStripMenuItem
            // 
            this.withSelectedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateOppositePressStateMappingsToolStripMenuItem});
            this.withSelectedToolStripMenuItem.Name = "withSelectedToolStripMenuItem";
            this.withSelectedToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.withSelectedToolStripMenuItem.Text = "With Selected...";
            // 
            // generateOppositePressStateMappingsToolStripMenuItem
            // 
            this.generateOppositePressStateMappingsToolStripMenuItem.Name = "generateOppositePressStateMappingsToolStripMenuItem";
            this.generateOppositePressStateMappingsToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.generateOppositePressStateMappingsToolStripMenuItem.Text = "Generate Opposite Press State Mappings";
            this.generateOppositePressStateMappingsToolStripMenuItem.Click += new System.EventHandler(this.GenerateOppositePressStateMappingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(176, 6);
            // 
            // userConfigurationsToolStripMenuItem
            // 
            this.userConfigurationsToolStripMenuItem.Image = global::Key2Joy.Gui.Properties.Resources.cog;
            this.userConfigurationsToolStripMenuItem.Name = "userConfigurationsToolStripMenuItem";
            this.userConfigurationsToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.userConfigurationsToolStripMenuItem.Text = "User Configurations";
            this.userConfigurationsToolStripMenuItem.Click += new System.EventHandler(this.UserConfigurationsToolStripMenuItem_Click);
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
            this.reportAProblemToolStripMenuItem.Click += new System.EventHandler(this.ReportAProblemToolStripMenuItem_Click);
            // 
            // viewSourceCodeToolStripMenuItem
            // 
            this.viewSourceCodeToolStripMenuItem.Name = "viewSourceCodeToolStripMenuItem";
            this.viewSourceCodeToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.viewSourceCodeToolStripMenuItem.Text = "View Source Code";
            this.viewSourceCodeToolStripMenuItem.Click += new System.EventHandler(this.ViewSourceCodeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(166, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::Key2Joy.Gui.Properties.Resources.information;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // ntfIndicator
            // 
            this.ntfIndicator.Icon = ((System.Drawing.Icon)(resources.GetObject("ntfIndicator.Icon")));
            this.ntfIndicator.Text = "Key2Joy";
            this.ntfIndicator.Visible = true;
            this.ntfIndicator.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NtfIndicator_MouseDoubleClick);
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
            this.pnlMainMenu.Size = new System.Drawing.Size(684, 23);
            this.pnlMainMenu.TabIndex = 85;
            // 
            // lblStatusInactive
            // 
            this.lblStatusInactive.BackColor = System.Drawing.Color.IndianRed;
            this.lblStatusInactive.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblStatusInactive.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblStatusInactive.Location = new System.Drawing.Point(483, 0);
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
            this.lblStatusActive.Location = new System.Drawing.Point(592, 0);
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
            this.ClientSize = new System.Drawing.Size(684, 561);
            this.Controls.Add(this.olvMappings);
            this.Controls.Add(this.pnlActionManagement);
            this.Controls.Add(this.pnlProfileManagement);
            this.Controls.Add(this.pnlMainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "MainForm";
            this.Text = "Key2Joy - Alpha Version";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.olvMappings)).EndInit();
            this.pnlActionManagement.ResumeLayout(false);
            this.pnlFiltering.ResumeLayout(false);
            this.pnlFiltering.PerformLayout();
            this.pnlProfileManagement.ResumeLayout(false);
            this.pnlProfileManagement.PerformLayout();
            this.menMainMenu.ResumeLayout(false);
            this.menMainMenu.PerformLayout();
            this.pnlMainMenu.ResumeLayout(false);
            this.pnlMainMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.Panel pnlActionManagement;
        private System.Windows.Forms.Panel pnlProfileManagement;
        private System.Windows.Forms.TextBox txtProfileName;
        private System.Windows.Forms.Label lblProfileName;
        private BrightIdeasSoftware.OLVColumn olvColumnTrigger;
        private BrightIdeasSoftware.OLVColumn olvColumnAction;
        private System.Windows.Forms.MenuStrip menMainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fillProfileWithToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportAProblemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewSourceCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button btnCreateMapping;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProfileFolderToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon ntfIndicator;
        private System.Windows.Forms.ToolStripMenuItem newProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProfileToolStripMenuItem;
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
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createNewMappingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewScriptOutputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewLogFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewEventViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem devicetestscomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gamepadtestercomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pluginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem managePluginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openPluginsFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem withSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateOppositePressStateMappingsToolStripMenuItem;
        private BrightIdeasSoftware.ObjectListView olvMappings;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Panel pnlFiltering;
        private System.Windows.Forms.Label txtFilterLabel;
    }
}

