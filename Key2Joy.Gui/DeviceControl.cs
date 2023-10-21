using System.Drawing;
using System.Windows.Forms;

namespace Key2Joy.Gui;

public partial class DeviceControl : UserControl
{
    private const int BORDER_WIDTH = 2;

    private DeviceControl()
    {
        this.InitializeComponent();

        this.pnlDevice.Paint += this.DeviceControl_Paint;
        this.Layout += this.DeviceControl_Layout;
    }

    private void DeviceControl_Layout(object sender, LayoutEventArgs e)
    {
        if (this.Height == this.Width)
        {
            return;
        }

        this.Height = this.Width;
    }

    public DeviceControl(int deviceIndex, string name)
        : this()
    {
        this.lblIndex.Text = $"#{deviceIndex}";
        this.lblDevice.Text = name;
        //this.picImage.Image = null;
    }

    private void DeviceControl_Paint(object sender, PaintEventArgs e)
    {
        var owner = (Control)sender;
        var g = e.Graphics;

        g.FillRectangle(Brushes.White, BORDER_WIDTH, BORDER_WIDTH, owner.Width - (BORDER_WIDTH * 2), owner.Height - (BORDER_WIDTH * 2));
    }
}
