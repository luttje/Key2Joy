using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Contracts.Mapping.Actions;

public abstract class AbstractAction : AbstractMappingAspect
{
    [JsonIgnore]
    public bool IsStarted { get; set; }

    protected AbstractTriggerListener Listener { get; set; }

    protected IList<AbstractAction> OtherActions;

    /// <summary>
    /// This method is called to execute the action. It can optionally be override, by
    /// default it throws a NotImplementedException, which is useful for actions that
    /// only have a script implementation (on other methods).
    /// </summary>
    /// <param name="inputBag"></param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public virtual Task Execute(AbstractInputBag inputBag = null) => throw new System.NotImplementedException();

    public AbstractAction(string name)
        : base(name) { }

    public virtual void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
        => this.SetStartData(listener, ref otherActions);

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
