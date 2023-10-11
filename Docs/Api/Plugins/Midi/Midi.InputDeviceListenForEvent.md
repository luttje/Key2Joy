# `Midi.InputDeviceListenForEvent` ()
> **Note**
> This is a plugin, meaning it's functionality is disabled by default.
> You can enable plugins by going to `View` > `Plugins` > `Manage Plugins`.

Listens for events on the specified Midi input device



## Returns

A collection with input devices
## Examples

> Adds an event listener to all input devices and prints to the logs when they fire.
> 
> #### _lua_:
> ```lua
> local devices = Midi.InputDeviceGetAll()
>             
> for _, device in collection(devices) do
>     Midi.InputDeviceListenForEvent(device, function(eventType, deltaTime, note, velocity)
>         print(eventType, deltaTime, note, velocity)
>     end)
> end
> ```
---
