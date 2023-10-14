# `Mouse.GetButtonDown` (```Buttons```)

Tests if the provided mouse button is currently pressed.
You can find the button codes in [../Enumerations/Buttons.md](../Enumerations/Buttons.md).

## Parameters
* **button (```Buttons```)** 
	The button to test for

## Returns
```Boolean```
True if the button is currently pressed down, false otherwise.

## Examples
> Shows how to show all mouse buttons currently pressed.
> 
> #### _lua_:
> ```lua
> for buttonName, button in pairs(Buttons)do
>    if(Buttons.GetButtonDown(button))then
>       Print(buttonName, "is currently pressed")
>    end
> end
> ```
---
