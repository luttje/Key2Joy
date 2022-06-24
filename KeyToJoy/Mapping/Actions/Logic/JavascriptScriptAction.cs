using Jint;
using KeyToJoy.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    [Action(
        Description = "Javascript Action",
        OptionsUserControl = typeof(JavascriptActionControl),
        NameFormat = "Javascript Script: {0}"
    )]
    internal class JavascriptAction : BaseScriptAction
    {
        private Engine engine;

        public JavascriptAction(string name, string description)
            : base(name, description)
        { }

        internal override async Task Execute(InputBag inputBag)
        {
            engine.Execute(Script);
        }

        public void Print(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        internal override void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
        {
            base.OnStartListening(listener, ref otherActions);

            engine = new Engine();
            engine.SetValue("print", new Action<string>(Print));

            var actionTypes = ActionAttribute.GetAllActions();

            // Register all actions as js functions
            foreach (var pair in actionTypes)
            {
                var actionType = pair.Key;
                var actionAttribute = pair.Value;

                if (!(actionAttribute is IScriptable scriptableActionAttribute))
                    continue;

                if (scriptableActionAttribute.ExposesEnumerations != null)
                {
                    foreach (var enumType in scriptableActionAttribute.ExposesEnumerations)
                    {
                        var enumNames = Enum.GetNames(enumType);

                        // TODO: Probably a better way to do this
                        engine.Execute(
                            enumType.Name + " = {" +
                            string.Join(", ", enumNames.Select((name, index) => $"{name}: {(int)Enum.Parse(enumType, name)}")) +
                            "    }"
                        );

                        engine.Execute($"print(JSON.stringify({enumType.Name}))");
                    }
                }

                if (scriptableActionAttribute.FunctionMethodName == null)
                    continue;
                
                var instance = MakeAction(actionType);
                var method = actionType.GetMethod(
                    scriptableActionAttribute.FunctionMethodName,
                    new[] { typeof(object[]) });

                engine.SetValue(
                    scriptableActionAttribute.FunctionName,
                    method.CreateDelegate(instance));
            }
        }
        
        internal override void OnStopListening(TriggerListener listener)
        {
            base.OnStopListening(listener);

            engine = null;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is JavascriptAction action))
                return false;

            return action.Name == Name
                && action.Script == Script;
        }

        public override object Clone()
        {
            return new JavascriptAction(Name, description)
            {
                Script = Script,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
