using System.Runtime.InteropServices;

namespace KeyToJoy
{
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
