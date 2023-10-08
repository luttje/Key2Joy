using System.Collections.Generic;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Contracts.Mapping.Actions;

public abstract class AbstractAction : AbstractMappingAspect
{
    public bool IsStarted { get; set; }

    protected AbstractTriggerListener Listener { get; set; }

    protected IList<AbstractAction> OtherActions;

    public virtual async Task Execute(AbstractInputBag inputBag = null)
    { }

    public virtual string GetNameDisplay() => this.Name;

    public AbstractAction(string name)
        : base(name) { }

    public virtual void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions) => this.SetStartData(listener, ref otherActions);

    public virtual void OnStopListening(AbstractTriggerListener listener)
    {
        this.IsStarted = false;

        this.Listener = null;
    }

    public void SetStartData(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
    {
        this.IsStarted = true;
        this.Listener = listener;
        this.OtherActions = otherActions;
    }
}
