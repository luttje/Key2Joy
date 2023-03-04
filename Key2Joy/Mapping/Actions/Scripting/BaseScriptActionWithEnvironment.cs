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
            var actionTypes = ActionAttribute.GetAllActions();
            cachedFile = null;

            // Register all scripting available action methods and enumerations
            foreach (var pair in actionTypes)
            {
                var actionType = pair.Key;
                var exposesEnumerationAttributes = (ExposesScriptingEnumerationAttribute[])actionType.GetCustomAttributes(typeof(ExposesScriptingEnumerationAttribute), false);

                foreach (var scriptingEnumAttribute in exposesEnumerationAttributes)
                {
                    RegisterScriptingEnum(scriptingEnumAttribute.ExposedEnumeration);
                }

                foreach (var methodInfo in actionType.GetMethods())
                {
                    var exposesMethodAttributes = (ExposesScriptingMethodAttribute[])methodInfo.GetCustomAttributes(typeof(ExposesScriptingMethodAttribute), false);

                    if (exposesMethodAttributes.Length == 0)
                        continue;

                    foreach (var scriptingMethodAttribute in exposesMethodAttributes)
                    {
                        var instance = IsStarted ? 
                            MakeStartedAction(actionType) 
                            : MakeAction(actionType);

                        RegisterScriptingMethod(
                            scriptingMethodAttribute.FunctionName,
                            instance,
                            methodInfo);
                    }
                }
            }
        }

        public override void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
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
