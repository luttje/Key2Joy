using System;
using System.Text.Json.Serialization;

namespace Key2Joy.Contracts.Mapping.Triggers;

public abstract class AbstractTrigger : AbstractMappingAspect
{
    public event EventHandler<TriggerExecutingEventArgs> Executing;

    [JsonIgnore]
    public AbstractInputBag LastInputBag { get; protected set; }

    [JsonIgnore]
    public DateTime LastActivated { get; protected set; }

    [JsonIgnore]
    public bool ExecutedLastActivation { get; protected set; }

    public AbstractTrigger(string name)
        : base(name) { }

    /// <summary>
    /// Must return a singleton listener that will listen for triggers.
    ///
    /// When the user starts their mappings, this listener will be given each relevant mapping to look for.
    /// </summary>
    /// <returns>Singleton trigger listener</returns>
    public abstract AbstractTriggerListener GetTriggerListener();

    /// <summary>
    /// Through this the trigger can decide if it wants to execute. By default it calls the
    /// <see cref="Executing"/> event and executes if it's not handled.
    /// This method is only called after the trigger listener has asked which mapped options
    /// should even be considered to excecute through <see cref="AbstractTriggerListener.TriggerActivating"/>.
    /// </summary>
    /// <returns></returns>
    public virtual bool GetShouldExecute()
    {
        TriggerExecutingEventArgs eventArgs = new();

        Executing?.Invoke(this, eventArgs);

        return !eventArgs.Handled;
    }

    /// <summary>
    /// Called when the trigger has been considered for triggering. By default it just registers
    /// the last activation time and input bag.
    /// </summary>
    /// <param name="inputBag"></param>
    /// <param name="executed"></param>
    public virtual void DoActivate(AbstractInputBag inputBag, bool executed = false)
    {
        this.LastActivated = DateTime.Now;
        this.LastInputBag = inputBag;
        this.ExecutedLastActivation = executed;
    }
}
