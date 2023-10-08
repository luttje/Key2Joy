using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Actions.Windows
{
    [Action(
        Description = "Find Window Handle",
        Visibility = MappingMenuVisibility.Never,
        NameFormat = "Find Window '{0}' with title {1}"
    )]
    public class WindowFindAction : CoreAction
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string className, string windowTitle);

        public string ClassName { get; set; }
        public string WindowName { get; set; }

        public WindowFindAction(string name)
            : base(name)
        { }

        /// <markdown-doc>
        /// <parent-name>Windows</parent-name>
        /// <path>Api/Windows</path>
        /// </markdown-doc>
        /// <summary>
        /// Find a window of a piece of software currently running.
        /// </summary>
        /// <param name="className">Window class name</param>
        /// <param name="windowTitle">Optional window title</param>
        /// <returns>Handle for the window</returns>
        /// <name>Window.Find</name>
        [ExposesScriptingMethod("Window.Find")]
        public IntPtr ExecuteForScript(string className, string windowTitle = null)
        {
            this.ClassName = className;
            this.WindowName = windowTitle;

            var windowHandle = FindWindow(this.ClassName, this.WindowName);
            return windowHandle;
        }

        public override async Task Execute(AbstractInputBag inputBag = null)
        {
            // TODO: Currently this is only a script action...
        }

        public override string GetNameDisplay()
        {
            return this.Name.Replace("{0}", this.ClassName)
                .Replace("{1}", this.WindowName);
        }

        public override bool Equals(object obj)
        {
            if (obj is not WindowFindAction action)
            {
                return false;
            }

            return action.ClassName == this.ClassName
                && action.WindowName == this.WindowName;
        }
    }
}
