using Neo.IronLua;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    [Action(
        Description = "Lua Script Action",
        OptionsUserControl = typeof(LuaScriptActionControl),
        NameFormat = "Lua Script: {0}"
    )]
    internal class LuaScriptAction : BaseAction
    {
        [JsonProperty]
        public string Script { get; set; }

        private Lua lua;

        public LuaScriptAction(string name, string description)
            : base(name, description)
        { }

        internal override async Task Execute(InputBag inputBag)
        {
            dynamic g = lua.CreateEnvironment<LuaGlobal>();
            // register new functions
            g.print = new Action<object[]>(Print);

            g.dochunk(Script, "KeyToJoy.Script.Inline.lua");
        }

        private void Print(object[] objects)
        {
            foreach (var obj in objects)
            {
                System.Diagnostics.Debug.Write(obj);
            }
            System.Diagnostics.Debug.WriteLine("");
        }

        internal override void OnStartListening()
        {
            base.OnStartListening();

            lua = new Lua();
        }

        internal override void OnStopListening()
        {
            base.OnStopListening();

            lua.Dispose();
        }

        public override string GetNameDisplay()
        {
            // Truncate the script to be no more than 50 characters
            string truncatedScript = Script.Length > 47 ? Script.Substring(0, 47) + "..." : Script;

            return Name.Replace("{0}", truncatedScript);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LuaScriptAction action))
                return false;

            return action.Name == Name
                && action.Script == Script;
        }

        public override object Clone()
        {
            return new LuaScriptAction(Name, description)
            {
                Script = Script,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
