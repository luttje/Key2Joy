using Key2Joy.Contracts.Mapping;
using System;
using Melanchall.DryWetMidi.Multimedia;
using System.Linq;
using Key2Joy.Contracts.Plugins;
using Melanchall.DryWetMidi.Core;

namespace Key2Joy.Plugin.Midi.Mapping
{
    [Action(
        Description = "MidiInputDevice.InputDeviceListenForEvent",
        NameFormat = "Script actions for listening to MIDI input device events",
        Visibility = MappingMenuVisibility.Never,
        GroupName = "Midi",
        GroupImage = "sound"
    )]
    public class InputDeviceListenForEvent : AbstractAction
    {
        public InputDeviceListenForEvent(string name)
            : base(name)
        {

        }

        /// <markdown-doc>
        /// <parent-name>Midi</parent-name>
        /// <path>Api/Plugins/Midi</path>
        /// </markdown-doc>
        /// <summary>
        /// Listens for events on the specified Midi input device
        /// </summary>
        /// <markdown-example>
        /// <![CDATA[
        /// local devices = Midi.InputDeviceGetAll()
        /// 
        /// for _, device in collection(devices) do
        ///     Midi.InputDeviceListenForEvent(device, function(eventType, deltaTime, note, velocity)
        ///         print(eventType, deltaTime, note, velocity)
        ///     end)
        /// end
        /// ]]>
        /// </markdown-example>
        /// <returns>A collection with input devices</returns>
        /// <name>InputDeviceListenForEvent</name>
        [ExposesScriptingMethod("Midi.InputDeviceListenForEvent")]
        public void ExecuteForScript(WrappedPluginType inputDevice, WrappedPluginType callback)
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
}
