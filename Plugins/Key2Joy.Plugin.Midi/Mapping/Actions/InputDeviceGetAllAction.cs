using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Plugins;
using Melanchall.DryWetMidi.Multimedia;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Key2Joy.Plugin.Midi.Mapping
{
    [Action(
        Description = "MidiInputDevice.GetAll",
        NameFormat = "Gets all MIDI input devices",
        Visibility = MappingMenuVisibility.Never,
        GroupName = "Midi",
        GroupImage = "sound"
    )]
    public class InputDeviceGetAllAction : PluginAction
    {
        /// <markdown-doc>
        /// <parent-name>Midi</parent-name>
        /// <path>Api/Plugins/Midi</path>
        /// </markdown-doc>
        /// <summary>
        /// Gets all Midi input devices
        /// </summary>
        /// <markdown-example>
        /// <![CDATA[
        /// local devices = Midi.InputDeviceGetAll()
        /// for k, v in collection(devices) do
        ///     print(v)
        /// end
        /// ]]>
        /// </markdown-example>
        /// <returns>A collection with input devices</returns>
        /// <name>InputDeviceGetAll</name>
        [ExposesScriptingMethod("Midi.InputDeviceGetAll")]
        public ICollection<WrappedPluginType> ExecuteForScript()
        {
            return InputDevice.GetAll().Select(input => new WrappedPluginType(input)).ToList();
        }
    }
}
