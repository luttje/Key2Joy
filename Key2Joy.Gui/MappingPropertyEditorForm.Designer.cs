namespace Key2Joy.Gui;

partial class MappingPropertyEditorForm
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
            this.grpValueEditor = new System.Windows.Forms.GroupBox();
            this.btnApplyChanges = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlValueInputParent = new System.Windows.Forms.Panel();
            this.grpValueEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpValueEditor
            // 
            this.grpValueEditor.Controls.Add(this.pnlValueInputParent);
            this.grpValueEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpValueEditor.Location = new System.Drawing.Point(5, 5);
            this.grpValueEditor.Name = "grpValueEditor";
            this.grpValueEditor.Size = new System.Drawing.Size(324, 51);
            this.grpValueEditor.TabIndex = 0;
            this.grpValueEditor.TabStop = false;
            this.grpValueEditor.Text = "Value Editor";
            // 
            // btnApplyChanges
            // 
            this.btnApplyChanges.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnApplyChanges.Location = new System.Drawing.Point(5, 56);
            this.btnApplyChanges.Name = "btnApplyChanges";
            this.btnApplyChanges.Size = new System.Drawing.Size(324, 33);
            this.btnApplyChanges.TabIndex = 0;
            this.btnApplyChanges.Text = "Apply Changes";
            this.btnApplyChanges.UseVisualStyleBackColor = true;
            this.btnApplyChanges.Click += new System.EventHandler(this.BtnApplyChanges_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(5, 89);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(324, 48);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // pnlValueInputParent
            // 
            this.pnlValueInputParent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlValueInputParent.Location = new System.Drawing.Point(3, 16);
            this.pnlValueInputParent.Name = "pnlValueInputParent";
            this.pnlValueInputParent.Padding = new System.Windows.Forms.Padding(5);
            this.pnlValueInputParent.Size = new System.Drawing.Size(318, 32);
            this.pnlValueInputParent.TabIndex = 0;
            // 
            // MappingPropertyEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 142);
            this.Controls.Add(this.grpValueEditor);
            this.Controls.Add(this.btnApplyChanges);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MappingPropertyEditorForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Editting Multiple Properties";
            this.grpValueEditor.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox grpValueEditor;
    private System.Windows.Forms.Button btnApplyChanges;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Panel pnlValueInputParent;
}
