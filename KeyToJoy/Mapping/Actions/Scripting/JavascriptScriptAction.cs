using Jint;
using Jint.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using KeyToJoy.Util;
using System.Text;
using Jint.Native.Object;
using Jint.Runtime.Interop;

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
        {
            ImageResource = "JS";
        }

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

        internal override void RegisterScriptingEnum(Type enumType)
        {
            var enumNames = Enum.GetNames(enumType);

            // TODO: Probably a better way to register enums
            engine.Execute(
                enumType.Name + " = {" +
                string.Join(", ", enumNames.Select((name, index) =>
                {
                    var underlyingType = Enum.GetUnderlyingType(enumType);
                    var e = Enum.Parse(enumType, name);

                    if (underlyingType == typeof(int))
                        return $"{name}: {(int)e}";
                    else if (underlyingType == typeof(short))
                        return $"{name}: {(short)e}";
                    else
                        throw new NotImplementedException("Enumeration was of unimplemented underlying datatype: " + underlyingType);
                })) +
                "    }"
            );

            engine.Execute($"Print(JSON.stringify({enumType.Name}))");
        }

        internal override void RegisterScriptingMethod(string functionName, BaseAction instance, MethodInfo method)
        {
            var parents = functionName.Split('.');
            var @delegate = new DelegateWrapper(engine, method.CreateDelegate(instance));

            if (parents.Length > 1)
            {
                ObjectInstance currentObject = engine.Global;

                for (int i = 0; i < parents.Length; i++)
                {
                    if(i != parents.Length - 1)
                    {
                        var childObject = new ObjectInstance(engine);
                        currentObject.FastAddProperty(parents[i], childObject, false, true, true);
                        currentObject = childObject;
                    }
                    else
                    {
                        currentObject.FastAddProperty(parents[i], @delegate, false, true, true);
                    }
                }

                return;
            }

            engine.SetValue(
                functionName,
                @delegate);
        }

        internal override void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
        {
            engine = new Engine();
            engine.SetValue("Print", new Action<object[]>(Print));

            base.OnStartListening(listener, ref otherActions);
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
