using System;
using System.Collections.Generic;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Actions.Scripting
{
    public abstract class BaseScriptActionWithEnvironment<TEnvironment> : BaseScriptAction
    {
        protected TEnvironment environment;

        public BaseScriptActionWithEnvironment(string name)
            : base(name)
        { }

        public TEnvironment SetupEnvironment()
        {
            if (this.environment is not null and IDisposable disposableEnvironment)
            {
                disposableEnvironment.Dispose();
            }

            this.environment = this.MakeEnvironment();
            this.RegisterEnvironmentObjects();

            return this.environment;
        }

        public void ReplaceEnvironment(TEnvironment environment)
        {
            this.environment = environment;
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

                foreach (var exposedEnumeration in ExposedEnumerations)
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
            foreach (var action in otherActions)
            {
                if (action.GetType() == this.GetType()
                    && action.IsStarted)
                {
                    // Use an existing environment if it exists
                    var otherAction = (BaseScriptActionWithEnvironment<TEnvironment>)action;
                    this.environment = otherAction.environment;
                    return;
                }
            }

            base.OnStartListening(listener, ref otherActions);

            this.SetupEnvironment();
        }
    }
}
