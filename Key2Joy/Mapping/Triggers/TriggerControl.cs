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

namespace Key2Joy.Mapping
{
    public partial class TriggerControl : UserControl
    {
        public BaseTrigger Trigger { get; private set; }
        public event Action<BaseTrigger> TriggerChanged;
        
        private bool isLoaded = false;
        private ITriggerOptionsControl options;

        private BaseTrigger selectedTrigger = null;
        
        public TriggerControl()
        {
            InitializeComponent();
        }

        private void BuildTrigger()
        {
            if (cmbTrigger.SelectedItem == null)
            {
                TriggerChanged?.Invoke(null);
                return;
            }

            var selected = (KeyValuePair<Type, TriggerAttribute>)cmbTrigger.SelectedItem;
            var selectedType = selected.Key;
            var attribute = selected.Value;

            if(Trigger == null || Trigger.GetType() != selectedType)
                Trigger = (BaseTrigger)Activator.CreateInstance(selectedType, new object[]
                {
                    attribute.NameFormat,
                    attribute.Description
                });

            if (options != null)
                options.Setup(Trigger);

            TriggerChanged?.Invoke(Trigger);
        }

        internal void SelectTrigger(BaseTrigger trigger)
        {
            selectedTrigger = trigger;
            
            if (!isLoaded)
                return;
            
            var selected = cmbTrigger.Items.Cast<KeyValuePair<Type, TriggerAttribute>>();
            var selectedType = selected.FirstOrDefault(x => x.Key == trigger.GetType());
            cmbTrigger.SelectedItem = selectedType;
        }

        private void LoadTriggers()
        {
            var triggerTypes = TriggerAttribute.GetAllTriggers();

            cmbTrigger.DataSource = new BindingSource(triggerTypes, null);
            cmbTrigger.DisplayMember = "Value";
            cmbTrigger.ValueMember = "Key";
            cmbTrigger.SelectedIndex = -1;

            isLoaded = true;

            if (selectedTrigger != null)
                SelectTrigger(selectedTrigger);
        }
        
        private void cmbTrigger_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoaded)
                return;
            
            var options = MappingForm.BuildOptionsForComboBox<TriggerAttribute>(cmbTrigger, pnlTriggerOptions);

            if (options != null)
            {
                if(this.options != null)
                    this.options.OptionsChanged -= OnOptionsChanged;

                this.options = options as ITriggerOptionsControl;

                if (this.options != null)
                {
                    if (selectedTrigger != null)
                        this.options.Select(selectedTrigger);

                    this.options.OptionsChanged += OnOptionsChanged;
                }
            }
            
            BuildTrigger();
            
            selectedTrigger = null;
            PerformLayout();
        }

        private void OnOptionsChanged()
        {
            var selected = (KeyValuePair<Type, TriggerAttribute>)cmbTrigger.SelectedItem;
            var attribute = selected.Value;

            if (options == null || attribute.OptionsUserControl != options.GetType())
                return;
            
            BuildTrigger();
        }

        private void TriggerControl_Load(object sender, EventArgs e)
        {
            LoadTriggers();
        }
    }
}
