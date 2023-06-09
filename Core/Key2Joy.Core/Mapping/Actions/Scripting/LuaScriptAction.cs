using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Plugins;
using Key2Joy.Plugins;
using NLua;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Lua Script Action",
        NameFormat = "Lua Script: {0}",
        GroupName = "Scripting",
        GroupImage = "script_code"
    )]
    public class LuaScriptAction : BaseScriptActionWithEnvironment<Lua>
    {
        public LuaScriptAction(string name)
            : base(name)
        {
            ImageResource = "Lua";
        }

        public override async Task Execute(AbstractInputBag inputBag)
        {
            try
            {
                var source = "Key2Joy.Script.Inline";
                if (IsScriptPath)
                    source = Script;

                lock (LockObject)
                {
                    if (environment.State == null)
                        Debugger.Break(); // This really shouldn't happen.

                    environment.DoString(GetExecutableScript(), Script);
                }
            }
            catch (NLua.Exceptions.LuaScriptException ex)
            {
                Output.WriteLine(ex.Message + ex.StackTrace);
                Debugger.Break();
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
            }
        }
        
        public override void RegisterScriptingEnum(ExposedEnumeration enumeration)
        {
            // TODO: Use https://github.com/NLua/NLua/blob/3aaff863c78e89a009c21ff3aef94502018f2566/src/LuaRegistrationHelper.cs#LL76C28-L76C39
            environment.NewTable(enumeration.Name);

            foreach (var kvp in enumeration.KeyValues)
            {
                var enumKey = kvp.Key;
                var enumValue = kvp.Value;

                string path = enumeration.Name + "." + enumKey;
                environment.SetObjectToPath(path, enumValue);
            }
        }

        public override void RegisterScriptingMethod(ExposedMethod exposedMethod, AbstractAction instance)
        {
            var functionName = exposedMethod.FunctionName;
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

            if (exposedMethod is AppDomainExposedMethod methodNeedProxy)
            {
                var proxyHost = new ActionScriptProxyHost(methodNeedProxy.AppDomain, methodNeedProxy.AssemblyPath, instance, methodNeedProxy.MethodName);
                proxyHost.RegisterParameterTransformer<LuaFunction>(luaFunction => new WrappedPluginType((Delegate)luaFunction.Call));
                environment.RegisterFunction(functionName, proxyHost, proxyHost.GetExecutorMethodInfo());
                return;
            }

            environment.RegisterFunction(
                functionName,
                instance,
                instance.GetType().GetMethod(exposedMethod.MethodName));
        }

        public override Lua MakeEnvironment()
        {
            return new Lua();
        }

        public override void RegisterEnvironmentObjects()
        {
            environment.RegisterFunction("print", this, typeof(LuaScriptAction).GetMethod(nameof(Print), new[] { typeof(object[]) }));
            environment.RegisterFunction("Print", this, typeof(LuaScriptAction).GetMethod(nameof(Print), new[] { typeof(object[]) }));
            environment.RegisterFunction("collection", this, typeof(LuaScriptAction).GetMethod(nameof(CollectionIterator), new[] { typeof(ICollection) }));

            base.RegisterEnvironmentObjects();
        }

        public override void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
        {
            base.OnStartListening(listener, ref otherActions);
        }

        public override void OnStopListening(AbstractTriggerListener listener)
        {
            base.OnStopListening(listener);

            // environment.Dispose(); // Uncomment this to cause NullReferenceException problem on NLua state described here: https://github.com/luttje/Key2Joy/pull/39#issuecomment-1581537603
        }

        /// <summary>
        /// Returns a function that, when called, will return the next value in the collection.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public LuaFunction CollectionIterator(ICollection data)
        {
            var iterator = new LuaIterator(data);

            return environment.RegisterFunction("__iterator", iterator, iterator.GetType().GetMethod(nameof(LuaIterator.Next)));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LuaScriptAction action))
                return false;

            return action.Name == Name
                && action.Script == Script;
        }
    }
}
