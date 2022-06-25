﻿using Newtonsoft.Json;
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
        Description = "Get Window Title",
        Visibility = ActionVisibility.Never,
        NameFormat = "Get Window Title",
        FunctionName = SCRIPT_COMMAND,
        FunctionMethodName = nameof(ExecuteActionForScript)
    )]
    internal class WindowGetTitleAction : BaseAction
    {
        internal const string SCRIPT_COMMAND = "window_get_title";
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        public WindowGetTitleAction(string name, string description)
            : base(name, description)
        { }

        public object ExecuteActionForScript(BaseScriptAction scriptAction, params object[] parameters)
        {
            if (parameters.Length < 1)
                throw new ArgumentException($"{SCRIPT_COMMAND} expected a window handle!");

            if (!scriptAction.TryConvertParameterToPointer(parameters[0], out IntPtr handle))
                throw new ArgumentException($"{SCRIPT_COMMAND} expected a window handle as the first argument!");

            var length = GetWindowTextLength(handle) + 1;
            var title = new StringBuilder(length);
            GetWindowText(handle, title, length);
            return title.ToString();
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
            if (!(obj is WindowGetTitleAction action))
                return false;

            // TODO: Currently this is only a script action so this is irrelevant
            return false;
        }

        public override object Clone()
        {
            return new WindowGetTitleAction(Name, description)
            {
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}