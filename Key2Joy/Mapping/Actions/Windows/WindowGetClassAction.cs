using Key2Joy.Contracts.Mapping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Get Window Class name",
        Visibility = MappingMenuVisibility.Never,
        NameFormat = "Get Window Class name"
    )]
    public class WindowGetClassAction : CoreAction
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public WindowGetClassAction(string name)
            : base(name)
        { }

        /// <markdown-doc>
        /// <parent-name>Windows</parent-name>
        /// <path>Api/Windows</path>
        /// </markdown-doc>
        /// <summary>
        /// Get the class name for a specified Window.
        /// 
        /// You can use <see href="Window.GetAllAction.md"/>, <see href="Window.FindAction.md"/> or  <see href="Window.GetForegroundAction.md"/> to get handles.
        /// </summary>
        /// <param name="handle">The window handle to get the class for</param>
        /// <returns>Class name for the window</returns>
        /// <name>Window.GetClass</name>
        [ExposesScriptingMethod("Window.GetClass")]
        public string ExecuteForScript(IntPtr handle)
        {
            int nRet;
            // Pre-allocate 256 characters, since this is the maximum class name length.
            var classNameBuilder = new StringBuilder(256);
            //Get the window class name
            nRet = GetClassName((IntPtr)handle, classNameBuilder, classNameBuilder.Capacity);
            
            if (nRet == 0)
                throw new InvalidCastException("Invalid window handle?");

            return classNameBuilder.ToString();
        }

        public override async Task Execute(IInputBag inputBag = null)
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
            return new WindowGetClassAction(Name, new Dictionary<string, object>
            {
                { "ImageResource", ImageResource }
            });
        }
    }
}
