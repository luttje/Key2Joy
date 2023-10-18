using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Mapping.Actions.Scripting;
using Key2Joy.Util;

namespace Key2Joy.Mapping.Actions.Graphics;

[Action(
    Description = "Capture Screen Region",
    Visibility = MappingMenuVisibility.Never,
    NameFormat = "Capture the specified Screen Region"
)]
public class CaptureScreenAction : CoreAction
{
    public CaptureScreenAction(string name)
        : base(name)
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
        var format = FileSystem.GetImageFormatFromExtension(
            Path.GetExtension(savePath)
        );

        x ??= SystemInformation.VirtualScreen.X;

        y ??= SystemInformation.VirtualScreen.Y;

        w ??= SystemInformation.VirtualScreen.Width;

        h ??= SystemInformation.VirtualScreen.Height;

        Bitmap pixelCache = new((int)w, (int)h);
        Rectangle bounds = new((int)x, (int)y, (int)w, (int)h);

        lock (BaseScriptAction.LockObject)
        {
            using var g = System.Drawing.Graphics.FromImage(pixelCache);
            g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
        }

        var directory = Path.GetDirectoryName(savePath);
        if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        pixelCache.Save(savePath, format);
    }

    public override bool Equals(object obj)
    {
        if (obj is not CaptureScreenAction)
        {
            return false;
        }

        return true;
    }
}
