using System;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Plugins;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;

namespace Key2Joy.Plugin.Midi.Mapping.Actions;

[Action(
    Description = "MidiInputDevice.InputDeviceListenForEvent",
    NameFormat = "Script actions for listening to MIDI input device events",
    Visibility = MappingMenuVisibility.Never,
    GroupName = "Midi",
    GroupImage = "sound"
)]
public class InputDeviceListenForEvent : PluginAction
{
    /// <markdown-doc>
    /// <parent-name>Midi</parent-name>
    /// <path>Api/Plugins/Midi</path>
    /// </markdown-doc>
    /// <summary>
    /// Listens for events on the specified Midi input device
    /// </summary>
    /// <markdown-example>
    /// Adds an event listener to all input devices and prints to the logs when they fire.
    /// <code language="lua">
    /// <![CDATA[
    /// local devices = Midi.InputDeviceGetAll()
    ///
    /// for _, device in collection(devices) do
    ///     Midi.InputDeviceListenForEvent(device, function(eventType, deltaTime, note, velocity)
    ///         print(eventType, deltaTime, note, velocity)
    ///     end)
    /// end
    /// ]]>
    /// </code>
    /// </markdown-example>
    /// <returns>A collection with input devices</returns>
    /// <name>Midi.InputDeviceListenForEvent</name>
    [ExposesScriptingMethod("Midi.InputDeviceListenForEvent")]
    public void ExecuteForScript(CallbackActionWrapper inputDevice, CallbackActionWrapper callback)
    {
        var device = inputDevice.GetInstance<InputDevice>();
        device.EventReceived += (sender, e) =>
        {
            switch (e.Event)
            {
                // TODO: Add more event types
                // TODO: Create enum that reroutes e.Event.EventType (so the host doesnt need to reference DryWetMidi)
                case NoteOnEvent noteOnEvent:
                    callback.AsDelegateInvoke((byte)e.Event.EventType, noteOnEvent.DeltaTime, Convert.ToByte(noteOnEvent.NoteNumber), Convert.ToByte(noteOnEvent.Velocity));
                    break;

                case NoteOffEvent noteOffEvent:
                    callback.AsDelegateInvoke((byte)e.Event.EventType, noteOffEvent.DeltaTime, Convert.ToByte(noteOffEvent.NoteNumber), Convert.ToByte(noteOffEvent.Velocity));
                    break;

                default:
                    callback.AsDelegateInvoke((byte)e.Event.EventType, e.Event.DeltaTime);
                    break;
            }
        };

        var allModules = AppDomain.CurrentDomain.GetAssemblies();
        if (!device.IsListeningForEvents)
        {
            device.StartEventsListening();
        }
    }
}
