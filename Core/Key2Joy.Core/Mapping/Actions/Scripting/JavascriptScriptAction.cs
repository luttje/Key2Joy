using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Jint.Native;
using Jint.Native.Object;
using Jint.Runtime.Descriptors;
using Jint.Runtime.Interop;
using Key2Joy.Contracts;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Plugins;
using Key2Joy.Util;

namespace Key2Joy.Mapping.Actions.Scripting;

[Action(
    Description = "Javascript Action",
    NameFormat = "Javascript Script: {0}",
    GroupName = "Scripting",
    GroupImage = "script_code"
)]
public class JavascriptAction : BaseScriptActionWithEnvironment<Engine>
{
    public JavascriptAction(string name)
        : base(name) => this.ImageResource = "JS";

    public override async Task Execute(AbstractInputBag inputBag)
    {
        try
        {
            lock (LockObject)
            {
                this.Environment.Execute(this.GetExecutableScript());
            }
        }
        catch (Jint.Runtime.JavaScriptException ex)
        {
            Output.WriteLine(ex);
        }

        await base.Execute(inputBag);
    }

    public override void RegisterScriptingEnum(ExposedEnumeration enumeration)
    {
        StringBuilder enumInjectScript = new();
        enumInjectScript.Append(enumeration.Name + " = {");

        foreach (var kvp in enumeration.KeyValues)
        {
            var enumKey = kvp.Key;
            var enumValue = kvp.Value;

            enumInjectScript.Append(enumKey);
            enumInjectScript.Append(": ");
            enumInjectScript.Append(enumValue);

            enumInjectScript.Append(",\n");
        }

        enumInjectScript.Append("};");

        var enumInjection = enumInjectScript.ToString();
        this.Environment.Execute(enumInjection);

        //this.environment.Execute($"Print(JSON.stringify({enumInjection}))");
    }

    public override void RegisterScriptingMethod(ExposedMethod exposedMethod, AbstractAction instance)
    {
        exposedMethod.Prepare(instance);

        var functionName = exposedMethod.FunctionName;
        var parents = functionName.Split('.');
        var methodInfo = exposedMethod.GetExecutorMethodInfo();
        var @delegate = new DelegateWrapper(this.Environment, methodInfo.CreateDelegate(exposedMethod));

        if (parents.Length > 1)
        {
            var currentObject = this.Environment.Realm.GlobalObject;

            for (var i = 0; i < parents.Length; i++)
            {
                if (i != parents.Length - 1)
                {
                    if (!currentObject.TryGetValue(parents[i], out var child))
                    {
                        child = new JsObject(this.Environment);
                    }

                    if (child is not ObjectInstance childObject)
                    {
                        throw new NotImplementedException($"Tried using a non object({parents[i]}) as object parent while registering function: {functionName}!");
                    }

                    currentObject.FastSetProperty(parents[i], new PropertyDescriptor(childObject, false, true, true));
                    currentObject = childObject;
                }
                else
                {
                    currentObject.FastSetProperty(parents[i], new PropertyDescriptor(@delegate, false, true, true));
                }
            }

            return;
        }

        this.Environment.SetValue(
            functionName,
            methodInfo);
    }

    public override Engine MakeEnvironment() => new Engine(options =>
    {
        options.Interop.AllowSystemReflection = true;
    });

    public override void RegisterEnvironmentObjects()
    {
        this.Environment.SetValue("Print", new Action<object[]>(this.Print));

        base.RegisterEnvironmentObjects();
    }

    public override void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions) => base.OnStartListening(listener, ref otherActions);

    public override void OnStopListening(AbstractTriggerListener listener)
    {
        base.OnStopListening(listener);

        this.Environment = null;
    }

    public override bool Equals(object obj)
    {
        if (obj is not JavascriptAction action)
        {
            return false;
        }

        return action.Name == this.Name
            && action.Script == this.Script;
    }
}
