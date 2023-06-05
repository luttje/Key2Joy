using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Esprima;
using System.Windows.Forms;
using System.IO;
using Key2Joy.Contracts.Mapping;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Capture Screen Region",
        Visibility = MappingMenuVisibility.Never,
        NameFormat = "Capture the specified Screen Region"
    )]
    public class CaptureScreenAction : CoreAction
    {
        public CaptureScreenAction(string name, string description)
            : base(name, description)
        { }

        /// <markdown-doc>
        /// <parent-name>Graphics</parent-name>
        /// <path>Api/Graphics</path>
        /// </markdown-doc>
        /// <summary>
        /// Captures the specified Screen Region in the specified format
        /// </summary>
        /// <markdown-example>
        /// Captures the entire screen to a jpeg file on your desktop
        /// <code language="lua">
        /// <![CDATA[
        /// Graphics.CaptureScreen(Util.PathExpand("%HOMEDRIVE%/%HOMEPATH%/Desktop/capture.jpg"))
        /// ]]>
        /// </code>
        /// </markdown-example>
        /// <markdown-example>
        /// Captures a region of 500 sq pixels around the cursor as a png to the desktop.
        /// <code language="lua">
        /// <![CDATA[
        /// local cursor = Cursor.GetPosition()
        /// local size = 500
        /// local halfSize = size * .5
        /// Graphics.CaptureScreen(
        ///     Util.PathExpand("%HOMEDRIVE%/%HOMEPATH%/Desktop/region-capture.png"),
        ///     cursor.X - halfSize,
        ///     cursor.Y - halfSize,
        ///     size,
        ///     size
        /// )
        /// ]]>
        /// </code>
        /// </markdown-example>
        /// <markdown-example>
        /// Captures a sequence of images from the screen and saves them to a folder on the desktop (frames/)
        /// <code language="lua">
        /// <![CDATA[
        /// local frame = 1
        /// local frameCount = 30
        /// local framesPerSecond = 5
        /// local interval
        /// interval = SetInterval(function()
        ///    Graphics.CaptureScreen(Util.PathExpand("%HOMEDRIVE%/%HOMEPATH%/Desktop/frames/"..frame..".png"))
        ///    frame = frame + 1
        /// 
        ///    if(frame > frameCount)then
        ///       ClearInterval(interval)
        ///    end
        /// end, 1000 / framesPerSecond)
        /// ]]>
        /// </code>
        /// </markdown-example>
        /// <param name="savePath">File path on device where to save the screen capture. The extension you specify decides the format. Supported extensions: .jpeg/.jpg, .png, .bmp, .gif(not animated), .ico, .emf, .exif, .tiff, .wmf</param>
        /// <param name="x">X position on screen. Defaults to first monitor X start.</param>
        /// <param name="y">Y position on screen. Defaults to first monitor Y start.</param>
        /// <param name="w">Width of region to capture. Defaults to (all) screens width.</param>
        /// <param name="h">Height of region to capture. Defaults to (all) screens height.</param>
        /// <name>Graphics.CaptureScreen</name>
        [ExposesScriptingMethod("Graphics.CaptureScreen")]
        public void ExecuteForScript(
            string savePath, 
            int? x = null, int? y = null, 
            int? w = null, int? h = null)
        {
            var format = ImageFormat.Jpeg;

            if (savePath.EndsWith(".png"))
                format = ImageFormat.Png;
            else if (savePath.EndsWith(".bmp"))
                format = ImageFormat.Bmp;
            else if (savePath.EndsWith(".gif"))
                format = ImageFormat.Gif;
            else if (savePath.EndsWith(".ico"))
                format = ImageFormat.Icon;
            else if (savePath.EndsWith(".emf"))
                format = ImageFormat.Emf;
            else if (savePath.EndsWith(".exif"))
                format = ImageFormat.Exif;
            else if (savePath.EndsWith(".tiff"))
                format = ImageFormat.Tiff;
            else if (savePath.EndsWith(".wmf"))
                format = ImageFormat.Wmf;

            if (x == null)
                x = SystemInformation.VirtualScreen.X;

            if (y == null)
                y = SystemInformation.VirtualScreen.Y;

            if (w == null)
                w = SystemInformation.VirtualScreen.Width;

            if (h == null)
                h = SystemInformation.VirtualScreen.Height;
            
            var pixelCache = new Bitmap((int)w, (int)h);
            var bounds = new Rectangle((int)x, (int)y, (int)w, (int)h);

            lock (BaseScriptAction.LockObject)
                using (var g = Graphics.FromImage(pixelCache))
                    g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);

            var directory = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            pixelCache.Save(savePath, format);
        }

        public override async Task Execute(IInputBag inputBag = null)
        {
            // TODO: Currently this is only a script action...
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CaptureScreenAction action))
                return false;

            return true;
        }

        public override object Clone()
        {
            return new CaptureScreenAction(Name, description)
            {
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
