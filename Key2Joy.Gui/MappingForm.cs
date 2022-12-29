using Key2Joy.Mapping;
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

        public static UserControl BuildOptionsForComboBox<TAttribute>(ComboBox comboBox, Panel optionsPanel)
            where TAttribute : MappingAttribute
        {
            var optionsUserControl = optionsPanel.Controls.OfType<UserControl>().FirstOrDefault();

            if (optionsUserControl != null) 
            {
                optionsPanel.Controls.Remove(optionsUserControl);
                optionsUserControl.Dispose();
            }

            if (comboBox.SelectedItem == null)
                return null;

            var selected = (KeyValuePair<TAttribute, Type>)comboBox.SelectedItem;
            var selectedType = selected.Value;
            var attribute = selected.Key;

            optionsUserControl = CreateOptionsControl(selectedType, attribute);

            if (optionsUserControl == null)
            {
                MessageBox.Show("Could not create options control for " + selectedType.Name + ". \n\nPlease report this issue to the developer.\n\nThe app will now crash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new NotImplementedException("Could not create options control for " + selectedType.Name);
            }
            
            optionsPanel.Controls.Add(optionsUserControl);
            optionsUserControl.Dock = DockStyle.Top;

            return optionsUserControl;
        }

        private static UserControl CreateOptionsControl<TAttribute>(Type selectedType, TAttribute attribute) where TAttribute : MappingAttribute
        {
            UserControl optionsUserControl;
            var typeName = selectedType.Name;
            var parameters = new object[] {};

            // If it's LuaScriptAction or JavascriptAction change the typename to ScriptAction
            if (selectedType == typeof(LuaScriptAction) 
                || selectedType == typeof(JavascriptAction))
            {
                typeName = "ScriptAction";
                parameters = new object[]{
                    selectedType == typeof(LuaScriptAction) ? "Lua" : "Javascript"
                };
            }

            var fullName = $"{typeof(MappingForm).Namespace}.{nameof(Mapping)}.{typeName}Control";
            var correspondingControl = Assembly.GetExecutingAssembly().GetType(fullName);

            if (correspondingControl == null)
                return null;

            optionsUserControl = (UserControl)Activator.CreateInstance(correspondingControl, parameters);
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
