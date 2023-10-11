# `Cursor.GetPosition` ()


Gets the current cursor position



## Returns

A Point object with X and Y properties that represent the cursor X and Y
## Examples

> The code below prints 0, 0 when the cursor is held in the top left of the first monitor.
> 
> #### _js_:
> ```js
> var cursorPosition = Cursor.GetPosition()
> Print(`${cursorPosition.X}, ${cursorPosition.Y}`)
> ```
> 
> #### _lua_:
> ```lua
> local cursorPosition = Cursor.GetPosition()
> print(cursorPosition.X .. ", " .. cursorPosition.Y)
> ```
---
