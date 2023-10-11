# `Midi.InputDeviceGetAll` ()
> **Note**
> This is a plugin, meaning it's functionality is disabled by default.
> You can enable plugins by going to `View` > `Plugins` > `Manage Plugins`.

Gets all Midi input devices



## Returns

A collection with input devices
## Examples

> Loops all input devices and displays their name in the logs.
> 
> #### _lua_:
> ```lua
> local devices = Midi.InputDeviceGetAll()
> for k, v in collection(devices) do
>     print(v)
> end
> ```
---
