using Jint;
using Jint.Native;
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
    internal abstract class BaseScriptAction : BaseAction
    {
        [JsonProperty]
        public string Script { get; set; }

        [JsonProperty]
        public bool IsScriptPath { get; set; }

        public BaseScriptAction(string name, string description)
            : base(name, description)
        { }

        // TODO: Keep this here and output to a source the user can see (send errors there too)
        //public void Print(string message)
        //{
        //    System.Diagnostics.Debug.WriteLine(message);
        //}

        public override string GetNameDisplay()
        {
            // Truncate the script to be no more than 50 characters
            string truncatedScript = Script.Length > 47 ? Script.Substring(0, 47) + "..." : Script;

            return Name.Replace("{0}", truncatedScript);
        }

        public static bool TryConvertParameterToLong(object parameter, out long result)
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
            else if (parameter is int)
            {
                result = (int)parameter;
                return true;
            }
            else if (parameter is string)
            {
                return long.TryParse((string)parameter, out result);
            }
            
            result = 0;
            return false;
        }

        internal static bool TryConvertParameterToCallback(object parameter, out Action callback)
        {
            // TODO: Move this to specific script implementation somehow
            if (parameter is NLua.LuaFunction luaCallback)
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

            // TODO: Move this to specific script implementation somehow 
            if (parameter is Delegate @delegate)
            {
                callback = () =>
                {
                    try
                    {
                        var thisArg = JsValue.Undefined;
                        var arguments = new JsValue[] {};
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

            throw new NotImplementedException("TODO: Support other callbacks");
            
            callback = null;
            return false;
        }
    }
}
