using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Key2Joy.LowLevelInput
{
    // Source: https://stackoverflow.com/a/2934866
    internal class VirtualKeyConverter
    {
        [DllImport("user32.dll")] 
        static extern short VkKeyScan(char ch);

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, MapVirtualKeyMapTypes uMapType);

        internal static Keys KeysFromVirtual(int virtualKeyCode)
        {
            return (Keys)virtualKeyCode;
        }

        internal static Keys KeysFromScanCode(KeyboardKey scanCode)
        {
            return KeysFromVirtual((int)MapVirtualKey((uint)scanCode, MapVirtualKeyMapTypes.MAPVK_VSC_TO_VK_EX));
        }

        internal static Keys KeysFromChar(char keyChar)
        {
            var helper = new Helper { Value = VkKeyScan(keyChar) };

            byte virtualKeyCode = helper.Low;
            byte shiftState = helper.High;

            Keys keys = (Keys)virtualKeyCode;

            keys |= (shiftState & 1) != 0 ? Keys.Shift : Keys.None;
            keys |= (shiftState & 2) != 0 ? Keys.Control : Keys.None;
            keys |= (shiftState & 4) != 0 ? Keys.Alt : Keys.None;

            return keys;
        }

        internal static string KeysToString(Keys keys)
        {
            return new KeysConverter().ConvertToString(keys);
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct Helper
        {
            [FieldOffset(0)] public short Value;
            [FieldOffset(0)] public byte Low;
            [FieldOffset(1)] public byte High;
        }
    }

    /// <summary>
    /// The set of valid MapTypes used in MapVirtualKey
    /// </summary>
    /// <remarks>
    /// Source: http://pinvoke.net/default.aspx/user32/MapVirtualKey.html?diff=y
    /// </remarks>
    public enum MapVirtualKeyMapTypes : uint
    {
        /// <summary>
        /// uCode is a virtual-key code and is translated into a scan code.
        /// If it is a virtual-key code that does not distinguish between left- and
        /// right-hand keys, the left-hand scan code is returned.
        /// If there is no translation, the function returns 0.
        /// </summary>
        MAPVK_VK_TO_VSC = 0x00,

        /// <summary>
        /// uCode is a scan code and is translated into a virtual-key code that
        /// does not distinguish between left- and right-hand keys. If there is no
        /// translation, the function returns 0.
        /// </summary>
        MAPVK_VSC_TO_VK = 0x01,

        /// <summary>
        /// uCode is a virtual-key code and is translated into an unshifted
        /// character value in the low-order word of the return value. Dead keys (diacritics)
        /// are indicated by setting the top bit of the return value. If there is no
        /// translation, the function returns 0.
        /// </summary>
        MAPVK_VK_TO_CHAR = 0x02,

        /// <summary>
        /// Windows NT/2000/XP: uCode is a scan code and is translated into a
        /// virtual-key code that distinguishes between left- and right-hand keys. If
        /// there is no translation, the function returns 0.
        /// </summary>
        MAPVK_VSC_TO_VK_EX = 0x03,

        /// <summary>
        /// Not currently documented
        /// </summary>
        MAPVK_VK_TO_VSC_EX = 0x04
    }
}
