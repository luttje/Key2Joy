using Key2Joy.Contracts.Mapping;
using Newtonsoft.Json;
using System;
using System.Drawing;

namespace Key2Joy.Mapping
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class CoreTrigger : AbstractTrigger
    {
        public string ImageResource { get; set; }

        protected string description;


        public CoreTrigger(string name, string description)
        {
            Name = name;
            this.description = description;
        }
    }
}
