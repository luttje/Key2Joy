using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                if (this.Environment.State == null)
                {
                    Debugger.Break(); // This really shouldn't happen.
                }

                this.Environment.DoString(this.GetExecutableScript(), this.Script);
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

        await base.Execute(inputBag);
    }

    public override void RegisterScriptingEnum(ExposedEnumeration enumeration)
    {
        // TODO: Use https://github.com/NLua/NLua/blob/3aaff863c78e89a009c21ff3aef94502018f2566/src/LuaRegistrationHelper.cs#LL76C28-L76C39
        this.Environment.NewTable(enumeration.Name);

        foreach (var kvp in enumeration.KeyValues)
        {
            var enumKey = kvp.Key;
            var enumValue = kvp.Value;

            var path = enumeration.Name + "." + enumKey;
            this.Environment.SetObjectToPath(path, enumValue);
        }
    }

    public override void RegisterScriptingMethod(ExposedMethod exposedMethod, AbstractAction instance)
    {
        exposedMethod.Prepare(instance);

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

            if (this.Environment.GetTable(path) == null)
            {
                this.Environment.NewTable(path);
            }
        }

        exposedMethod.RegisterParameterTransformer<LuaTable>((luaTable, expectedType) =>
        {
            var dictionary = new Dictionary<object, object>();
            var areKeysSequential = true;
            var sequentialKey = 1L;

            foreach (var key in luaTable.Keys)
            {
                var keyAsLong = key as long?;
                var value = luaTable[key];

                if (areKeysSequential
                && keyAsLong != null
                && keyAsLong == sequentialKey)
                {
                    dictionary.Add(sequentialKey, value);
                    sequentialKey++;
                }
                else
                {
                    areKeysSequential = false;
                    dictionary.Add(key, value);
                }
            }

            if (areKeysSequential)
            {
                return dictionary.Values.ToArray();
            }

            return dictionary;
        });
        exposedMethod.RegisterParameterTransformer<LuaFunction>((luaFunction, expectedType) => new WrappedPluginType(luaFunction.Call));
        this.Environment.RegisterFunction(functionName, exposedMethod, exposedMethod.GetExecutorMethodInfo());
    }

    public override Lua MakeEnvironment() => new Lua();

    public override void RegisterEnvironmentObjects()
    {
        this.Environment.RegisterFunction("print", this, typeof(LuaScriptAction).GetMethod(nameof(Print), new[] { typeof(object[]) }));
        this.Environment.RegisterFunction("Print", this, typeof(LuaScriptAction).GetMethod(nameof(Print), new[] { typeof(object[]) }));
        this.Environment.RegisterFunction("collection", this, typeof(LuaScriptAction).GetMethod(nameof(CollectionIterator), new[] { typeof(ICollection) }));

        base.RegisterEnvironmentObjects();
    }

    public override void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
        => base.OnStartListening(listener, ref otherActions);

    public override void OnStopListening(AbstractTriggerListener listener)
        => base.OnStopListening(listener);// environment.Dispose(); // Uncomment this to cause NullReferenceException problem on NLua state described here: https://github.com/luttje/Key2Joy/pull/39#issuecomment-1581537603

    /// <summary>
    /// Returns a function that, when called, will return the next value in the collection.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public LuaFunction CollectionIterator(ICollection data)
    {
        LuaIterator iterator = new(data);

        return this.Environment.RegisterFunction("__iterator", iterator, iterator.GetType().GetMethod(nameof(LuaIterator.Next)));
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
