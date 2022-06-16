using KeyToJoy.Properties;
using Newtonsoft.Json;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace KeyToJoy.Mapping
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class MappedOption : ICloneable
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public BaseAction Action;
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public BaseTrigger Binding;

        public string GetActionDisplay()
        {
            return Action.ToString();
        }

        internal object GetBindDisplay()
        {
            if (Binding == null)
                return "<not bound to key>";

            return Binding.ToString();
        }

        public object Clone()
        {
            return new MappedOption()
            {
                Binding = Binding != null ? (BaseTrigger)Binding.Clone() : null,
                Action = Action,
            };
        }
    }
}
