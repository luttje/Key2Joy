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
    public partial class TriggerControl : UserControl
    {
        public BaseTrigger Trigger => BuildTrigger();
        private bool isLoaded = false;
        private ISetupTrigger options;
        
        public TriggerControl()
        {
            InitializeComponent();
        }

        private BaseTrigger BuildTrigger()
        {
            if (cmbTrigger.SelectedItem == null)
                return null;
            
            var selected = (KeyValuePair<Type, TriggerAttribute>)cmbTrigger.SelectedItem;
            var selectedType = selected.Key;
            var attribute = selected.Value;

            var trigger = (BaseTrigger)Activator.CreateInstance(selectedType, new object[]
            {
                attribute.Name as string,
                attribute.ImagePath as string,
            });

            if (options != null)
                options.Setup(trigger);
            
            return trigger;
        }

        private void LoadTriggers()
        {
            var triggerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttribute(typeof(TriggerAttribute), false) != null)
                .ToDictionary(t => t, t => t.GetCustomAttribute(typeof(TriggerAttribute), false) as TriggerAttribute);

            cmbTrigger.DataSource = new BindingSource(triggerTypes, null);
            cmbTrigger.DisplayMember = "Value";
            cmbTrigger.ValueMember = "Key";
            cmbTrigger.SelectedIndex = -1;

            isLoaded = true;
        }
        
        private void cmbTrigger_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoaded)
                return;
            
            var options = MappingForm.BuildOptionsForComboBox<TriggerAttribute>(cmbTrigger, pnlTriggerOptions);

            if (options == null)
                return;

            this.options = options as ISetupTrigger;

            PerformLayout();
        }

        private void TriggerControl_Load(object sender, EventArgs e)
        {
            LoadTriggers();
        }
    }
}
