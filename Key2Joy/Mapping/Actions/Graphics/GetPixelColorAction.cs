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

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Get Pixel Color",
        Visibility = MappingMenuVisibility.Never,
        NameFormat = "Get a Pixel Color"
    )]
    internal class GetPixelColorAction : BaseAction
    {
        private Bitmap pixelCache = new Bitmap(1, 1);
        
        public GetPixelColorAction(string name, string description)
            : base(name, description)
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
            var bounds = new Rectangle(x, y, 1, 1);

            lock (BaseScriptAction.LockObject)
                using (var g = Graphics.FromImage(pixelCache))
                    g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
            
            return pixelCache.GetPixel(0, 0);
        }

        internal override async Task Execute(IInputBag inputBag = null)
        {
            // TODO: Currently this is only a script action...
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GetPixelColorAction action))
                return false;

            return true;
        }

        public override object Clone()
        {
            return new GetPixelColorAction(Name, description)
            {
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
