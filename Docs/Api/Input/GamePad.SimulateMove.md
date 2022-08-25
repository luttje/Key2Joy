# `GamePad.SimulateMove` (`Double`, `Double`, `GamePadStick`, `Int32`)


Simulate moving a gamepad joystick


## Parameters

* **deltaX (`Double`)** 

	The fraction by which to move the stick forward (negative) or backward (positive)

* **deltaY (`Double`)** 

	The fraction by which to move the stick right (positive) or left (negative)

* **stick (`GamePadStick`)** 

	Which gamepad stick to move, either GamePadStick.Left (default) or .Right

* **gamepadIndex (`Int32`)** 

	Which of 4 possible gamepads to simulate (0, 1, 2 or 3)


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