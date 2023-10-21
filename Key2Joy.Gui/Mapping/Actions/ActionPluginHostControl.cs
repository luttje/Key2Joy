using System;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Plugins;

namespace Key2Joy.Gui.Mapping;

public partial class ActionPluginHostControl : UserControl, IActionOptionsControl
{
    public event EventHandler OptionsChanged;

    private readonly IActionOptionsControl pluginControlWithOptions;

    public ActionPluginHostControl() => this.InitializeComponent();

    public ActionPluginHostControl(ElementHostProxy pluginUserControl)
        : this()
    {
        this.pluginControlWithOptions = pluginUserControl;

        this.Padding = new Padding(0, 5, 0, 5);
        var desiredHeight = pluginUserControl.GetDesiredHeight() + this.Padding.Vertical;
        this.Height = desiredHeight + this.Padding.Vertical;

        this.Controls.Add(pluginUserControl);
        pluginUserControl.Dock = DockStyle.Fill;
        pluginUserControl.PerformLayout();

        ActionOptionsChangeListener listener = new(this.pluginControlWithOptions);
        listener.OptionsChanged += (s, e) => OptionsChanged?.Invoke(s, e);
    }

    public bool CanMappingSave(AbstractAction action) => this.pluginControlWithOptions.CanMappingSave(action);

    public void Select(AbstractAction action) => this.pluginControlWithOptions.Select(action);

    public void Setup(AbstractAction action) => this.pluginControlWithOptions.Setup(action);
}
