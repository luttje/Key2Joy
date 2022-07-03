namespace Key2Joy
{
    partial class OutputForm
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
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.chkAutoScroll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // rtbOutput
            // 
            this.rtbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbOutput.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbOutput.Location = new System.Drawing.Point(0, 0);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.ReadOnly = true;
            this.rtbOutput.Size = new System.Drawing.Size(819, 375);
            this.rtbOutput.TabIndex = 0;
            this.rtbOutput.Text = "";
            // 
            // chkAutoScroll
            // 
            this.chkAutoScroll.AutoSize = true;
            this.chkAutoScroll.Checked = true;
            this.chkAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoScroll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chkAutoScroll.Location = new System.Drawing.Point(0, 375);
            this.chkAutoScroll.Name = "chkAutoScroll";
            this.chkAutoScroll.Padding = new System.Windows.Forms.Padding(5);
            this.chkAutoScroll.Size = new System.Drawing.Size(819, 27);
            this.chkAutoScroll.TabIndex = 1;
            this.chkAutoScroll.Text = "Automatically scroll to bottom when new logs are added.";
            this.chkAutoScroll.UseVisualStyleBackColor = true;
            // 
            // OutputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 402);
            this.Controls.Add(this.rtbOutput);
            this.Controls.Add(this.chkAutoScroll);
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "OutputForm";
            this.ShowIcon = false;
            this.Text = "Key2Joy Output";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OutputForm_FormClosed);
            this.Load += new System.EventHandler(this.OutputForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.CheckBox chkAutoScroll;
    }
}