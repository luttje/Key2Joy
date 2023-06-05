using Key2Joy.Contracts.Mapping;
using Key2Joy.Gui.Mapping;
using Key2Joy.Mapping;
using Key2Joy.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Gui
{
    public partial class MappingForm : Form
    {
        public MappedOption MappedOption { get; private set; } = null;

        public MappingForm()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;

            actionControl.IsTopLevel = true;
            triggerControl.IsTopLevel = true;

            FormClosed += (s, e) => Dispose();
        }

        public MappingForm(MappedOption mappedOption)
            :this()
        {
            if (mappedOption == null)
                return;
            
            MappedOption = mappedOption;

            triggerControl.SelectTrigger(mappedOption.Trigger);
            actionControl.SelectAction(mappedOption.Action);
        }

        public static UserControl BuildOptionsForComboBox<TAttribute, TAspect>(ComboBox comboBox, Panel optionsPanel)
            where TAttribute : MappingAttribute
            where TAspect : AbstractMappingAspect
        {
            var optionsUserControl = optionsPanel.Controls.OfType<UserControl>().FirstOrDefault();

            if (optionsUserControl != null) 
            {
                optionsPanel.Controls.Remove(optionsUserControl);
                optionsUserControl.Dispose();
            }

            if (comboBox.SelectedItem == null)
                return null;

            var selected = ((ImageComboBoxItem<KeyValuePair<TAttribute, MappingTypeFactory<TAspect>>>)comboBox.SelectedItem).ItemValue;
            var selectedTypeFactory = selected.Value;
            var attribute = selected.Key;

            optionsUserControl = CreateOptionsControl(selectedTypeFactory.FullTypeName, attribute);

            if (optionsUserControl == null)
            {
                MessageBox.Show("Could not create options control for " + selectedTypeFactory.FullTypeName + ". \n\nPlease report this issue to the developer.\n\nThe app will now crash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new NotImplementedException("Could not create options control for " + selectedTypeFactory.FullTypeName);
            }
            
            optionsPanel.Controls.Add(optionsUserControl);
            optionsUserControl.Dock = DockStyle.Top;

            return optionsUserControl;
        }

        private static UserControl CreateOptionsControl<TAttribute>(string selectedTypeName, TAttribute attribute) where TAttribute : MappingAttribute
        {
            var mappingControlFactory = MappingControlRepository.GetMappingControlFactory(selectedTypeName);

            if (mappingControlFactory == null)
                return null;

            return mappingControlFactory.CreateInstance<UserControl>();
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

            MappedOption = MappedOption ?? new MappedOption();
            MappedOption.Trigger = trigger;
            MappedOption.Action = action;

            if (!actionControl.CanMappingSave(MappedOption))
                return;

            DialogResult = DialogResult.OK;

            Close();
        }
    }
}
