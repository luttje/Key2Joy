using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Plugins;
using System;
using System.Windows.Forms;

namespace Key2Joy.Plugin.Midi.Mapping
{
    [MappingControl(
        ForType = typeof(Key2Joy.Plugin.Midi.Mapping.GetHelloWorldAction),
        ImageResourceName = "clock"
    )]
    public partial class GetHelloWorldActionControl : AbstractPluginForm, IActionOptionsControl
    {
        public event EventHandler OptionsChanged;
        
        public GetHelloWorldActionControl()
        {
            InitializeComponent();

            MappingConfigValues.Add(nameof(GetHelloWorldAction.Target), "");
        }

        public void Select(AbstractAction action)
        {
            var thisAction = (GetHelloWorldAction)action;

            txtTarget.Text = thisAction.Target;
        }

        public void Setup(AbstractAction action)
        { 
            var thisAction = (GetHelloWorldAction)action;

            thisAction.Target = txtTarget.Text;
            MappingConfigValues[nameof(GetHelloWorldAction.Target)] = txtTarget.Text;
        }
        
        public bool CanMappingSave(AbstractAction action)
        {
            return true;
        }

        private void btnHelloWorld_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Hello {txtTarget.Text}!");
        }

        private void txtTarget_TextChanged(object sender, EventArgs e)
        {
            MappingConfigValues[nameof(GetHelloWorldAction.Target)] = txtTarget.Text;
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
