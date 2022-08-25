# `GamePad.Reset` (`Int32`)


Reset the gamepad so the stick returns to the resting position (0,0)


## Parameters

* **gamepadIndex (`Int32`)** 

	Which of 4 possible gamepads to reset (0, 1, 2 or 3)


## Examples

> Moves the left gamepad joystick halfway down and to the right, then resets after 500ms
> 
> #### _lua_:
> ```lua
> GamePad.SimulateMove(0.5,0.5)
> SetTimeout(function()
>    GamePad.Reset()
> end, 500)
> ```
---