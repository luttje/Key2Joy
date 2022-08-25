# `Graphics.GetPixelColor` (`Int32`, `Int32`)


Gets the color of a pixel at the given x and y position


## Parameters

* **x (`Int32`)** 

	X position on screen

* **y (`Int32`)** 

	Y position on screen

## Returns

A color object containing Red, Green and Blue color information
## Examples

> Shows the color of the pixel at the mouse position
> 
> #### _lua_:
> ```lua
> local cursor = Cursor.GetPosition()
> local pixelColor = Graphics.GetPixelColor(cursor.X, cursor.Y)
> print(pixelColor.R) -- Red
> print(pixelColor.G) -- Green
> print(pixelColor.B) -- Blue
> print(pixelColor.A) -- Alpha (opacity, always 255)
> ```
---