using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Key2Joy.Mapping
{
    [Util.ObjectListViewGroup(
        Name = "Scripting",
        Image = "script_code"
    )]
    internal abstract class BaseScriptAction : BaseAction
    {
        internal static readonly object LockObject = new object();

        [JsonProperty]
        public string Script { get; set; }

        [JsonProperty]
        public bool IsScriptPath { get; set; }

        protected string cachedFile;

        public BaseScriptAction(string name, string description)
            : base(name, description)
        { }

        protected virtual string GetExecutableScript()
        {
            if (IsScriptPath)
            {
                if (cachedFile == null)
                    cachedFile = System.IO.File.ReadAllText(Script);

                return cachedFile;
            }

            return Script;
        }

        internal abstract void RegisterScriptingEnum(Type enumType);
        internal abstract void RegisterScriptingMethod(string functionName, BaseAction instance, MethodInfo method);

        public virtual void Print(params object[] args)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < args.Length; i++)
            {
                if (i > 0)
                    sb.Append("\t");

                sb.Append(args[i]);
            }

            Output.WriteLine(sb);
        }

        public override string GetNameDisplay()
        {
            // Truncate the script to be no more than 50 characters
            string truncatedScript = Script.Length > 47 ? Script.Substring(0, 47) + "..." : Script;

            return Name.Replace("{0}", truncatedScript);
        }

        internal override void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
        {
            base.OnStartListening(listener, ref otherActions);
        }
    }
}
