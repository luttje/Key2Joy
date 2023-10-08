using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Gui.Mapping;
using Key2Joy.Mapping;
using Key2Joy.Plugins;

namespace Key2Joy.Gui
{
    public partial class MappingForm : Form
    {
        public MappedOption MappedOption { get; private set; } = null;

        public MappingForm()
        {
            this.InitializeComponent();
            this.DialogResult = DialogResult.Cancel;

            this.actionControl.IsTopLevel = true;
            this.triggerControl.IsTopLevel = true;

            FormClosed += (s, e) => this.Dispose();
        }

        public MappingForm(MappedOption mappedOption)
            : this()
        {
            if (mappedOption == null)
            {
                return;
            }

            this.MappedOption = mappedOption;

            this.triggerControl.SelectTrigger(mappedOption.Trigger);
            this.actionControl.SelectAction(mappedOption.Action);
        }

        public static Control BuildOptionsForComboBox<TAttribute, TAspect>(ComboBox comboBox, Panel optionsPanel)
            where TAttribute : MappingAttribute
            where TAspect : AbstractMappingAspect
        {
            var optionsUserControl = optionsPanel.Controls.OfType<Control>().FirstOrDefault();

            if (optionsUserControl != null)
            {
                optionsPanel.Controls.Remove(optionsUserControl);
                optionsUserControl.Dispose();
            }

            if (comboBox.SelectedItem == null)
            {
                return null;
            }

            var selected = ((ImageComboBoxItem<KeyValuePair<TAttribute, MappingTypeFactory<TAspect>>>)comboBox.SelectedItem).ItemValue;
            var selectedTypeFactory = selected.Value;

            optionsUserControl = CreateOptionsControl(selectedTypeFactory.FullTypeName);

            if (optionsUserControl == null)
            {
                MessageBox.Show("Could not create options control for " + selectedTypeFactory.FullTypeName + ". \n\nPlease report this issue to the developer.\n\nThe app will now crash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new NotImplementedException("Could not create options control for " + selectedTypeFactory.FullTypeName);
            }

            if (optionsUserControl is ElementHostProxy pluginUserControl)
            {
                optionsUserControl = new ActionPluginHostControl(pluginUserControl);
            }

            optionsPanel.Controls.Add(optionsUserControl);
            optionsUserControl.Dock = DockStyle.Top;

            return optionsUserControl;
        }

        private static Control CreateOptionsControl(string selectedTypeName)
        {
            var mappingControlFactory = MappingControlRepository.GetMappingControlFactory(selectedTypeName);

            if (mappingControlFactory == null)
            {
                return null;
            }

            return mappingControlFactory.CreateInstance<Control>();
        }

        private void BtnSaveMapping_Click(object sender, EventArgs e)
        {
            var trigger = this.triggerControl.Trigger;
            var action = this.actionControl.Action;

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

            this.MappedOption ??= new MappedOption();
            this.MappedOption.Trigger = trigger;
            this.MappedOption.Action = action;

            if (!this.actionControl.CanMappingSave(this.MappedOption))
            {
                return;
            }

            this.DialogResult = DialogResult.OK;

            this.Close();
        }
    }
}
