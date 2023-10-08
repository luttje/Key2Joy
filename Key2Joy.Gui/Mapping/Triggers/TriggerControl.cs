using Key2Joy.Contracts.Mapping;
using Key2Joy.Mapping;
using Key2Joy.Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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
            InitializeComponent();
        }

        private void BuildTrigger()
        {
            if (cmbTrigger.SelectedItem == null)
            {
                TriggerChanged?.Invoke(this, TriggerChangedEventArgs.Empty);
                return;
            }

            var selected = (ImageComboBoxItem<KeyValuePair<TriggerAttribute, MappingTypeFactory<AbstractTrigger>>>)cmbTrigger.SelectedItem;
            var selectedTypeFactory = selected.ItemValue.Value;
            var attribute = selected.ItemValue.Key;

            if (Trigger == null || Trigger.GetType().FullName != selectedTypeFactory.FullTypeName)
            {
                Trigger = selectedTypeFactory.CreateInstance(new object[]
                {
                    attribute.NameFormat,
                });
            }

            options?.Setup(Trigger);

            TriggerChanged?.Invoke(this, new TriggerChangedEventArgs(Trigger));
        }

        public void SelectTrigger(AbstractTrigger trigger)
        {
            selectedTrigger = trigger;

            if (!isLoaded)
                return;

            var selected = cmbTrigger.Items.Cast<ImageComboBoxItem<KeyValuePair<TriggerAttribute, MappingTypeFactory<AbstractTrigger>>>>();
            var triggerFullTypeName = trigger.GetType().FullName;
            var selectedType = selected.FirstOrDefault(x => x.ItemValue.Value.FullTypeName == triggerFullTypeName);
            cmbTrigger.SelectedItem = selectedType;
        }

        private void LoadTriggers()
        {
            var triggerTypes = TriggersRepository.GetAllTriggers(IsTopLevel);

            foreach (var keyValuePair in triggerTypes)
            {
                var mappingControlFactory = MappingControlRepository.GetMappingControlFactory(keyValuePair.Value.FullTypeName);
                var customImage = mappingControlFactory.ImageResourceName;
                var image = Program.ResourceBitmapFromName(customImage ?? "error");
                var item = new ImageComboBoxItem<KeyValuePair<TriggerAttribute, MappingTypeFactory<AbstractTrigger>>>(keyValuePair, new Bitmap(image), "Key");

                cmbTrigger.Items.Add(item);
            }

            isLoaded = true;

            if (selectedTrigger != null)
                SelectTrigger(selectedTrigger);
        }

        private void cmbTrigger_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoaded)
                return;

            var options = MappingForm.BuildOptionsForComboBox<TriggerAttribute, AbstractTrigger>(cmbTrigger, pnlTriggerOptions);

            if (options != null)
            {
                if (this.options != null)
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

        private void OnOptionsChanged(object sender, EventArgs e)
        {
            var selected = (ImageComboBoxItem<KeyValuePair<TriggerAttribute, MappingTypeFactory<AbstractTrigger>>>)cmbTrigger.SelectedItem;
            var attribute = selected.ItemValue.Key;

            if (options == null) // TODO: what did I use this for before refactoring to seperate logic and GUI? --> || attribute.OptionsUserControl != options.GetType() 
                return;

            BuildTrigger();
        }

        private void TriggerControl_Load(object sender, EventArgs e)
        {
            LoadTriggers();
        }
    }
}
