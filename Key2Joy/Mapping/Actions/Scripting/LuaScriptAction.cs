using Key2Joy.Util;
using Newtonsoft.Json;
using NLua;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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
    internal class LuaScriptAction : BaseScriptActionWithEnvironment<Lua>
    {
        public LuaScriptAction(string name, string description)
            : base(name, description)
        {
            ImageResource = "Lua";
        }

        internal override async Task Execute(IInputBag inputBag)
        {
            try
            {
                var source = "Key2Joy.Script.Inline";
                if (IsScriptPath)
                    source = Script;

                lock (LockObject)
                    environment.DoString(GetExecutableScript(), Script);
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
            environment.NewTable(enumType.Name);

            for (int i = 0; i < names.Length; i++)
            {
                string path = enumType.Name + "." + names[i];
                environment.SetObjectToPath(path, Enum.GetValues(enumType).GetValue(i));
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

                if(environment.GetTable(path) == null)
                    environment.NewTable(path);
            }

            var parameters = method.GetParameters();
            var paramDebug = string.Join(", ", parameters
                .Select(p => $"{p.ParameterType.Name} {p.Name}")
                .ToArray());
            Output.WriteLine(Output.OutputModes.Verbose, $"lua.RegisterFunction({functionName},{instance},{method.Name}({paramDebug}):{method.ReturnType})");

            //if (parameters.Any(p => p.ParameterType.IsList()))
            //{
            //    var oldMethod = method;

            //    // Create a wrapper method with the same parameters
            //    method = new DynamicMethod(method.Name, method.ReturnType, parameters.Select(p => p.ParameterType.IsList() ? typeof(LuaTable) : p.ParameterType).ToArray());

            //}

            environment.RegisterFunction(
                functionName,
                instance,
                method);
        }

        internal override Lua MakeEnvironment()
        {
            return new Lua();
        }

        public override void RegisterEnvironmentObjects()
        {
            environment.RegisterFunction("print", this, typeof(LuaScriptAction).GetMethod(nameof(Print), new[] { typeof(object[]) }));
            environment.RegisterFunction("Print", this, typeof(LuaScriptAction).GetMethod(nameof(Print), new[] { typeof(object[]) }));

            base.RegisterEnvironmentObjects();
        }

        internal override void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
        {
            base.OnStartListening(listener, ref otherActions);
        }

        internal override void OnStopListening(TriggerListener listener)
        {
            base.OnStopListening(listener);

            environment.Dispose();
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
                IsScriptPath = IsScriptPath,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
