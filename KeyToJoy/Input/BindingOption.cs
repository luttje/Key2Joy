using KeyToJoy.Properties;
using Newtonsoft.Json;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace KeyToJoy.Input
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class BindingOption : ICloneable
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public BindableAction Action;
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public Binding Binding;

        public string GetActionDisplay()
        {
            return Action.ToString();
        }

        internal object GetBindDisplay()
        {
            return Binding.ToString();
        }

        public object Clone()
        {
            return new BindingOption()
            {
                Binding = (Binding)Binding.Clone(),
                Action = Action,
            };
        }
    }
}
