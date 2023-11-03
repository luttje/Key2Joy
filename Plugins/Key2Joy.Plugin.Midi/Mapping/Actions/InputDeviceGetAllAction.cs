using System;
using System.Collections.Generic;
using System.Linq;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Plugins;
using Melanchall.DryWetMidi.Multimedia;

namespace Key2Joy.Plugin.Midi.Mapping.Actions;

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
    /// Loops all input devices and displays their name in the logs.
    /// <code language="lua">
    /// <![CDATA[
    /// local devices = Midi.InputDeviceGetAll()
    /// for k, v in collection(devices) do
    ///     print(v)
    /// end
    /// ]]>
    /// </code>
    /// </markdown-example>
    /// <returns>A collection with input devices</returns>
    /// <name>Midi.InputDeviceGetAll</name>
    [ExposesScriptingMethod("Midi.InputDeviceGetAll")]
    public ICollection<CallbackActionWrapper> ExecuteForScript() => InputDevice.GetAll().Select(input => new CallbackActionWrapper(input)).ToList();
}
