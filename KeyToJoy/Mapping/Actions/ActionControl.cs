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
        public BaseAction Action { get; private set; }
        public event Action<BaseAction> ActionChanged;
        public bool IsTopLevel { get; set; }

        private bool isLoaded = false;
        private IActionOptionsControl options;
        private BaseAction selectedAction = null;

        public ActionControl()
        {
            InitializeComponent();
        }
        
        private void BuildAction()
        {
            if (cmbAction.SelectedItem == null) 
            {
                ActionChanged?.Invoke(null);
                return;
            }

            var selected = (KeyValuePair<Type, ActionAttribute>)cmbAction.SelectedItem;
            var selectedType = selected.Key;
            var attribute = selected.Value;

            if (Action == null || Action.GetType() != selectedType)
                Action = (BaseAction)Activator.CreateInstance(selectedType, new object[]
                {
                    attribute.NameFormat,
                    attribute.Description
                });

            if (options != null)
                options.Setup(Action);

            ActionChanged?.Invoke(Action);
        }

        internal void SelectAction(BaseAction action)
        {
            selectedAction = action;
            
            if (!isLoaded)
                return;

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

            if (options != null)
            {
                this.options = options as IActionOptionsControl;

                if (this.options != null)
                {
                    if(selectedAction != null)
                        this.options.Select(selectedAction);

                    this.options.OptionsChanged += () => BuildAction();
                }
            }
            
            BuildAction();
            
            selectedAction = null;
            PerformLayout();
        }

        private void ActionControl_Load(object sender, EventArgs e)
        {
            LoadActions();
        }
    }
}
