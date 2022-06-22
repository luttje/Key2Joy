using KeyToJoy.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy
{
    public partial class MappingForm : Form
    {
        public MappingForm()
        {
            InitializeComponent();

            actionControl.IsTopLevel = true;
        }

        public static UserControl BuildOptionsForComboBox<TAttribute>(ComboBox comboBox, Panel optionsPanel)
            where TAttribute : MappingAttribute
        {
            optionsPanel.Controls.Clear();

            if (comboBox.SelectedItem == null)
                return null;

            var selected = (KeyValuePair<Type, TAttribute>)comboBox.SelectedItem;
            var selectedType = selected.Key;
            var attribute = selected.Value;

            if (attribute.OptionsUserControl == null)
                return null;
            
            var optionsUserControl = (UserControl)Activator.CreateInstance(attribute.OptionsUserControl);
            optionsPanel.Controls.Add(optionsUserControl);
            optionsUserControl.Dock = DockStyle.Top;

            return optionsUserControl;
        }

        private void btnSaveMapping_Click(object sender, EventArgs e)
        {
            var trigger = triggerControl.Trigger;
            var action = actionControl.Action;

            if (trigger == null)
            {
                MessageBox.Show("You must select a trigger!", "No trigger selected!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (action == null) 
            { 
                MessageBox.Show("You must select an action!", "No action selected!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
