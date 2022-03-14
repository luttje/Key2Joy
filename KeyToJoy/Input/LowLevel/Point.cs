using System.Runtime.InteropServices;

namespace KeyToJoy.Input.LowLevel
{
    // Source: https://stackoverflow.com/a/34384189
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        /// <summary>
        /// The x-coordinate of the point.
        /// </summary>
        public int X;

        /// <summary>
        /// The y-coordinate of the point.
        /// </summary>
        public int Y;
    }
}
