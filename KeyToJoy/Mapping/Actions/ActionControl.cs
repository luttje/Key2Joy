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

namespace KeyToJoy.Mapping
{
    public partial class ActionControl : UserControl
    {
        public BaseAction Action => BuildAction();

        public bool IsTopLevel { get; set; }

        private bool isLoaded = false;
        private ISelectAndSetupAction options;
        private BaseAction selectedAction = null;


        public ActionControl()
        {
            InitializeComponent();
        }

        private BaseAction BuildAction()
        {
            if (cmbAction.SelectedItem == null)
                return null;

            var selected = (KeyValuePair<Type, ActionAttribute>)cmbAction.SelectedItem;
            var selectedType = selected.Key;
            var attribute = selected.Value;

            var action = (BaseAction)Activator.CreateInstance(selectedType, new object[]
            {
                attribute.Name
            });

            if (options != null)
                options.Setup(action);

            return action;
        }

        internal void SelectAction(BaseAction action)
        {
            if (!isLoaded)
            {
                selectedAction = action;
                return;
            }

            var selected = cmbAction.Items.Cast<KeyValuePair<Type, ActionAttribute>>();
            var selectedType = selected.FirstOrDefault(x => x.Key == action.GetType());
            cmbAction.SelectedItem = selectedType;
        }

        private void LoadActions()
        {
            var actionTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                {
                    var actionAttribute = t.GetCustomAttributes(typeof(ActionAttribute), false).FirstOrDefault() as ActionAttribute;
                    
                    if (actionAttribute == null 
                    || actionAttribute.Visibility == ActionVisibility.Never)
                        return false;

                    if(IsTopLevel)
                        return actionAttribute.Visibility == ActionVisibility.Always 
                            || actionAttribute.Visibility == ActionVisibility.OnlyTopLevel;

                    return actionAttribute.Visibility == ActionVisibility.Always || actionAttribute.Visibility == ActionVisibility.UnlessTopLevel;
                })
                .ToDictionary(t => t, t => t.GetCustomAttribute(typeof(ActionAttribute), false) as ActionAttribute);

            cmbAction.DataSource = new BindingSource(actionTypes, null);
            cmbAction.DisplayMember = "Value";
            cmbAction.ValueMember = "Key";
            cmbAction.SelectedIndex = -1;

            isLoaded = true;

            if(selectedAction != null)
                SelectAction(selectedAction);
        }
        
        private void cmbAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoaded)
                return;
            
            var options = MappingForm.BuildOptionsForComboBox<ActionAttribute>(cmbAction, pnlActionOptions);
            
            if (options == null)
                return;

            this.options = options as ISelectAndSetupAction;

            if (this.options != null && selectedAction != null)
                this.options.Select(selectedAction);

            PerformLayout();
        }

        private void ActionControl_Load(object sender, EventArgs e)
        {
            LoadActions();
        }
    }
}
