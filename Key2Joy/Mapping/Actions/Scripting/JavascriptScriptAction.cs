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

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Javascript Action",
        OptionsUserControl = typeof(ScriptActionControl),
        OptionsUserControlParams = new[] { "Javascript" },
        NameFormat = "Javascript Script: {0}"
    )]
    internal class JavascriptAction : BaseScriptAction
    {
        private Engine engine;
        private string cachedFile;

        public JavascriptAction(string name, string description)
            : base(name, description)
        {
            ImageResource = "JS";
        }

        internal override async Task Execute(InputBag inputBag)
        {
            try
            {
                if (IsScriptPath)
                {
                    if (cachedFile == null)
                        cachedFile = System.IO.File.ReadAllText(Script);

                    lock (LockObject)
                        engine.Execute(cachedFile);
                    return;
                }

                lock (LockObject)
                    engine.Execute(Script);
            }
            catch (Jint.Runtime.JavaScriptException e)
            {
                Output.WriteLine(e);
            }
        }

        internal override void RegisterScriptingEnum(Type enumType)
        {
            var enumNames = Enum.GetNames(enumType);

            // TODO: Probably should make a better way to register enums
            engine.Execute(
                enumType.Name + " = {" +
                string.Join(", ", enumNames.Select((name, index) =>
                {
                    var underlyingType = Enum.GetUnderlyingType(enumType);
                    var e = Enum.Parse(enumType, name);

                    if (underlyingType == typeof(int))
                        return $"{name}: {(int)e}";
                    else if (underlyingType == typeof(short))
                        return $"{name}: {(short)e}";
                    else
                        throw new NotImplementedException("Enumeration was of unimplemented underlying datatype: " + underlyingType);
                })) +
                "    }"
            );

            engine.Execute($"Print(JSON.stringify({enumType.Name}))");
        }

        internal override void RegisterScriptingMethod(string functionName, BaseAction instance, MethodInfo method)
        {
            var parents = functionName.Split('.');
            var @delegate = new DelegateWrapper(engine, method.CreateDelegate(instance));

            var paramDebug = string.Join(", ", method.GetParameters()
                .Select(p => $"{p.ParameterType.Name} {p.Name}")
                .ToArray());
            Output.WriteLine(Output.OutputModes.Verbose, $"js.RegisterFunction({functionName},{instance},{method.Name}({paramDebug}):{method.ReturnType})");

            if (parents.Length > 1)
            {
                var currentObject = engine.Realm.GlobalObject;

                for (int i = 0; i < parents.Length; i++)
                {
                    if(i != parents.Length - 1)
                    {
                        JsValue child;

                        if (!currentObject.TryGetValue(parents[i], out child))
                            child =  new ObjectInstance(engine);

                        if (!(child is ObjectInstance childObject))
                            throw new NotImplementedException($"Tried using a non object({parents[i]}) as object parent while registering function: {functionName}!");

                        currentObject.FastAddProperty(parents[i], childObject, false, true, true);
                        currentObject = childObject;
                    }
                    else
                    {
                        currentObject.FastAddProperty(parents[i], @delegate, false, true, true);
                    }
                }

                return;
            }

            engine.SetValue(
                functionName,
                @delegate);
        }

        internal override void ResetEnvironment()
        {
            engine = new Engine();
            engine.SetValue("Print", new Action<object[]>(Print));

            base.ResetEnvironment();
        }

        internal override void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
        {
            base.OnStartListening(listener, ref otherActions);
        }

        internal override void OnStopListening(TriggerListener listener)
        {
            base.OnStopListening(listener);

            engine = null;
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
