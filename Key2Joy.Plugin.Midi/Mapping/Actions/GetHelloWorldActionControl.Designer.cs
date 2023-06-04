namespace Key2Joy.Gui.Mapping
{
    partial class GetHelloWorldActionControl
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
            this.btnHelloWorld = new System.Windows.Forms.Button();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(43, 21);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.Text = "Greet:";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnHelloWorld
            // 
            this.btnHelloWorld.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnHelloWorld.Location = new System.Drawing.Point(243, 0);
            this.btnHelloWorld.Name = "btnHelloWorld";
            this.btnHelloWorld.Size = new System.Drawing.Size(62, 21);
            this.btnHelloWorld.TabIndex = 11;
            this.btnHelloWorld.Text = "Hello...";
            this.btnHelloWorld.UseVisualStyleBackColor = true;
            this.btnHelloWorld.Click += new System.EventHandler(this.btnHelloWorld_Click);
            // 
            // txtTarget
            // 
            this.txtTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTarget.Location = new System.Drawing.Point(43, 0);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(200, 20);
            this.txtTarget.TabIndex = 12;
            this.txtTarget.Text = "World";
            this.txtTarget.TextChanged += new System.EventHandler(this.txtTarget_TextChanged);
            // 
            // GetHelloWorldActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.btnHelloWorld);
            this.Controls.Add(this.lblInfo);
            this.Name = "GetHelloWorldActionControl";
            this.Size = new System.Drawing.Size(305, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnHelloWorld;
        private System.Windows.Forms.TextBox txtTarget;
    }
}
