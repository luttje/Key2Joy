using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Key2Joy.Contracts;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Contracts.Plugins;
using Key2Joy.Plugins;
using NLua;

namespace Key2Joy.Mapping.Actions.Scripting;

[Action(
    Description = "Lua Script Action",
    NameFormat = "Lua Script: {0}",
    GroupName = "Scripting",
    GroupImage = "script_code"
)]
public class LuaScriptAction : BaseScriptActionWithEnvironment<Lua>
{
    public LuaScriptAction(string name)
        : base(name) => this.ImageResource = "Lua";

    public override async Task Execute(AbstractInputBag inputBag)
    {
        try
        {
            var source = "Key2Joy.Script.Inline";
            if (this.IsScriptPath)
            {
                source = this.Script;
            }

            lock (LockObject)
            {
                if (this.environment.State == null)
                {
                    Debugger.Break(); // This really shouldn't happen.
                }

                this.environment.DoString(this.GetExecutableScript(), this.Script);
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
        this.environment.NewTable(enumeration.Name);

        foreach (var kvp in enumeration.KeyValues)
        {
            var enumKey = kvp.Key;
            var enumValue = kvp.Value;

            var path = enumeration.Name + "." + enumKey;
            this.environment.SetObjectToPath(path, enumValue);
        }
    }

    public override void RegisterScriptingMethod(ExposedMethod exposedMethod, AbstractAction instance)
    {
        var functionName = exposedMethod.FunctionName;
        var parents = functionName.Split('.');

        if (parents.Length > 1)
        {
            StringBuilder currentPath = new();

            for (var i = 0; i < parents.Length - 1; i++)
            {
                if (i > 0)
                {
                    currentPath.Append('.');
                }

                currentPath.Append(parents[i]);
            }

            var path = currentPath.ToString();

            if (this.environment.GetTable(path) == null)
            {
                this.environment.NewTable(path);
            }
        }

        if (exposedMethod is PluginExposedMethod methodNeedProxy)
        {
            methodNeedProxy.RegisterParameterTransformer<LuaFunction>(luaFunction => new WrappedPluginType(luaFunction.Call));
            this.environment.RegisterFunction(functionName, methodNeedProxy, methodNeedProxy.GetExecutorMethodInfo((PluginActionProxy)instance));
            return;
        }

        this.environment.RegisterFunction(
            functionName,
            instance,
            instance.GetType().GetMethod(exposedMethod.MethodName));
    }

    public override Lua MakeEnvironment() => new Lua();

    public override void RegisterEnvironmentObjects()
    {
        this.environment.RegisterFunction("print", this, typeof(LuaScriptAction).GetMethod(nameof(Print), new[] { typeof(object[]) }));
        this.environment.RegisterFunction("Print", this, typeof(LuaScriptAction).GetMethod(nameof(Print), new[] { typeof(object[]) }));
        this.environment.RegisterFunction("collection", this, typeof(LuaScriptAction).GetMethod(nameof(CollectionIterator), new[] { typeof(ICollection) }));

        base.RegisterEnvironmentObjects();
    }

    public override void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions) => base.OnStartListening(listener, ref otherActions);

    public override void OnStopListening(AbstractTriggerListener listener) => base.OnStopListening(listener);// environment.Dispose(); // Uncomment this to cause NullReferenceException problem on NLua state described here: https://github.com/luttje/Key2Joy/pull/39#issuecomment-1581537603

    /// <summary>
    /// Returns a function that, when called, will return the next value in the collection.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public LuaFunction CollectionIterator(ICollection data)
    {
        LuaIterator iterator = new(data);

        return this.environment.RegisterFunction("__iterator", iterator, iterator.GetType().GetMethod(nameof(LuaIterator.Next)));
    }

    public override bool Equals(object obj)
    {
        if (obj is not LuaScriptAction action)
        {
            return false;
        }

        return action.Name == this.Name
            && action.Script == this.Script;
    }
}
