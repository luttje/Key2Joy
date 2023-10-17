using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Actions.Scripting;

public abstract class BaseScriptActionWithEnvironment<TEnvironment> : BaseScriptAction
{
    protected TEnvironment Environment;
    protected Guid EnvironmentId { get; private set; }

    [JsonIgnore]
    public bool IsRetired { get; private set; }

    public BaseScriptActionWithEnvironment(string name)
        : base(name)
    { }

    public TEnvironment SetupEnvironment()
    {
        if (this.Environment is not null and IDisposable disposableEnvironment)
        {
            disposableEnvironment.Dispose();
        }

        this.EnvironmentId = Guid.NewGuid();
        this.Environment = this.MakeEnvironment();
        this.RegisterEnvironmentObjects();

        // If we're started, find other actions with the same scripting environment and set their environment to this one
        if (this.IsStarted)
        {
            foreach (var otherAction in this.OtherActions)
            {
                if (otherAction is BaseScriptActionWithEnvironment<TEnvironment> scriptAction)
                {
                    scriptAction.IsRetired = false;
                    scriptAction.Environment = this.Environment;
                    scriptAction.EnvironmentId = this.EnvironmentId;
                }
            }
        }

        return this.Environment;
    }

    public void RetireEnvironment() => this.IsRetired = true;

    public override async Task Execute(AbstractInputBag inputBag)
    {
        if (this.IsRetired)
        {
            this.IsRetired = false;
            this.SetupEnvironment();
        }
    }

    public abstract TEnvironment MakeEnvironment();

    public virtual void RegisterEnvironmentObjects()
    {
        var actionTypes = ActionsRepository.GetAllActions();
        this.cachedFile = null;

        // Register all scripting available action methods and enumerations
        foreach (var pair in actionTypes)
        {
            var actionFactory = pair.Value;

            foreach (var exposedEnumeration in ExposedEnumerationRepository.GetAllExposedEnumerations())
            {
                this.RegisterScriptingEnum(exposedEnumeration);
            }

            foreach (var exposedMethods in actionFactory.ExposedMethods)
            {
                var instance = this.IsStarted ?
                    this.MakeStartedAction(actionFactory)
                    : MakeAction(actionFactory);

                this.RegisterScriptingMethod(
                    exposedMethods,
                    instance);
            }
        }
    }

    public override void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
    {
        base.OnStartListening(listener, ref otherActions);

        foreach (var otherAction in otherActions)
        {
            if (otherAction is BaseScriptActionWithEnvironment<TEnvironment> scriptAction
                && scriptAction.IsStarted)
            {
                // Use an existing environment if it exists
                // This is so multiple actions all share the same scripting environment (and thus global variables).
                this.Environment = scriptAction.Environment;
                this.EnvironmentId = scriptAction.EnvironmentId;

                if (this.Environment != null)
                {
                    return;
                }
            }
        }

        this.SetupEnvironment();
    }
}
