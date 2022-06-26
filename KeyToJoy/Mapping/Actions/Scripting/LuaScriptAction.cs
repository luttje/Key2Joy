using Newtonsoft.Json;
using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    [Action(
        Description = "Lua Script Action",
        OptionsUserControl = typeof(ScriptActionControl),
        OptionsUserControlParams = new[] { "Lua" },
        NameFormat = "Lua Script: {0}"
    )]
    internal class LuaScriptAction : BaseScriptAction
    {
        private Lua lua;

        public LuaScriptAction(string name, string description)
            : base(name, description)
        {
            ImageResource = "Lua";
        }

        internal override async Task Execute(InputBag inputBag)
        {
            try
            {
                if (IsScriptPath)
                {
                    lua.DoFile(Script);
                    return;
                }

                lua.DoString(Script, "KeyToJoy.Script.Inline");
            }
            catch (NLua.Exceptions.LuaScriptException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Print(object message)
        {
            System.Diagnostics.Debug.WriteLine(message.ToString());
        }

        internal override void RegisterScriptingEnum(Type enumType)
        {
            var enumNames = Enum.GetNames(enumType);

            lua.DoString(
                enumType.Name + " = {" +
                string.Join(", ", enumNames.Select((name, index) => $"{name} = {(int)Enum.Parse(enumType, name)}")) +
                "}"
            );
        }

        internal override void RegisterScriptingMethod(string functionName, BaseAction instance, MethodInfo method)
        {
            lua.RegisterFunction(
                functionName,
                instance,
                method);
        }

        internal override void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
        {
            lua = new Lua();
            lua.RegisterFunction("print", this, typeof(LuaScriptAction).GetMethod(nameof(Print), new[] { typeof(object) }));

            base.OnStartListening(listener, ref otherActions);
        }

        internal override void OnStopListening(TriggerListener listener)
        {
            base.OnStopListening(listener);

            lua.Dispose();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LuaScriptAction action))
                return false;

            return action.Name == Name
                && action.Script == Script;
        }

        public override object Clone()
        {
            return new LuaScriptAction(Name, description)
            {
                Script = Script,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
