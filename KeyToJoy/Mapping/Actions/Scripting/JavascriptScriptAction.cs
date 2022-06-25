using Jint;
using Jint.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    [Action(
        Description = "Javascript Action",
        OptionsUserControl = typeof(ScriptActionControl),
        OptionsUserControlParams = new[] { "Javascript" },
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
            if (IsScriptPath) 
            {
                string source = System.IO.File.ReadAllText(Script);
                engine.Execute(source);
                return;
            }

            engine.Execute(Script);
        }

        public void Print(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        internal override bool TryConvertParameterToDouble(object parameter, out double result)
        {
            if (parameter is double)
            {
                result = (double)parameter;
                return true;
            }

            throw new NotImplementedException("TODO: Support other types (I don't think Jint will give us anything other than double)");

            result = 0;
            return false;
        }

        internal override bool TryConvertParameterToLong(object parameter, out long result)
        {
            if (parameter is double)
            {
                result = Convert.ToInt64(parameter);
                return true;
            }

            throw new NotImplementedException("TODO: Support other types (I don't think Jint will give us anything other than double)");

            result = 0;
            return false;
        }

        internal override bool TryConvertParameterToCallback(object parameter, out Action callback)
        {
            if (parameter is Delegate @delegate)
            {
                callback = () =>
                {
                    try
                    {
                        var thisArg = JsValue.Undefined;
                        var arguments = new JsValue[] { };
                        @delegate.DynamicInvoke(thisArg, arguments);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                    }
                };
                return true;
            }

            var test = parameter.GetType();

            throw new NotImplementedException("TODO: Support other callback types (do they exist in Jint?)");

            callback = null;
            return false;
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
                var wrapper = new ScriptCallWrapper(
                    this,
                    instance, 
                    actionType, 
                    scriptableActionAttribute.FunctionMethodName);

                engine.SetValue(
                    scriptableActionAttribute.FunctionName,
                    wrapper.GetWrapperDelegate());
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
