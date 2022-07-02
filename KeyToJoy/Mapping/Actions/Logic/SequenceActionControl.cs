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
    public partial class SequenceActionControl : UserControl, IActionOptionsControl
    {
        public event Action OptionsChanged;
        
        private List<BaseAction> childActions;

        public SequenceActionControl()
        {
            InitializeComponent();

            childActions = new List<BaseAction>();
        }

        public void Select(BaseAction action)
        {
            var thisAction = (SequenceAction)action;

            foreach (var childAction in thisAction.ChildActions)
            {
                // Clone so we don't modify the action in a preset
                AddChildAction((BaseAction)childAction.Clone());
            }
        }

        public void Setup(BaseAction action)
        {
            var thisAction = (SequenceAction)action;

            foreach (var childAction in childActions)
            {
                thisAction.ChildActions.Add(childAction);
            }
        }

        private void AddChildAction(BaseAction action)
        {
            childActions.Add(action);
            lstActions.Items.Add(action);
        }

        private void RemoveChildAction(BaseAction action)
        {
            childActions.Remove(action);
            lstActions.Items.Remove(action);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddChildAction(actionControl.Action);
            OptionsChanged?.Invoke();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveChildAction((BaseAction)lstActions.SelectedValue);
            OptionsChanged?.Invoke();
        }

        private void lstActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = lstActions.SelectedIndex > -1;
        }

        private void actionControl_ActionChanged(BaseAction action)
        {
            btnAdd.Enabled = action != null;
        }
    }
}
