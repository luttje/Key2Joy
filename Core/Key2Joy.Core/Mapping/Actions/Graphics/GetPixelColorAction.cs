using System;
using System.Drawing;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Mapping.Actions.Scripting;

namespace Key2Joy.Mapping.Actions.Graphics;

[Action(
    Description = "Get Pixel Color",
    Visibility = MappingMenuVisibility.Never,
    NameFormat = "Get a Pixel Color"
)]
public class GetPixelColorAction : CoreAction
{
    private readonly Bitmap pixelCache = new(1, 1);

    public GetPixelColorAction(string name)
        : base(name)
    { }

    /// <markdown-doc>
    /// <parent-name>Graphics</parent-name>
    /// <path>Api/Graphics</path>
    /// </markdown-doc>
    /// <summary>
    /// Gets the color of a pixel at the given x and y position
    /// </summary>
    /// <markdown-example>
    /// Shows the color of the pixel at the mouse position
    /// <code language="lua">
    /// <![CDATA[
    /// local cursor = Cursor.GetPosition()
    /// local pixelColor = Graphics.GetPixelColor(cursor.X, cursor.Y)
    /// print(pixelColor.R) -- Red
    /// print(pixelColor.G) -- Green
    /// print(pixelColor.B) -- Blue
    /// print(pixelColor.A) -- Alpha (opacity, always 255)
    /// ]]>
    /// </code>
    /// </markdown-example>
    /// <returns>A color object containing Red, Green and Blue color information</returns>
    /// <param name="x">X position on screen</param>
    /// <param name="y">Y position on screen</param>
    /// <name>Graphics.GetPixelColor</name>
    [ExposesScriptingMethod("Graphics.GetPixelColor")]
    public Color ExecuteForScript(int x, int y)
    {
        Rectangle bounds = new(x, y, 1, 1);

        lock (BaseScriptAction.LockObject)
        {
            using var g = System.Drawing.Graphics.FromImage(this.pixelCache);
            g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
        }

        return this.pixelCache.GetPixel(0, 0);
    }

    public override async Task Execute(AbstractInputBag inputBag = null)
    {
        // TODO: Currently this is only a script action...
    }

    public override bool Equals(object obj)
    {
        if (obj is not GetPixelColorAction)
        {
            return false;
        }

        return true;
    }
}
