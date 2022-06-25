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

        internal override bool TryConvertParameterToByte(object parameter, out byte result)
        {
            if (parameter is byte)
            {
                result = (byte)parameter;
                return true;
            }

            var type = parameter.GetType();
            throw new NotImplementedException("TODO: Support other types");

            result = 0;
            return false;
        }

        internal override bool TryConvertParameterToDouble(object parameter, out double result)
        {
            if (parameter is double || parameter is long)
            {
                result = (double)parameter;
                return true;
            }

            var type = parameter.GetType();
            throw new NotImplementedException("TODO: Support other types (I don't think NLua will give us anything other than double)");

            result = 0;
            return false;
        }

        internal override bool TryConvertParameterToLong(object parameter, out long result)
        {
            if (parameter is long)
            {
                result = (long)parameter;
                return true;
            }
            else if (parameter is double)
            {
                result = Convert.ToInt64(parameter);
                return true;
            }

            var type = parameter.GetType();
            throw new NotImplementedException("TODO: Support other longs");

            result = 0;
            return false;
        }

        internal override bool TryConvertParameterToCallback(object parameter, out Action callback)
        {
            if (parameter is LuaFunction luaCallback)
            {
                callback = () =>
                {
                    try
                    {
                        luaCallback.Call();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                    }
                };
                return true;
            }

            var type = parameter.GetType();
            throw new NotImplementedException("TODO: Support other callbacks");

            callback = null;
            return false;
        }

        internal override bool TryConvertParameterToPointer(object parameter, out IntPtr result)
        {
            if (parameter is IntPtr)
            {
                result = (IntPtr)parameter;
                return true;
            }

            var type = parameter.GetType();
            throw new NotImplementedException("TODO: Support other types");

            result = IntPtr.Zero;
            return false;
        }

        internal override void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
        {
            base.OnStartListening(listener, ref otherActions);

            lua = new Lua();
            
            lua.RegisterFunction("print", this, typeof(LuaScriptAction).GetMethod(nameof(Print), new[] { typeof(object) }));

            var actionTypes = ActionAttribute.GetAllActions();

            // Register all actions as lua functions
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

                        lua.DoString(
                            enumType.Name + " = {" +
                            string.Join(", ", enumNames.Select((name, index) => $"{name} = {(int)Enum.Parse(enumType, name)}")) +
                            "    }"
                        );
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

                lua.RegisterFunction(
                    scriptableActionAttribute.FunctionName,
                    wrapper,
                    wrapper.GetWrapperMethod());
            }
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
