using Key2Joy.Mapping;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.LowLevelInput
{
    [JsonConverter(typeof(LegacyPressStateConverter))]
    public enum PressState
    {
        /// <summary>
        /// Will be converted to a seperate Press and Release later.
        /// </summary>
        [Obsolete("LegacyPressAndRelease is deprecated, please use Press and Release instead.")]
        LegacyPressAndRelease,

        /// <summary>
        /// Pressed down
        /// </summary>
        Press,

        /// <summary>
        /// Release (after pressed down)
        /// </summary>
        Release,
    }

    public class LegacyPressStateConverter : JsonConverter<PressState>
    {
        public override void WriteJson(JsonWriter writer, PressState value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override PressState ReadJson(JsonReader reader, Type objectType, PressState existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if ((string)reader.Value == "PressAndRelease")
            {
                return PressState.LegacyPressAndRelease;
            }

            return (PressState)Enum.Parse(typeof(PressState), reader.Value.ToString());
        }

        internal static void UpdateLegacyIfApplicable(MappingPreset preset)
        {
            bool changed = false;
            var listCopy = new List<MappedOption>(preset.MappedOptions);

            foreach (var mappedOption in listCopy)
            {
                MappedOption releaseClone = null;
                
                if (mappedOption.Trigger is IPressState pressTrigger)
                {
                    if (pressTrigger.PressState == PressState.LegacyPressAndRelease)
                    {
                        pressTrigger.PressState = PressState.Press;

                        releaseClone = mappedOption.Clone() as MappedOption;
                        (releaseClone.Trigger as IPressState).PressState = PressState.Release;
                    }
                }
                
                if (mappedOption.Action is IPressState pressAction)
                {
                    if (pressAction.PressState == PressState.LegacyPressAndRelease)
                    {
                        pressAction.PressState = PressState.Press;

                        releaseClone = releaseClone ?? mappedOption.Clone() as MappedOption;
                        (releaseClone.Action as IPressState).PressState = PressState.Release;
                    }
                }

                if(releaseClone != null) 
                { 
                    changed = true;
                    preset.MappedOptions.Add(releaseClone);
                }
            }

            if (changed)
                preset.Save();
        }
    }
}
