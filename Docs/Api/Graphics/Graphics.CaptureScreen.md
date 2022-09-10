# `Graphics.CaptureScreen` (`String`, `Int32?`, `Int32?`, `Int32?`, `Int32?`)


Captures the specified Screen Region in the specified format


## Parameters

* **savePath (`String`)** 

	File path on device where to save the screen capture. The extension you specify decides the format. Supported extensions: .jpeg/.jpg, .png, .bmp, .gif(not animated), .ico, .emf, .exif, .tiff, .wmf

* **x (Optional `Int32`)** 

	X position on screen. Defaults to first monitor X start.

* **y (Optional `Int32`)** 

	Y position on screen. Defaults to first monitor Y start.

* **w (Optional `Int32`)** 

	Width of region to capture. Defaults to (all) screens width.

* **h (Optional `Int32`)** 

	Height of region to capture. Defaults to (all) screens height.


## Examples

> Captures the entire screen to a jpeg file on your desktop
> 
> #### _lua_:
> ```lua
> Graphics.CaptureScreen(Util.PathExpand("%HOMEDRIVE%/%HOMEPATH%/Desktop/capture.jpg"))
> ```
---

> Captures a region of 500 sq pixels around the cursor as a png to the desktop.
> 
> #### _lua_:
> ```lua
> local cursor = Cursor.GetPosition()
> local size = 500
> local halfSize = size * .5
> Graphics.CaptureScreen(
>     Util.PathExpand("%HOMEDRIVE%/%HOMEPATH%/Desktop/region-capture.png"),
>     cursor.X - halfSize,
>     cursor.Y - halfSize,
>     size,
>     size
> )
> ```
---