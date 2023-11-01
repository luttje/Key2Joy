# `GamePad.SimulateTrigger` (```Single```, ```GamePadSide```, ```Int32```)

Simulate pulling back a gamepad trigger

## Parameters
* **delta (```Single```)** 
	The fraction by which to pull the trigger back (between 0 and 1)

* **side (```GamePadSide```)** 
	Which gamepad trigger to pull, either GamePadSide.Left (default) or .Right

* **gamepadIndex (```Int32```)** 
	Which of 4 possible gamepads to simulate: 0 (default), 1, 2 or 3


## Examples
> Pulls the left gamepad trigger halfway back, then resets after 500ms
> 
> #### _lua_:
> ```lua
> GamePad.SimulateTrigger(0.5)
> SetTimeout(function()
>    GamePad.Reset()
> end, 500)
> ```
---
