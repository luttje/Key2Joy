using KeyToJoy.Properties;
using Newtonsoft.Json;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace KeyToJoy.Mapping
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MappedOption : ICloneable
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public BaseAction Action;
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public BaseTrigger Trigger;

        public string GetActionDisplay()
        {
            return Action.GetNameDisplay();
        }

        internal object GetTriggerDisplay()
        {
            if (Trigger == null)
                return "<not bound to key>";

            return Trigger.ToString();
        }

        public object Clone()
        {
            return new MappedOption()
            {
                Trigger = Trigger != null ? (BaseTrigger)Trigger.Clone() : null,
                Action = Action,
            };
        }
    }
}
