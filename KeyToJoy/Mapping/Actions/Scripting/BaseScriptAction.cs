using Jint;
using Jint.Native;
using KeyToJoy.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    internal abstract class BaseScriptAction : BaseAction
    {
        [JsonProperty]
        public string Script { get; set; }

        [JsonProperty]
        public bool IsScriptPath { get; set; }

        public BaseScriptAction(string name, string description)
            : base(name, description)
        { }

        // TODO: Keep this here and output to a source the user can see (send errors there too)
        //public void Print(string message)
        //{
        //    System.Diagnostics.Debug.WriteLine(message);
        //}

        public override string GetNameDisplay()
        {
            // Truncate the script to be no more than 50 characters
            string truncatedScript = Script.Length > 47 ? Script.Substring(0, 47) + "..." : Script;

            return Name.Replace("{0}", truncatedScript);
        }

        internal abstract bool TryConvertParameterToDouble(object parameter, out double result);
        internal abstract bool TryConvertParameterToLong(object parameter, out long result);
        internal abstract bool TryConvertParameterToByte(object parameter, out byte result);
        internal abstract bool TryConvertParameterToCallback(object parameter, out Action callback);
        internal abstract bool TryConvertParameterToPointer(object v, out IntPtr handle);
    }
}
