# `Keyboard.GetKeyDown` (`KeyboardKey`)


Tests if the provided keyboard key is currently pressed.

Note: This function currently has trouble distinguishing between left and right keys. This means that `Keyboard.GetKeyDown(KeyboardKey.RightControl)` will return true even if the left control is pressed.

You can find the keycodes in [the KeyboardKey Enumeration](../Enumerations/KeyboardKey.md).


## Parameters

* **key (`KeyboardKey`)** 
	The key to test for

## Returns

True if the key is currently pressed down, false otherwise
## Examples

> Shows how to show all keys currently pressed.
> 
> #### _lua_:
> ```lua
> for keyName, key in pairs(KeyboardKey)do
>    if(Keyboard.GetKeyDown(key))then
>       Print(keyName, "is currently pressed")
>    end
> end
> ```
---

> Shows how to only simulate pressing "A" when shift is also held down. This allows binding to multiple keys, where one is the trigger and the rest of the inputs are checked in the script.
> 
> #### _js_:
> ```js
> if(Keyboard.GetKeyDown(KeyboardKey.Shift)) {
>   GamePad.Simulate(GamePadControl.A, PressState.Press);
> }
> ```
---
