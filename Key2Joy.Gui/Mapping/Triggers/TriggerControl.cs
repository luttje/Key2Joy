using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Mapping;
using Key2Joy.Mapping.Triggers;
using Key2Joy.Plugins;

namespace Key2Joy.Gui.Mapping
{
    public partial class TriggerControl : UserControl
    {
        public AbstractTrigger Trigger { get; private set; }
        public event EventHandler<TriggerChangedEventArgs> TriggerChanged;
        public bool IsTopLevel { get; set; }

        private bool isLoaded = false;
        private ITriggerOptionsControl options;

        private AbstractTrigger selectedTrigger = null;

        public TriggerControl()
        {
            this.InitializeComponent();
        }

        private void BuildTrigger()
        {
            if (this.cmbTrigger.SelectedItem == null)
            {
                TriggerChanged?.Invoke(this, TriggerChangedEventArgs.Empty);
                return;
            }

            var selected = (ImageComboBoxItem<KeyValuePair<TriggerAttribute, MappingTypeFactory<AbstractTrigger>>>)this.cmbTrigger.SelectedItem;
            var selectedTypeFactory = selected.ItemValue.Value;
            var attribute = selected.ItemValue.Key;

            if (this.Trigger == null || this.Trigger.GetType().FullName != selectedTypeFactory.FullTypeName)
            {
                this.Trigger = selectedTypeFactory.CreateInstance(new object[]
                {
                    attribute.NameFormat,
                });
            }

            this.options?.Setup(this.Trigger);

            TriggerChanged?.Invoke(this, new TriggerChangedEventArgs(this.Trigger));
        }

        public void SelectTrigger(AbstractTrigger trigger)
        {
            this.selectedTrigger = trigger;

            if (!this.isLoaded)
            {
                return;
            }

            var selected = this.cmbTrigger.Items.Cast<ImageComboBoxItem<KeyValuePair<TriggerAttribute, MappingTypeFactory<AbstractTrigger>>>>();
            var triggerFullTypeName = trigger.GetType().FullName;
            var selectedType = selected.FirstOrDefault(x => x.ItemValue.Value.FullTypeName == triggerFullTypeName);
            this.cmbTrigger.SelectedItem = selectedType;
        }

        private void LoadTriggers()
        {
            var triggerTypes = TriggersRepository.GetAllTriggers(this.IsTopLevel);

            foreach (var keyValuePair in triggerTypes)
            {
                var mappingControlFactory = MappingControlRepository.GetMappingControlFactory(keyValuePair.Value.FullTypeName);
                var customImage = mappingControlFactory.ImageResourceName;
                var image = Program.ResourceBitmapFromName(customImage ?? "error");
                ImageComboBoxItem<KeyValuePair<TriggerAttribute, MappingTypeFactory<AbstractTrigger>>> item = new(keyValuePair, new Bitmap(image), "Key");

                this.cmbTrigger.Items.Add(item);
            }

            this.isLoaded = true;

            if (this.selectedTrigger != null)
            {
                this.SelectTrigger(this.selectedTrigger);
            }
        }

        private void CmbTrigger_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.isLoaded)
            {
                return;
            }

            var options = MappingForm.BuildOptionsForComboBox<TriggerAttribute, AbstractTrigger>(this.cmbTrigger, this.pnlTriggerOptions);

            if (options != null)
            {
                if (this.options != null)
                {
                    this.options.OptionsChanged -= this.OnOptionsChanged;
                }

                this.options = options as ITriggerOptionsControl;

                if (this.options != null)
                {
                    if (this.selectedTrigger != null)
                    {
                        this.options.Select(this.selectedTrigger);
                    }

                    this.options.OptionsChanged += this.OnOptionsChanged;
                }
            }

            this.BuildTrigger();

            this.selectedTrigger = null;
            this.PerformLayout();
        }

        private void OnOptionsChanged(object sender, EventArgs e)
        {
            var selected = (ImageComboBoxItem<KeyValuePair<TriggerAttribute, MappingTypeFactory<AbstractTrigger>>>)this.cmbTrigger.SelectedItem;
            _ = selected.ItemValue.Key;

            if (this.options == null) // TODO: what did I use this for before refactoring to seperate logic and GUI? --> || attribute.OptionsUserControl != options.GetType() 
            {
                return;
            }

            this.BuildTrigger();
        }

        private void TriggerControl_Load(object sender, EventArgs e)
        {
            this.LoadTriggers();
        }
    }
}
