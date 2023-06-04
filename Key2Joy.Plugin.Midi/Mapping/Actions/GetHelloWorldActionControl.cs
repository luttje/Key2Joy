using Key2Joy.Mapping;
using Key2Joy.Plugin.Midi.Mapping;
using System;
using System.Windows.Forms;

namespace Key2Joy.Gui.Mapping
{
    [MappingControl(
        ForType = typeof(Key2Joy.Plugin.Midi.Mapping.GetHelloWorldAction),
        ImageResourceName = "clock"
    )]
    public partial class GetHelloWorldActionControl : UserControl, IActionOptionsControl
    {
        public event EventHandler OptionsChanged;
        
        public GetHelloWorldActionControl()
        {
            InitializeComponent();
        }

        public void Select(BaseAction action)
        {
            var thisAction = (GetHelloWorldAction)action;

            txtTarget.Text = thisAction.Target;
        }

        public void Setup(BaseAction action)
        { 
            var thisAction = (GetHelloWorldAction)action;

            thisAction.Target = txtTarget.Text;
        }
        
        public bool CanMappingSave(BaseAction action)
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
