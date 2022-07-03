using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Key2Joy.LowLevelInput
{
    // Source: https://stackoverflow.com/a/2934866
    internal class VirtualKeyConverter
    {
        [DllImport("user32.dll")] static extern short VkKeyScan(char ch);

        internal static Keys KeysFromVirtual(int virtualKeyCode)
        {
            return (Keys)virtualKeyCode;
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
}
