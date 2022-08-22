# `Mouse.SimulateMove` (`Int32`, `Int32`, `MoveType`)


Simulate moving the mouse


## Parameters

* **x (`Int32`)** 

	X coordinate to move by/to

* **y (`Int32`)** 

	Y coordinate to move by/to

* **moveType (`MoveType`)** 

	Whether to move relative to the current cursor position (default) or to an absolute position on screen


## Examples

> Nudges the cursor 100 pixels to the left from where it is now.
> 
> #### _js_:
> ```js
> Mouse.SimulateMove(-100,0)
> ```
---

> Moves the cursor to an absolute position on the screen.
> 
> #### _lua_:
> ```lua
> Mouse.SimulateMove(1024,1050,MoveType.Absolute)
> ```
---