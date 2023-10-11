using System;
using FFMpegCore;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Plugins;

namespace Key2Joy.Plugin.Ffmpeg.Mapping.Actions;

[Action(
    Description = "Create a gif from a sequence of images",
    Visibility = MappingMenuVisibility.Never,
    NameFormat = "Create a gif from a sequence of images"
)]
public class AnimationFromImagesAction : PluginAction
{
    /// <markdown-doc>
    /// <parent-name>Ffmpeg</parent-name>
    /// <path>Api/Ffmpeg</path>
    /// </markdown-doc>
    /// <summary>
    /// Encodes a sequence of images into an animation (like a gif).
    ///
    /// You can save as a gif, mp4, avi, flv and more. See the supported formats in ffmpeg here: https://ffmpeg.org/ffmpeg-formats.html#Muxers.
    ///
    /// Depending on the amount of frames this will take a bit of time. It may cause a stutter in performance while it's encoding the animation.
    /// </summary>
    /// <markdown-example>
    /// Creates a gif on the desktop (animated.gif) with each frame being a different image that was already saved in a folder (frames/) on the desktop.
    ///
    /// Since the framerate is set to 30 and there are 30 images, the gif will last 1 second.
    /// <code language="lua">
    /// <![CDATA[
    /// -- Only works if the FFmpeg plugin has access to desktop (disabled by default):
    /// -- local targetDirectory = Util.PathExpand("%HOMEDRIVE%%HOMEPATH%/Desktop/")
    /// local targetDirectory = "./"
    /// local images = {}
    ///
    /// for i=1,30 do
    ///   images[i] = targetDirectory.."frames/"..i..".png"
    /// end
    ///
    /// Ffmpeg.AnimationFromImages(targetDirectory.."animated.gif", 30, images)
    /// ]]>
    /// </code>
    /// <code language="js">
    /// <![CDATA[
    /// // Only works if the FFmpeg plugin has access to desktop (disabled by default):
    /// // let targetDirectory = Util.PathExpand("%HOMEDRIVE%%HOMEPATH%/Desktop/")
    /// let targetDirectory = "./"
    /// let images = []
    ///
    /// for(let i=1; i<=30; i++){
    ///   images[i-1] = targetDirectory+"frames/"+i+".png"
    /// }
    ///
    /// Ffmpeg.AnimationFromImages(targetDirectory+"animated.gif", 30, images)
    /// ]]>
    /// </code>
    /// </markdown-example>
    /// <param name="savePath">File path on device where to save the animation. The file extension instructs the format (e.g: .gif).</param>
    /// <param name="frameRate">Framecount per second.</param>
    /// <param name="framePaths">Array/Table of paths to where the frames are located</param>
    /// <name>Ffmpeg.AnimationFromImages</name>
    [ExposesScriptingMethod("Ffmpeg.AnimationFromImages")]
    public void ExecuteForScript(string savePath, double frameRate, object framePaths)
    {
        string[] allFramePaths;

        // WORKAROUND: for NLua not converting a table to array
        // Ideally we would just have the third parameter framePathsObj be: string[] framePaths
        // But Lua does not recognize a table as suitable for a string[] and if we use IEnumerable it mangles the type
        // TODO: Here's to hoping it just works (the following was commented after migrating this action to plugin)
        //if (framePaths is LuaTable table)
        //{
        //    allFramePaths = new string[table.Values.Count];
        //    table.Values.CopyTo(allFramePaths, 0);
        //}
        //else
        allFramePaths = Array.ConvertAll((object[])framePaths, fp => (string)fp);
        // End of WORKAROUND

        this.JoinImageSequence(savePath, frameRate, allFramePaths);
    }

    /// <remarks>
    /// This code is copied from https://github.com/rosenbjerg/FFMpegCore/blob/9f34591a03ef5db49b4b993122e03cbd8a7fa8c4/FFMpegCore/FFMpeg/FFMpeg.cs#L309
    /// and modified to add the framerate before the input file. Otherwise not all frames will show.
    /// </remarks>
    /// <summary>
    /// Converts an image sequence to a video.
    /// </summary>
    /// <param name="output">Output video file.</param>
    /// <param name="frameRate">FPS</param>
    /// <param name="images">Image sequence collection</param>
    /// <returns>Output video information.</returns>
    private bool JoinImageSequence(string output, double frameRate = 30, params string[] images)
    {
        try
        {
            return FFMpeg.JoinImageSequence(output, frameRate, images);
        }
        catch (Exception ex)
        {
            throw PluginException.FromException(this.Plugin, ex);
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is not AnimationFromImagesAction)
        {
            return false;
        }

        return true;
    }
}
