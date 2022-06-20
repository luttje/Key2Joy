namespace KeyToJoy
{
    partial class ComplexMappingForm
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
            this.ndcLogic = new NodeEditor.NodesControl();
            this.btnExecute = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ndcLogic
            // 
            this.ndcLogic.Context = null;
            this.ndcLogic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ndcLogic.Location = new System.Drawing.Point(0, 0);
            this.ndcLogic.Name = "ndcLogic";
            this.ndcLogic.Size = new System.Drawing.Size(800, 450);
            this.ndcLogic.TabIndex = 0;
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecute.Location = new System.Drawing.Point(674, 12);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(114, 37);
            this.btnExecute.TabIndex = 1;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // ComplexMappingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.ndcLogic);
            this.Name = "ComplexMappingForm";
            this.Text = "Map triggers to actions with complex logic";
            this.Load += new System.EventHandler(this.ComplexMappingForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private NodeEditor.NodesControl ndcLogic;
        private System.Windows.Forms.Button btnExecute;
    }
}