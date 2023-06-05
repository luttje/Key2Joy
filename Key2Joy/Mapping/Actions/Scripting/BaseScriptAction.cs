using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Util;
using Key2Joy.Plugins;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Key2Joy.Mapping
{
    [ObjectListViewGroup(
        Name = "Scripting",
        Image = "script_code"
    )]
    public abstract class BaseScriptAction : CoreAction
    {
        public static readonly object LockObject = new object();

        /// <summary>
        /// TODO: Clean this up. This is just a quick hack to get enumerations into scripts. Should move to something the plugins can affect nicely.
        /// </summary>
        public static IList<ExposedEnumeration> ExposedEnumerations;

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

        public abstract void RegisterScriptingEnum(ExposedEnumeration enumeration);
        public abstract void RegisterScriptingMethod(ExposedMethod exposedMethod, AbstractAction instance);

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

        public override void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
        {
            base.OnStartListening(listener, ref otherActions);
        }
    }
}
