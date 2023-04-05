using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Esprima;
using System.Windows.Forms;
using FFMpegCore;
using FFMpegCore.Helpers;
using System.IO;
using FFMpegCore.Enums;
using NLua;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Create a gif from a sequence of images",
        Visibility = MappingMenuVisibility.Never,
        NameFormat = "Create a gif from a sequence of images"
    )]
    public class AnimationFromImagesAction : BaseAction
    {
        public AnimationFromImagesAction(string name, string description)
            : base(name, description)
        { }

        /// <markdown-doc>
        /// <parent-name>Graphics</parent-name>
        /// <path>Api/Graphics</path>
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
        /// local desktopDir = Util.PathExpand("%HOMEDRIVE%%HOMEPATH%/Desktop/")
        /// local images = {}
        /// 
        /// for i=1,30 do 
        ///   images[i] = desktopDir.."frames/"..i..".png"
        /// end
        /// 
        /// Graphics.AnimationFromImages(desktopDir.."animated.gif", 30, images)
        /// ]]>
        /// </code>
        /// <code language="js">
        /// <![CDATA[
        /// let desktopDir = Util.PathExpand("%HOMEDRIVE%%HOMEPATH%/Desktop/")
        /// let images = []
        /// 
        /// for(let i=1; i<=30; i++){ 
        ///   images[i] = desktopDir+"frames/"+i+".png"
        /// }
        /// 
        /// Graphics.AnimationFromImages(desktopDir+"animated.gif", 30, images)
        /// ]]>
        /// </code>
        /// </markdown-example>
        /// <param name="savePath">File path on device where to save the animation. The file extension instructs the format (e.g: .gif).</param>
        /// <param name="frameRate">Framecount per second.</param>
        /// <param name="framePaths">Array/Table of paths to where the frames are located</param>
        /// <name>Graphics.AnimationFromImages</name>
        [ExposesScriptingMethod("Graphics.AnimationFromImages")]
        public void ExecuteForScript(string savePath, double frameRate, object framePaths)
        {
            string[] allFramePaths;
            
            // WORKAROUND: for NLua not converting a table to array
            // Ideally we would just have the third parameter framePathsObj be: string[] framePaths
            // But Lua does not recognize a table as suitable for a string[] and if we use IEnumerable it mangles the type
            if (framePaths is LuaTable table)
            {
                allFramePaths = new string[table.Values.Count];
                table.Values.CopyTo(allFramePaths, 0);
            }
            else
                allFramePaths = Array.ConvertAll((object[])framePaths, fp => (string)fp);
            // End of WORKAROUND

            JoinImageSequence(savePath, frameRate, allFramePaths);
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
        private static bool JoinImageSequence(string output, double frameRate = 30, params string[] images)
        {
            return FFMpeg.JoinImageSequence(output, frameRate, images);
        }

        public override async Task Execute(IInputBag inputBag = null)
        {
            // TODO: Currently this is only a script action...
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AnimationFromImagesAction action))
                return false;

            return true;
        }

        public override object Clone()
        {
            return new AnimationFromImagesAction(Name, description)
            {
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
