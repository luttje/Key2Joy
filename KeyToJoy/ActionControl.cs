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
    public partial class ActionControl : UserControl
    {
        private bool isLoaded = false;
        
        public ActionControl()
        {
            InitializeComponent();

            LoadActions();
        }
        
        private void LoadActions()
        {
            var actionTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                {
                    var actionAttribute = t.GetCustomAttributes(typeof(ActionAttribute), false).FirstOrDefault() as ActionAttribute;
                    return actionAttribute != null && actionAttribute.IsTopLevel;
                })
                .ToDictionary(t => t, t => t.GetCustomAttribute(typeof(ActionAttribute), false) as MappingAttribute);

            cmbAction.DataSource = new BindingSource(actionTypes, null);
            cmbAction.DisplayMember = "Value";
            cmbAction.ValueMember = "Key";
            cmbAction.SelectedIndex = -1;

            isLoaded = true;
        }
        
        private void HandleComboBox(ComboBox comboBox, Panel optionsPanel)
        {
            optionsPanel.Controls.Clear();

            if (comboBox.SelectedItem == null)
                return;

            var selected = (KeyValuePair<Type, MappingAttribute>)comboBox.SelectedItem;
            var selectedType = selected.Key;
            var attribute = selected.Value;

            if (attribute.OptionsUserControl != null)
            {
                var optionsUserControl = (UserControl)Activator.CreateInstance(attribute.OptionsUserControl);
                optionsPanel.Controls.Add(optionsUserControl);
                optionsUserControl.Dock = DockStyle.Top;
            }

            PerformLayout();
        }

        private void cmbAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoaded)
                return;
            
            HandleComboBox(cmbAction, pnlActionOptions);
        }
    }
}
