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

        internal abstract void RegisterScriptingEnum(Type enumType);
        internal abstract void RegisterScriptingMethod(string functionName, BaseAction instance, MethodInfo method);

        internal abstract bool TryConvertParameterToDouble(object parameter, out double result);
        internal abstract bool TryConvertParameterToLong(object parameter, out long result);
        internal abstract bool TryConvertParameterToByte(object parameter, out byte result);
        internal abstract bool TryConvertParameterToCallback(object parameter, out Action callback);
        internal abstract bool TryConvertParameterToPointer(object v, out IntPtr handle);

        public override string GetNameDisplay()
        {
            // Truncate the script to be no more than 50 characters
            string truncatedScript = Script.Length > 47 ? Script.Substring(0, 47) + "..." : Script;

            return Name.Replace("{0}", truncatedScript);
        }

        internal override void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
        {
            base.OnStartListening(listener, ref otherActions);

            var actionTypes = ActionAttribute.GetAllActions();

            // Register all scripting available action methods and enumerations
            foreach (var pair in actionTypes)
            {
                var actionType = pair.Key;
                var exposesEnumerationAttributes = (ExposesScriptingEnumerationAttribute[])actionType.GetCustomAttributes(typeof(ExposesScriptingEnumerationAttribute), false);

                foreach (var scriptingEnumAttribute in exposesEnumerationAttributes)
                {
                    RegisterScriptingEnum(scriptingEnumAttribute.ExposedEnumeration);
                }

                foreach (var methodInfo in actionType.GetMethods())
                {
                    var exposesMethodAttributes = (ExposesScriptingMethodAttribute[])methodInfo.GetCustomAttributes(typeof(ExposesScriptingMethodAttribute), false);
                    
                    if (exposesMethodAttributes.Length == 0)
                        continue;

                    foreach (var scriptingMethodAttribute in exposesMethodAttributes)
                    {
                        var instance = MakeAction(actionType);

                        RegisterScriptingMethod(
                            scriptingMethodAttribute.FunctionName,
                            instance,
                            methodInfo);
                    }
                }
            }
        }
    }
}
