using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Mapping;
using Key2Joy.Mapping.Actions;
using Key2Joy.Plugins;

namespace Key2Joy.Gui.Mapping;

public partial class ActionControl : UserControl
{
    public AbstractAction Action { get; private set; }
    public event Action<AbstractAction> ActionChanged;
    public bool IsTopLevel { get; set; }

    private bool isLoaded = false;
    private IActionOptionsControl options;
    private AbstractAction selectedAction = null;

    public ActionControl() => this.InitializeComponent();

    private void BuildAction()
    {
        if (this.cmbAction.SelectedItem == null)
        {
            ActionChanged?.Invoke(null);
            return;
        }

        var selected = (ImageComboBoxItem<KeyValuePair<ActionAttribute, MappingTypeFactory<AbstractAction>>>)this.cmbAction.SelectedItem;
        var selectedTypeFactory = selected.ItemValue.Value;

        if (this.Action == null || this.Action.GetType().FullName != selectedTypeFactory.FullTypeName)
        {
            this.Action = CoreAction.MakeAction(selectedTypeFactory);
        }

        this.options?.Setup(this.Action);

        ActionChanged?.Invoke(this.Action);
    }

    public bool CanMappingSave(AbstractMappedOption mappedOption)
    {
        if (this.options != null)
        {
            return this.options.CanMappingSave(mappedOption.Action);
        }

        return false;
    }

    public void SelectAction(AbstractAction action)
    {
        this.selectedAction = action;

        if (!this.isLoaded)
        {
            return;
        }

        var selected = this.cmbAction.Items.Cast<ImageComboBoxItem<KeyValuePair<ActionAttribute, MappingTypeFactory<AbstractAction>>>>();
        var actionFullTypeName = MappingTypeHelper.GetTypeFullName(
            ActionsRepository.GetAllActions(),
            action
        );
        actionFullTypeName = MappingTypeHelper.EnsureSimpleTypeName(actionFullTypeName);

        var selectedType = selected.FirstOrDefault(x => x.ItemValue.Value.FullTypeName == actionFullTypeName);
        this.cmbAction.SelectedItem = selectedType;
    }

    private void LoadActions()
    {
        var actionTypeFactories = ActionsRepository.GetAllActions(this.IsTopLevel);

        foreach (var keyValuePair in actionTypeFactories)
        {
            var mappingControlFactory = MappingControlRepository.GetMappingControlFactory(keyValuePair.Value.FullTypeName);

            if (mappingControlFactory == null)
            {
                continue;
            }

            var customImage = mappingControlFactory.ImageResourceName;
            var image = Program.ResourceBitmapFromName(customImage ?? "error");
            ImageComboBoxItem<KeyValuePair<ActionAttribute, MappingTypeFactory<AbstractAction>>> item = new(keyValuePair, new Bitmap(image), "Key");

            this.cmbAction.Items.Add(item);
        }

        this.cmbAction.SelectedIndex = -1;

        this.isLoaded = true;

        if (this.selectedAction != null)
        {
            this.SelectAction(this.selectedAction);
        }
    }

    private void CmbAction_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!this.isLoaded)
        {
            return;
        }

        var options = MappingForm.BuildOptionsForComboBox<ActionAttribute, AbstractAction>(this.cmbAction, this.pnlActionOptions);

        if (options != null)
        {
            this.options = options as IActionOptionsControl;

            if (this.options != null)
            {
                if (this.selectedAction != null)
                {
                    this.options.Select(this.selectedAction);
                }

                this.options.OptionsChanged += (s, _) => this.BuildAction();
            }
        }

        this.BuildAction();

        this.selectedAction = null;
        this.PerformLayout();
    }

    private void ActionControl_Load(object sender, EventArgs e) => this.LoadActions();
}
