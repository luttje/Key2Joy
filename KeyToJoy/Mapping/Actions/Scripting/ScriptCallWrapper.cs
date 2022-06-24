using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KeyToJoy.Util;

namespace KeyToJoy.Mapping
{
    internal class ScriptCallWrapper
    {
        private BaseScriptAction scriptAction;
        private BaseAction instance;
        private MethodInfo method;

        public ScriptCallWrapper(
            BaseScriptAction scriptAction,
            BaseAction instance, 
            Type actionType, 
            string methodName)
        {
            this.scriptAction = scriptAction;
            this.instance = instance;
            this.method = actionType.GetMethod(
                methodName,
                new[] { typeof(BaseScriptAction), typeof(object[]) });
        }

        public object[] CallWithScriptAction(params object[] parameters)
        {
            // Append the script action to the parameters
            var newParameters = new object[]
            {
                scriptAction,
                parameters
            };
            
            return method.Invoke(instance, newParameters) as object[];
        }

        internal Delegate GetWrapperDelegate()
        {
            return GetWrapperMethod().CreateDelegate(this);
        }

        internal MethodInfo GetWrapperMethod()
        {
            return typeof(ScriptCallWrapper).GetMethod(
                nameof(CallWithScriptAction),
                new[] { typeof(object[]) });
        }
    }
}
