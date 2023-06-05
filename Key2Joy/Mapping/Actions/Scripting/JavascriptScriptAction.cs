using Jint;
using Jint.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Key2Joy.Util;
using System.Text;
using Jint.Native.Object;
using Jint.Runtime.Interop;
using Jint.Runtime.Descriptors;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Plugins;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Javascript Action",
        NameFormat = "Javascript Script: {0}"
    )]
    public class JavascriptAction : BaseScriptActionWithEnvironment<Engine>
    {
        public JavascriptAction(string name, string description)
            : base(name, description)
        {
            ImageResource = "JS";
        }

        public override async Task Execute(IInputBag inputBag)
        {
            try
            {
                lock (LockObject)
                    environment.Execute(GetExecutableScript());
            }
            catch (Jint.Runtime.JavaScriptException e)
            {
                Output.WriteLine(e);
            }
        }

        public override void RegisterScriptingEnum(ExposedEnumeration enumeration)
        {
            var enumInjectScript = new StringBuilder();
            enumInjectScript.Append(enumeration.Name + " = {");

            foreach (var kvp in enumeration.KeyValues)
            {
                var enumKey = kvp.Key;
                var enumValue = kvp.Value;
                //var underlyingType = enumValue.GetType();

                //if (underlyingType != typeof(int) && underlyingType != typeof(short))
                //{
                //    throw new NotImplementedException("Enumeration was of unimplemented underlying datatype: " + underlyingType);
                //}

                enumInjectScript.Append(enumKey);
                enumInjectScript.Append(": ");
                enumInjectScript.Append(enumValue);

                enumInjectScript.Append(",\n");
            }

            enumInjectScript.Append("};");

            environment.Execute(enumInjectScript.ToString());

            environment.Execute($"Print(JSON.stringify({enumInjectScript}))");
        }

        public override void RegisterScriptingMethod(ExposedMethod exposedMethod, AbstractAction instance)
        {
            var functionName = exposedMethod.FunctionName;
            var parents = functionName.Split('.');
            var @delegate = new DelegateWrapper(environment, exposedMethod.CreateDelegate(instance));

            //var paramDebug = string.Join(", ", method.GetParameters()
            //    .Select(p => $"{p.ParameterType.Name} {p.Name}")
            //    .ToArray());
            //Output.WriteLine(Output.OutputModes.Verbose, $"js.RegisterFunction({functionName},{instance},{method.Name}({paramDebug}):{method.ReturnType})");

            if (parents.Length > 1)
            {
                var currentObject = environment.Realm.GlobalObject;

                for (int i = 0; i < parents.Length; i++)
                {
                    if(i != parents.Length - 1)
                    {
                        if (!currentObject.TryGetValue(parents[i], out JsValue child))
                            child = new JsObject(environment);

                        if (!(child is ObjectInstance childObject))
                            throw new NotImplementedException($"Tried using a non object({parents[i]}) as object parent while registering function: {functionName}!");

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

            environment.SetValue(
                functionName,
                @delegate);
        }

        public override Engine MakeEnvironment()
        {
            return new Engine();
        }

        public override void RegisterEnvironmentObjects()
        {
            environment.SetValue("Print", new Action<object[]>(Print));
            
            base.RegisterEnvironmentObjects();
        }

        public override void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
        {
            base.OnStartListening(listener, ref otherActions);
        }

        public override void OnStopListening(AbstractTriggerListener listener)
        {
            base.OnStopListening(listener);

            environment = null;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is JavascriptAction action))
                return false;

            return action.Name == Name
                && action.Script == Script;
        }

        public override object Clone()
        {
            return new JavascriptAction(Name, description)
            {
                Script = Script,
                IsScriptPath = IsScriptPath,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
