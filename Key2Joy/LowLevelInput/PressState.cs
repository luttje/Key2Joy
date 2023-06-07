using Key2Joy.Contracts.Mapping;
using Key2Joy.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        /// The key/button is pressed down
        /// </summary>
        Press,

        /// <summary>
        /// The key/button is released (after having been pressed down)
        /// </summary>
        Release,
    }

    public class LegacyPressStateConverter : JsonConverter<PressState>
    {
        public override PressState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String && reader.GetString() == "PressAndRelease")
            {
                return PressState.LegacyPressAndRelease;
            }

            return (PressState)Enum.Parse(typeof(PressState), reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, PressState value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }

        public static PressState[] GetPressStatesWithoutLegacy()
        {
            var values = new PressState[2];
            int i = 0;
            foreach (PressState value in Enum.GetValues(typeof(PressState)))
            {
                if (value != PressState.LegacyPressAndRelease)
                    values[i++] = value;
            }

            return values;
        }

        public static void UpdateLegacyIfApplicable(MappingProfile profile)
        {
            bool changed = false;
            var listCopy = new List<AbstractMappedOption>(profile.MappedOptions);

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
                    profile.MappedOptions.Add(releaseClone);
                }
            }

            if (changed)
                profile.Save();
        }
    }
}
