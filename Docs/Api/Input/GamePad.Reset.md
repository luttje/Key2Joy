# `GamePad.Reset` ()


Reset the gamepad so the stick returns to the resting position (0,0)




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