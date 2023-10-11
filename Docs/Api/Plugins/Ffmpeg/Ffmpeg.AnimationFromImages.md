# `Ffmpeg.AnimationFromImages` (`String`, `Double`, `Object`)
> **Note**
> This is a plugin, meaning it's functionality is disabled by default.
> You can enable plugins by going to `View` > `Plugins` > `Manage Plugins`.

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
> -- Only works if the FFmpeg plugin has access to desktop:
> -- local targetDirectory = Util.PathExpand("%HOMEDRIVE%%HOMEPATH%/Desktop/")
> local targetDirectory = "./"
> local images = {}
>             
> for i=1,30 do
>   images[i] = targetDirectory.."frames/"..i..".png"
> end
>             
> Ffmpeg.AnimationFromImages(targetDirectory.."animated.gif", 30, images)
> ```
> 
> #### _js_:
> ```js
> // Only works if the FFmpeg plugin has access to desktop:
> // let targetDirectory = Util.PathExpand("%HOMEDRIVE%%HOMEPATH%/Desktop/")
> let targetDirectory = "./"
> let images = []
>             
> for(let i=1; i<=30; i++){
>   images[i-1] = targetDirectory+"frames/"+i+".png"
> }
>             
> Ffmpeg.AnimationFromImages(targetDirectory+"animated.gif", 30, images)
> ```
---
