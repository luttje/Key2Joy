using Key2Joy.Contracts.Mapping;
using Key2Joy.Mapping;
using Key2Joy.Plugins;
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

namespace Key2Joy.Gui.Mapping
{
    public partial class ActionControl : UserControl
    {
        public AbstractAction Action { get; private set; }
        public event Action<AbstractAction> ActionChanged;
        public bool IsTopLevel { get; set; }

        private bool isLoaded = false;
        private IActionOptionsControl options;
        private AbstractAction selectedAction = null;

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

            var selected = (ImageComboBoxItem<KeyValuePair<ActionAttribute, MappingTypeFactory<AbstractAction>>>)cmbAction.SelectedItem;
            var selectedTypeFactory = selected.ItemValue.Value;

            if (Action == null || Action.GetType().FullName != selectedTypeFactory.FullTypeName)
                Action = CoreAction.MakeAction(selectedTypeFactory);

            if (options != null)
                options.Setup(Action);

            ActionChanged?.Invoke(Action);
        }

        public bool CanMappingSave(AbstractMappedOption mappedOption)
        {
            if (options != null)
                return options.CanMappingSave(mappedOption.Action);

            return false;
        }

        public void SelectAction(AbstractAction action)
        {
            selectedAction = action;
            
            if (!isLoaded)
                return;

            var selected = cmbAction.Items.Cast<ImageComboBoxItem<KeyValuePair<ActionAttribute, MappingTypeFactory<AbstractAction>>>>();
            var actionFullTypeName = action.GetType().FullName;
            var selectedType = selected.FirstOrDefault(x => x.ItemValue.Value.FullTypeName == actionFullTypeName);
            cmbAction.SelectedItem = selectedType;
        }

        private void LoadActions()
        {
            var actionTypeFactories = ActionsRepository.GetAllActions(IsTopLevel);

            foreach (var keyValuePair in actionTypeFactories)
            {
                var mappingControlFactory = MappingControlRepository.GetMappingControlFactory(keyValuePair.Value.FullTypeName);
                var customImage = mappingControlFactory.ImageResourceName;
                var image = Program.ResourceBitmapFromName(customImage ?? "error");
                var item = new ImageComboBoxItem<KeyValuePair<ActionAttribute, MappingTypeFactory<AbstractAction>>>(keyValuePair, new Bitmap(image), "Key");

                cmbAction.Items.Add(item);
            }

            cmbAction.SelectedIndex = -1;

            isLoaded = true;

            if(selectedAction != null)
                SelectAction(selectedAction);
        }
        
        private void cmbAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoaded)
                return;
            
            var options = MappingForm.BuildOptionsForComboBox<ActionAttribute, AbstractAction>(cmbAction, pnlActionOptions);

            if (options != null)
            {
                this.options = options as IActionOptionsControl;

                if (this.options != null)
                {
                    if(selectedAction != null)
                        this.options.Select(selectedAction);

                    this.options.OptionsChanged += (s, _) => BuildAction();
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
