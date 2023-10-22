using System;
using System.Windows;
using System.Windows.Controls;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Plugins;

namespace Key2Joy.Plugin.HelloWorld.Mapping.Actions;

[MappingControl(
    ForType = typeof(GetHelloWorldAction),
    ImageResourceName = "clock"
)]
public partial class GetHelloWorldActionControl : UserControl, IPluginUserControl, IPluginActionOptionsControl
{
    public event EventHandler OptionsChanged;

    public GetHelloWorldActionControl() => this.InitializeComponent();

    public int GetDesiredHeight() => 50;

    private void Button_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Hello from GetHelloWorldActionControl!");

    public void Select(PluginAction action)
    {
        var thisAction = (GetHelloWorldAction)action;

        this.txtName.Text = thisAction.Target;
    }

    public void Setup(PluginAction action)
    {
        var thisAction = (GetHelloWorldAction)action;

        thisAction.Target = this.txtName.Text;
    }

    public bool CanMappingSave(PluginAction action) => true;

    private void BtnHelloWorld_Click(object sender, EventArgs e) => MessageBox.Show($"Hello {this.txtName.Text}!");

    private void TxtName_TextChanged(object sender, EventArgs e) => OptionsChanged?.Invoke(this, EventArgs.Empty);
}
