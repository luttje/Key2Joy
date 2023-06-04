using Key2Joy.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Gui.Mapping
{
    [MappingControl(
        ForType = typeof(Key2Joy.Mapping.SequenceAction),
        ImageResourceName = "text_list_numbers"
    )]
    public partial class SequenceActionControl : UserControl, IActionOptionsControl
    {
        public event EventHandler OptionsChanged;
        
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
                // Clone so we don't modify the action in a profile
                AddChildAction((BaseAction)childAction.Clone());
            }
        }

        public void Setup(BaseAction action)
        {
            var thisAction = (SequenceAction)action;
            thisAction.ChildActions.Clear();

            foreach (var childAction in childActions)
            {
                thisAction.ChildActions.Add(childAction);
            }
        }

        public bool CanMappingSave(BaseAction action)
        {
            return true;
        }

        private void AddChildAction(BaseAction action)
        {
            childActions.Add(action);
            lstActions.Items.Add(action);
        }

        private void RemoveChildAction(BaseAction action)
        {
            var index = lstActions.Items.IndexOf(action);
            
            childActions.Remove(action);
            lstActions.Items.Remove(action);

            if (lstActions.Items.Count > 0)
            {
                if (index >= lstActions.Items.Count)
                {
                    index = lstActions.Items.Count - 1;
                }
                lstActions.SelectedIndex = index;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddChildAction((BaseAction)actionControl.Action.Clone());
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveChildAction((BaseAction)lstActions.SelectedItem);
            OptionsChanged?.Invoke(this, EventArgs.Empty);
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
