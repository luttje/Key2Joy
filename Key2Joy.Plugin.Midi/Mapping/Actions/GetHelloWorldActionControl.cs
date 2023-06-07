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

            // Always set this, so the mapping form knows what height to set the parent panel.
            this.DesiredHeight = 25;
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
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
