# `Graphics.AnimationFromImages` (`String`, `Double`, `Object`)


Encodes a sequence of images into an animation (like a gif).

You can save as a gif, mp4, avi, flv and more. See the supported formats in ffmpeg here: https://ffmpeg.org/ffmpeg-formats.html#Muxers.

Depending on the amount of frames this will take a bit of time. It may cause a stutter in performance while it's encoding the animation.


## Parameters

* **savePath (`String`)** 

	File path on device where to save the animation. The file extension instructs the format (e.g: .gif).

* **frameRate (`Double`)** 

	Framecount per second.

* **framePaths (`Object`)** 

	Array/Table of paths to where the frames are located


## Examples

> Creates a gif on the desktop (animated.gif) with each frame being a different image that was already saved in a folder (frames/) on the desktop.
> 
> Since the framerate is set to 30 and there are 30 images, the gif will last 1 second.
> 
> #### _lua_:
> ```lua
> local desktopDir = Util.PathExpand("%HOMEDRIVE%%HOMEPATH%/Desktop/")
> local images = {}
> 
> for i=1,30 do 
>   images[i] = desktopDir.."frames/"..i..".png"
> end
> 
> Graphics.AnimationFromImages(desktopDir.."animated.gif", 30, images)
> ```
> 
> #### _js_:
> ```js
> let desktopDir = Util.PathExpand("%HOMEDRIVE%%HOMEPATH%/Desktop/")
> let images = []
> 
> for(let i=1; i<=30; i++){ 
>   images[i] = desktopDir+"frames/"+i+".png"
> }
> 
> Graphics.AnimationFromImages(desktopDir+"animated.gif", 30, images)
> ```
---