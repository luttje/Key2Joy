using Jint;
using Jint.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using KeyToJoy.Util;

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

        public void Print(object message)
        {
            System.Diagnostics.Debug.WriteLine(message.ToString());
        }

        internal override void RegisterScriptingEnum(Type enumType)
        {
            var enumNames = Enum.GetNames(enumType);

            // TODO: Probably a better way to do this
            engine.Execute(
                enumType.Name + " = {" +
                string.Join(", ", enumNames.Select((name, index) => $"{name}: {(int)Enum.Parse(enumType, name)}")) +
                "    }"
            );

            engine.Execute($"Print(JSON.stringify({enumType.Name}))");
        }

        internal override void RegisterScriptingMethod(string functionName, BaseAction instance, MethodInfo method)
        {
            engine.SetValue(
                functionName,
                method.CreateDelegate(instance));
        }

        internal override void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
        {
            engine = new Engine();
            engine.SetValue("Print", new Action<object>(Print));

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
