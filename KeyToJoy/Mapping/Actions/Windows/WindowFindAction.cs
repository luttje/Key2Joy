using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    [Action(
        Description = "Find Window Handle",
        Visibility = ActionVisibility.Never,
        NameFormat = "Find Window '{0}' with title {1}",
        FunctionName = SCRIPT_COMMAND,
        FunctionMethodName = nameof(ExecuteActionForScript)
    )]
    internal class WindowFindAction : BaseAction
    {
        internal const string SCRIPT_COMMAND = "window_find";

        [JsonProperty]
        internal string ClassName { get; set; }
        
        [JsonProperty]
        internal string WindowName { get; set; }

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string className, string windowTitle);

        public WindowFindAction(string name, string description)
            : base(name, description)
        { }

        public object ExecuteActionForScript(BaseScriptAction scriptAction, params object[] parameters)
        {
            if (parameters.Length < 1)
                throw new ArgumentException($"{SCRIPT_COMMAND} expected a Window class name string and optional title!");

            if (!(parameters[0] is string className))
                throw new ArgumentException($"{SCRIPT_COMMAND} expected a Window class name string as the first argument!");

            string windowTitle = parameters.Length > 1 ? (parameters[1] as string) : null;
            if (parameters.Length > 1 && windowTitle == null)
                throw new ArgumentException($"{SCRIPT_COMMAND} expected a Window title string as the second argument!");

            ClassName = className;
            WindowName = windowTitle;

            var windowHandle = FindWindow(ClassName, WindowName);
            return windowHandle;
        }

        internal override async Task Execute(InputBag inputBag = null)
        {
            // TODO: Currently this is only a script action...
        }

        public override string GetNameDisplay()
        {
            return Name.Replace("{0}", ClassName)
                .Replace("{1}", WindowName);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WindowFindAction action))
                return false;

            return action.ClassName == ClassName
                && action.WindowName == WindowName;
        }

        public override object Clone()
        {
            return new WindowFindAction(Name, description)
            {
                ClassName = ClassName,
                WindowName = WindowName,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
