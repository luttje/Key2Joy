using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Plugins;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Key2Joy.Plugin.HelloWorld.Mapping
{
    [MappingControl(
        ForType = typeof(Key2Joy.Plugin.HelloWorld.Mapping.GetHelloWorldAction),
        ImageResourceName = "clock"
    )]
    public partial class GetHelloWorldActionControl : UserControl, IPluginUserControl, IActionOptionsControl
    {
        public event EventHandler OptionsChanged;

        public GetHelloWorldActionControl()
        {
            InitializeComponent();
        }

        public int GetDesiredHeight() => 50;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello from GetHelloWorldActionControl!");
        }

        public void Select(object action)
        {
            var thisAction = (GetHelloWorldAction)action;

            txtName.Text = thisAction.Target;
        }

        public void Setup(object action)
        {
            var thisAction = (GetHelloWorldAction)action;

            thisAction.Target = txtName.Text;
        }

        public bool CanMappingSave(object action)
        {
            return true;
        }

        private void btnHelloWorld_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Hello {txtName.Text}!");
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}