using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Mapping.Actions.Logic;

namespace Key2Joy.Gui.Mapping;

[MappingControl(
    ForType = typeof(SequenceAction),
    ImageResourceName = "text_list_numbers"
)]
public partial class SequenceActionControl : UserControl, IActionOptionsControl
{
    public event EventHandler OptionsChanged;

    private readonly List<AbstractAction> childActions;

    public SequenceActionControl()
    {
        this.InitializeComponent();

        this.childActions = new List<AbstractAction>();
    }

    public void Select(object action)
    {
        var thisAction = (SequenceAction)action;

        foreach (var childAction in thisAction.ChildActions)
        {
            // Clone so we don't modify the action in a profile
            this.AddChildAction((AbstractAction)childAction.Clone());
        }
    }

    public void Setup(object action)
    {
        var thisAction = (SequenceAction)action;
        thisAction.ChildActions.Clear();

        foreach (var childAction in this.childActions)
        {
            thisAction.ChildActions.Add(childAction);
        }
    }

    public bool CanMappingSave(object action) => true;

    private void AddChildAction(AbstractAction action)
    {
        this.childActions.Add(action);
        this.lstActions.Items.Add(action);
    }

    private void RemoveChildAction(AbstractAction action)
    {
        var index = this.lstActions.Items.IndexOf(action);

        this.childActions.Remove(action);
        this.lstActions.Items.Remove(action);

        if (this.lstActions.Items.Count > 0)
        {
            if (index >= this.lstActions.Items.Count)
            {
                index = this.lstActions.Items.Count - 1;
            }
            this.lstActions.SelectedIndex = index;
        }
    }

    private void BtnAdd_Click(object sender, EventArgs e)
    {
        this.AddChildAction((AbstractAction)this.actionControl.Action.Clone());
        OptionsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void BtnRemove_Click(object sender, EventArgs e)
    {
        this.RemoveChildAction((AbstractAction)this.lstActions.SelectedItem);
        OptionsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void LstActions_SelectedIndexChanged(object sender, EventArgs e) => this.btnRemove.Enabled = this.lstActions.SelectedIndex > -1;

    private void ActionControl_ActionChanged(AbstractAction action) => this.btnAdd.Enabled = action != null;
}
