# `GamePad.SimulateMove` (```Int16```, ```Int16```, ```GamePadSide```, ```Int32```)

Simulate moving a gamepad joystick

## Parameters
* **deltaX (```Int16```)** 
	The fraction by which to move the stick forward (negative) or backward (positive)

* **deltaY (```Int16```)** 
	The fraction by which to move the stick right (positive) or left (negative)

* **side (```GamePadSide```)** 
	Which gamepad stick to move, either GamePadSide.Left (default) or .Right

* **gamepadIndex (```Int32```)** 
	Which of 4 possible gamepads to simulate: 0 (default), 1, 2 or 3


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
