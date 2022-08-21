using Jint;
using Jint.Native;
using Key2Joy.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    [Util.ObjectListViewGroup(
        Name = "Scripting",
        Image = "script_code"
    )]
    internal abstract class BaseScriptAction : BaseAction
    {
        [JsonProperty]
        public string Script { get; set; }

        [JsonProperty]
        public bool IsScriptPath { get; set; }
        
        public static BaseScriptAction Instance { get; private set; }

        public BaseScriptAction(string name, string description)
            : base(name, description)
        { }

        internal abstract void RegisterScriptingEnum(Type enumType);
        internal abstract void RegisterScriptingMethod(string functionName, BaseAction instance, MethodInfo method);

        public virtual void Print(params object[] args)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < args.Length; i++)
            {
                if (i > 0)
                    sb.Append("\t");

                sb.Append(args[i]);
            }

            Output.WriteLine(sb);
        }

        public override string GetNameDisplay()
        {
            // Truncate the script to be no more than 50 characters
            string truncatedScript = Script.Length > 47 ? Script.Substring(0, 47) + "..." : Script;

            return Name.Replace("{0}", truncatedScript);
        }

        internal override void ResetEnvironment()
        {
            base.ResetEnvironment();

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

        internal override void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
        {
            Instance = this;

            base.OnStartListening(listener, ref otherActions);

            ResetEnvironment();
        }
    }
}
