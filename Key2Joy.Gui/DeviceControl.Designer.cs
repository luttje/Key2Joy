namespace Key2Joy.Gui;

partial class DeviceControl
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
            this.picImage = new System.Windows.Forms.PictureBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.lblIndex = new System.Windows.Forms.Label();
            this.pnlDevice = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.pnlDevice.SuspendLayout();
            this.SuspendLayout();
            // 
            // picImage
            // 
            this.picImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picImage.Image = global::Key2Joy.Gui.Properties.Resources.disconnect;
            this.picImage.Location = new System.Drawing.Point(8, 26);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(77, 49);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picImage.TabIndex = 0;
            this.picImage.TabStop = false;
            // 
            // lblDevice
            // 
            this.lblDevice.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblDevice.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDevice.Location = new System.Drawing.Point(8, 75);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(77, 18);
            this.lblDevice.TabIndex = 1;
            this.lblDevice.Text = "Device Name";
            this.lblDevice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblIndex
            // 
            this.lblIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblIndex.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblIndex.Location = new System.Drawing.Point(8, 8);
            this.lblIndex.Name = "lblIndex";
            this.lblIndex.Size = new System.Drawing.Size(77, 18);
            this.lblIndex.TabIndex = 2;
            this.lblIndex.Text = "?";
            this.lblIndex.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pnlDevice
            // 
            this.pnlDevice.Controls.Add(this.picImage);
            this.pnlDevice.Controls.Add(this.lblIndex);
            this.pnlDevice.Controls.Add(this.lblDevice);
            this.pnlDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDevice.Location = new System.Drawing.Point(0, 0);
            this.pnlDevice.Name = "pnlDevice";
            this.pnlDevice.Padding = new System.Windows.Forms.Padding(8);
            this.pnlDevice.Size = new System.Drawing.Size(93, 101);
            this.pnlDevice.TabIndex = 3;
            // 
            // DeviceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnlDevice);
            this.Name = "DeviceControl";
            this.Size = new System.Drawing.Size(93, 101);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.pnlDevice.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox picImage;
    private System.Windows.Forms.Label lblDevice;
    private System.Windows.Forms.Label lblIndex;
    private System.Windows.Forms.Panel pnlDevice;
}
