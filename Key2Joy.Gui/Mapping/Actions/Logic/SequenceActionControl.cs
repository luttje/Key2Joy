using Key2Joy.Contracts.Mapping;
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
        
        private List<AbstractAction> childActions;

        public SequenceActionControl()
        {
            InitializeComponent();

            childActions = new List<AbstractAction>();
        }

        public void Select(object action)
        {
            var thisAction = (SequenceAction)action;

            foreach (var childAction in thisAction.ChildActions)
            {
                // Clone so we don't modify the action in a profile
                AddChildAction((AbstractAction)childAction.Clone());
            }
        }

        public void Setup(object action)
        {
            var thisAction = (SequenceAction)action;
            thisAction.ChildActions.Clear();

            foreach (var childAction in childActions)
            {
                throw new NotImplementedException("TODO:::::");
               // thisAction.ChildActions.Add(PluginAction.GetFullyFormedAction(childAction));
            }
        }

        public bool CanMappingSave(object action)
        {
            return true;
        }

        private void AddChildAction(AbstractAction action)
        {
            childActions.Add(action);
            lstActions.Items.Add(action);
        }

        private void RemoveChildAction(AbstractAction action)
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
            AddChildAction((AbstractAction)actionControl.Action.Clone());
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveChildAction((AbstractAction)lstActions.SelectedItem);
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void lstActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = lstActions.SelectedIndex > -1;
        }

        private void actionControl_ActionChanged(AbstractAction action)
        {
            btnAdd.Enabled = action != null;
        }
    }
}
