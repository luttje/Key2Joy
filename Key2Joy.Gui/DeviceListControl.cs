using System.Windows.Forms;
using CommonServiceLocator;
using Key2Joy.LowLevelInput.SimulatedGamePad;
using Key2Joy.LowLevelInput.XInput;

namespace Key2Joy.Gui;

public partial class DeviceListControl : UserControl
{
    public DeviceListControl()
    {
        this.InitializeComponent();

        if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv")
        {
            return; // The designer can't handle the code below.
        }

        this.RefreshDevices();
    }

    public void RefreshDevices()
    {
        this.pnlDevices.Controls.Clear();

        this.RefreshSimulatedDevices();
        this.RefreshPhysicalDevices();
    }

    private void AddDeviceControl(DeviceControl control)
    {
        control.Dock = DockStyle.Top;
        this.pnlDevices.Controls.Add(control);
    }

    private void RefreshPhysicalDevices()
    {
        var xInputService = ServiceLocator.Current.GetInstance<IXInputService>();
        xInputService.RecognizePhysicalDevices();
        var deviceIndexes = xInputService.GetActiveDevicesInfo();

        foreach (var device in deviceIndexes)
        {
            this.AddDeviceControl(new DeviceControl(device));
        }
    }

    private void RefreshSimulatedDevices()
    {
        var gamePadService = ServiceLocator.Current.GetInstance<ISimulatedGamePadService>();
        var simulatedGamePads = gamePadService.GetActiveDevicesInfo();

        foreach (var gamePad in simulatedGamePads)
        {
            this.AddDeviceControl(new DeviceControl(gamePad));
        }
    }

    private void BtnRefresh_Click(object sender, System.EventArgs e) => this.RefreshDevices();
}
