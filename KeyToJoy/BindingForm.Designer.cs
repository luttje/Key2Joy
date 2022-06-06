namespace KeyToJoy
{
    partial class BindingForm
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pctController = new System.Windows.Forms.PictureBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlChkMouseMove = new System.Windows.Forms.Panel();
            this.cmbMouseDirection = new System.Windows.Forms.ComboBox();
            this.radMouseBind = new System.Windows.Forms.RadioButton();
            this.pnlCheckKeyBind = new System.Windows.Forms.Panel();
            this.txtKeyBind = new System.Windows.Forms.TextBox();
            this.radKeyBind = new System.Windows.Forms.RadioButton();
            this.lblInfo = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctController)).BeginInit();
            this.pnlContent.SuspendLayout();
            this.pnlChkMouseMove.SuspendLayout();
            this.pnlCheckKeyBind.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.pctController);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(454, 168);
            this.pnlHeader.TabIndex = 5;
            // 
            // pctController
            // 
            this.pctController.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pctController.Image = global::KeyToJoy.Properties.Resources.XboxSeriesX_Diagram;
            this.pctController.Location = new System.Drawing.Point(0, 0);
            this.pctController.Name = "pctController";
            this.pctController.Size = new System.Drawing.Size(454, 168);
            this.pctController.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pctController.TabIndex = 0;
            this.pctController.TabStop = false;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnConfirm.Enabled = false;
            this.btnConfirm.ForeColor = System.Drawing.Color.Black;
            this.btnConfirm.Location = new System.Drawing.Point(0, 258);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(454, 33);
            this.btnConfirm.TabIndex = 8;
            this.btnConfirm.Text = "Confirm binding X to Y";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.pnlChkMouseMove);
            this.pnlContent.Controls.Add(this.pnlCheckKeyBind);
            this.pnlContent.Controls.Add(this.lblInfo);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 168);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(454, 90);
            this.pnlContent.TabIndex = 9;
            // 
            // pnlChkMouseMove
            // 
            this.pnlChkMouseMove.Controls.Add(this.cmbMouseDirection);
            this.pnlChkMouseMove.Controls.Add(this.radMouseBind);
            this.pnlChkMouseMove.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChkMouseMove.Location = new System.Drawing.Point(0, 59);
            this.pnlChkMouseMove.Name = "pnlChkMouseMove";
            this.pnlChkMouseMove.Padding = new System.Windows.Forms.Padding(5);
            this.pnlChkMouseMove.Size = new System.Drawing.Size(454, 30);
            this.pnlChkMouseMove.TabIndex = 4;
            // 
            // cmbMouseDirection
            // 
            this.cmbMouseDirection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbMouseDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMouseDirection.Enabled = false;
            this.cmbMouseDirection.FormattingEnabled = true;
            this.cmbMouseDirection.Location = new System.Drawing.Point(127, 5);
            this.cmbMouseDirection.Name = "cmbMouseDirection";
            this.cmbMouseDirection.Size = new System.Drawing.Size(322, 21);
            this.cmbMouseDirection.TabIndex = 6;
            this.cmbMouseDirection.SelectedIndexChanged += new System.EventHandler(this.cmbMouseDirection_SelectedIndexChanged);
            // 
            // radMouseBind
            // 
            this.radMouseBind.AutoSize = true;
            this.radMouseBind.Dock = System.Windows.Forms.DockStyle.Left;
            this.radMouseBind.Location = new System.Drawing.Point(5, 5);
            this.radMouseBind.Name = "radMouseBind";
            this.radMouseBind.Size = new System.Drawing.Size(122, 20);
            this.radMouseBind.TabIndex = 5;
            this.radMouseBind.TabStop = true;
            this.radMouseBind.Text = "the mouse is moved:";
            this.radMouseBind.UseVisualStyleBackColor = true;
            // 
            // pnlCheckKeyBind
            // 
            this.pnlCheckKeyBind.Controls.Add(this.txtKeyBind);
            this.pnlCheckKeyBind.Controls.Add(this.radKeyBind);
            this.pnlCheckKeyBind.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCheckKeyBind.Location = new System.Drawing.Point(0, 29);
            this.pnlCheckKeyBind.Name = "pnlCheckKeyBind";
            this.pnlCheckKeyBind.Padding = new System.Windows.Forms.Padding(5);
            this.pnlCheckKeyBind.Size = new System.Drawing.Size(454, 30);
            this.pnlCheckKeyBind.TabIndex = 3;
            // 
            // txtKeyBind
            // 
            this.txtKeyBind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKeyBind.Enabled = false;
            this.txtKeyBind.Location = new System.Drawing.Point(175, 5);
            this.txtKeyBind.Name = "txtKeyBind";
            this.txtKeyBind.Size = new System.Drawing.Size(274, 20);
            this.txtKeyBind.TabIndex = 5;
            this.txtKeyBind.Text = "(press any key or click this field with a mouse button\r\n)";
            // 
            // radKeyBind
            // 
            this.radKeyBind.AutoSize = true;
            this.radKeyBind.Checked = true;
            this.radKeyBind.Dock = System.Windows.Forms.DockStyle.Left;
            this.radKeyBind.Location = new System.Drawing.Point(5, 5);
            this.radKeyBind.Name = "radKeyBind";
            this.radKeyBind.Size = new System.Drawing.Size(170, 20);
            this.radKeyBind.TabIndex = 4;
            this.radKeyBind.TabStop = true;
            this.radKeyBind.Text = "the following button is pressed:";
            this.radKeyBind.UseVisualStyleBackColor = true;
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInfo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(454, 29);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Pretend the X button is pressed when...";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BindingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(454, 291);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.pnlHeader);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "BindingForm";
            this.Text = "Press a key or select a mouse direction to bind...";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BindingForm_FormClosed);
            this.pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctController)).EndInit();
            this.pnlContent.ResumeLayout(false);
            this.pnlChkMouseMove.ResumeLayout(false);
            this.pnlChkMouseMove.PerformLayout();
            this.pnlCheckKeyBind.ResumeLayout(false);
            this.pnlCheckKeyBind.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.PictureBox pctController;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Panel pnlCheckKeyBind;
        private System.Windows.Forms.Panel pnlChkMouseMove;
        private System.Windows.Forms.RadioButton radKeyBind;
        private System.Windows.Forms.RadioButton radMouseBind;
        private System.Windows.Forms.TextBox txtKeyBind;
        private System.Windows.Forms.ComboBox cmbMouseDirection;
    }
}