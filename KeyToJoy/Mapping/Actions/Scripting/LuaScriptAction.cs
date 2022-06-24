﻿using Newtonsoft.Json;
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
        OptionsUserControl = typeof(LuaScriptActionControl),
        NameFormat = "Lua Script: {0}"
    )]
    internal class LuaScriptAction : BaseScriptAction
    {
        private Lua lua;

        public LuaScriptAction(string name, string description)
            : base(name, description)
        { }

        internal override async Task Execute(InputBag inputBag)
        {
            try
            {
                lua.DoString(Script, "KeyToJoy.Script.Inline");
            }
            catch (NLua.Exceptions.LuaScriptException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Print(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        internal override void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
        {
            base.OnStartListening(listener, ref otherActions);

            lua = new Lua();
            
            lua.RegisterFunction("print", this, typeof(LuaScriptAction).GetMethod(nameof(Print), new[] { typeof(string) }));

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