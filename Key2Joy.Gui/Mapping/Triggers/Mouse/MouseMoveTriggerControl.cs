using System;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Mapping;
using Key2Joy.Mapping.Triggers;
using Key2Joy.Mapping.Triggers.Mouse;

namespace Key2Joy.Gui.Mapping;

[MappingControl(
    ForType = typeof(MouseMoveTrigger),
    ImageResourceName = "mouse"
)]
public partial class MouseMoveTriggerControl : UserControl, ITriggerOptionsControl
{
    public event EventHandler OptionsChanged;

    public MouseMoveTriggerControl()
    {
        this.InitializeComponent();

        this.cmbMouseDirection.DataSource = Enum.GetValues(typeof(AxisDirection));
    }

    public void Select(AbstractTrigger trigger)
    {
        var thisTrigger = (MouseMoveTrigger)trigger;

        this.cmbMouseDirection.SelectedItem = thisTrigger.AxisBinding;
    }

    public void Setup(AbstractTrigger trigger)
    {
        var thisTrigger = (MouseMoveTrigger)trigger;

        thisTrigger.AxisBinding = (AxisDirection)this.cmbMouseDirection.SelectedItem;
    }

    public bool CanMappingSave(AbstractTrigger trigger) => true;
    
    private void CmbMouseDirection_SelectedIndexChanged(object sender, EventArgs e) => OptionsChanged?.Invoke(this, EventArgs.Empty);
}
