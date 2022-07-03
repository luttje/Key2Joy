using Newtonsoft.Json;
using NLua;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
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

                lua.DoString(Script, "Key2Joy.Script.Inline");
            }
            catch (NLua.Exceptions.LuaScriptException e)
            {
                Output.WriteLine(e);
            }
        }

        internal override void RegisterScriptingEnum(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("The type must be an enumeration!");

            string[] names = Enum.GetNames(enumType);
            lua.NewTable(enumType.Name);

            for (int i = 0; i < names.Length; i++)
            {
                string path = enumType.Name + "." + names[i];
                lua.SetObjectToPath(path, Enum.Parse(enumType, names[i]));
            }
        }

        internal override void RegisterScriptingMethod(string functionName, BaseAction instance, MethodInfo method)
        {
            var parents = functionName.Split('.');

            if (parents.Length > 1)
            {
                var currentPath = new StringBuilder();
                
                for (int i = 0; i < parents.Length - 1; i++)
                {
                    if (i > 0)
                        currentPath.Append('.');

                    currentPath.Append(parents[i]);
                }

                var path = currentPath.ToString();

                if(lua.GetTable(path) == null)
                    lua.NewTable(path);
            }

            var paramDebug = string.Join(", ", method.GetParameters()
                .Select(p => $"{p.ParameterType.Name} {p.Name}")
                .ToArray());
            Output.WriteLine(Output.OutputModes.Verbose, $"lua.RegisterFunction({functionName},{instance},{method.Name}({paramDebug}):{method.ReturnType})");
            
            lua.RegisterFunction(
                functionName,
                instance,
                method);
        }

        internal override void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
        {
            lua = new Lua();
            lua.RegisterFunction("print", this, typeof(LuaScriptAction).GetMethod(nameof(Print), new[] { typeof(object[]) }));
            lua.RegisterFunction("Print", this, typeof(LuaScriptAction).GetMethod(nameof(Print), new[] { typeof(object[]) }));

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
