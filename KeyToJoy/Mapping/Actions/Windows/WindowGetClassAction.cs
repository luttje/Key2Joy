using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    [Action(
        Description = "Get Window Class name",
        Visibility = ActionVisibility.Never,
        NameFormat = "Get Window Class name",
        FunctionName = SCRIPT_COMMAND,
        FunctionMethodName = nameof(ExecuteActionForScript)
    )]
    internal class WindowGetClassAction : BaseAction
    {
        internal const string SCRIPT_COMMAND = "window_get_class";

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public WindowGetClassAction(string name, string description)
            : base(name, description)
        { }

        public object ExecuteActionForScript(BaseScriptAction scriptAction, params object[] parameters)
        {
            if (parameters.Length < 1)
                throw new ArgumentException($"{SCRIPT_COMMAND} expected a window handle!");

            if (!scriptAction.TryConvertParameterToPointer(parameters[0], out IntPtr handle))
                throw new ArgumentException($"{SCRIPT_COMMAND} expected a window handle as the first argument!");

            int nRet;
            // Pre-allocate 256 characters, since this is the maximum class name length.
            StringBuilder classNameBuilder = new StringBuilder(256);
            //Get the window class name
            nRet = GetClassName((IntPtr)handle, classNameBuilder, classNameBuilder.Capacity);
            
            if (nRet == 0)
                throw new InvalidCastException("Invalid window handle?");

            return classNameBuilder.ToString();
        }

        internal override async Task Execute(InputBag inputBag = null)
        {
            // TODO: Currently this is only a script action...
        }

        public override string GetNameDisplay()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WindowGetClassAction action))
                return false;

            // TODO: Currently this is only a script action so this is irrelevant
            return false;
        }

        public override object Clone()
        {
            return new WindowGetClassAction(Name, description)
            {
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
