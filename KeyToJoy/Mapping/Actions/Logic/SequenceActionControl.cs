using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Mapping
{
    public partial class SequenceActionControl : UserControl, ISelectAndSetupAction
    {
        public SequenceActionControl()
        {
            InitializeComponent();
        }

        public void Select(BaseAction action)
        {
            var thisAction = (SequenceAction)action;

            // TODO: Select sub-actions here
        }

        public void Setup(BaseAction action)
        {
            var thisAction = (SequenceAction)action;

            // TODO: Setup sub-actions here
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

        }
    }
}
