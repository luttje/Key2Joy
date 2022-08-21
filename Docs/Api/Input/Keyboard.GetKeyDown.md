# `Keyboard.GetKeyDown` (`KeyboardKey`)


Tests if the provided keyboard key is currently pressed.

Note: This function currently has trouble distinguishing between left and right keys. This means that `Keyboard.GetKeyDown(KeyboardKey.RightControl)` will return true even if the left control is pressed.

You can find the keycodes in [../Enumerations/KeyboardKey.md](../Enumerations/KeyboardKey.md) .


## Parameters

* **key (`KeyboardKey`)** 

	The key to test for

## Returns

True if the key is currently pressed down, false otherwise
## Examples

Shows how to show all keys currently pressed.

```lua
for keyName, key in pairs(KeyboardKey)do
   if(Keyboard.GetKeyDown(key))then
      Print(keyName, "is currently pressed")
   end
end
```