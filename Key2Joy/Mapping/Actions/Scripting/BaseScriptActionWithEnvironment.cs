using Key2Joy.Contracts.Mapping;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Key2Joy.Mapping
{
    public abstract class BaseScriptActionWithEnvironment<TEnvironment> : BaseScriptAction
    {
        protected TEnvironment environment;

        public BaseScriptActionWithEnvironment(string name, string description)
            : base(name, description)
        { }

        public TEnvironment SetupEnvironment()
        {
            environment = MakeEnvironment();
            RegisterEnvironmentObjects();

            return environment;
        }

        public void ReplaceEnvironment(TEnvironment environment)
        {
            this.environment = environment;
        }

        public abstract TEnvironment MakeEnvironment();

        public virtual void RegisterEnvironmentObjects()
        {
            var actionTypes = ActionsRepository.GetAllActions();
            cachedFile = null;

            // Register all scripting available action methods and enumerations
            foreach (var pair in actionTypes)
            {
                var actionTypeFactory = pair.Value;

                foreach (var exposedEnumeration in ExposedEnumerations)
                {
                    RegisterScriptingEnum(exposedEnumeration);
                }

                foreach (var exposedMethods in actionTypeFactory.ExposedMethods)
                {
                    var instance = IsStarted ? 
                        MakeStartedAction(actionTypeFactory) 
                        : MakeAction(actionTypeFactory);

                    RegisterScriptingMethod(
                        exposedMethods,
                        instance);
                }
            }
        }

        public override void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
        {
            foreach (var action in otherActions)
            {
                if (action.GetType() == GetType()
                    && action.IsStarted)
                {
                    // Use an existing environment if it exists
                    var otherAction = (BaseScriptActionWithEnvironment<TEnvironment>)action;
                    environment = otherAction.environment;
                    return;
                }
            }

            base.OnStartListening(listener, ref otherActions);

            SetupEnvironment();
        }
    }
}
