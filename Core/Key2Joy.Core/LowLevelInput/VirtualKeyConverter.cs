using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Key2Joy.LowLevelInput;

public interface IVirtualKeyService
{
    short VkKeyScan(char ch);

    uint MapVirtualKey(uint uCode, MapVirtualKeyMapTypes uMapType);
}

[ExcludeFromCodeCoverage]
public class NativeVirtualKeyService : IVirtualKeyService
{
    [DllImport("user32.dll", EntryPoint = "VkKeyScan")]
    public static extern short InternalVkKeyScan(char ch);

    [DllImport("user32.dll", EntryPoint = "MapVirtualKey")]
    public static extern uint InternalMapVirtualKey(uint uCode, MapVirtualKeyMapTypes uMapType);

    public short VkKeyScan(char ch) => InternalVkKeyScan(ch);

    public uint MapVirtualKey(uint uCode, MapVirtualKeyMapTypes uMapType) => InternalMapVirtualKey(uCode, uMapType);
}

// Based on: https://stackoverflow.com/a/2934866
public class VirtualKeyConverter
{
    private const short IsEitherShiftPressed = 0b00000001;
    private const short IsEitherControlPressed = 0b00000010;
    private const short IsEitherAltPressed = 0b00000100;
    private const short IsHankakuKeyPressed = 0b00001000;

    private readonly IVirtualKeyService service;

    public VirtualKeyConverter(IVirtualKeyService service = null)
        => this.service = service ?? new NativeVirtualKeyService();

    public Keys KeysFromVirtual(int virtualKeyCode) => (Keys)virtualKeyCode;

    public Keys KeysFromScanCode(KeyboardKey scanCode)
        => this.KeysFromVirtual((int)this.service.MapVirtualKey((uint)scanCode, MapVirtualKeyMapTypes.MAPVK_VSC_TO_VK_EX));

    public Keys KeysFromChar(char keyChar)
    {
        Helper helper = new() { Value = this.service.VkKeyScan(keyChar) };

        var virtualKeyCode = helper.Low;
        var shiftState = helper.High;

        var keys = (Keys)virtualKeyCode;

        keys |= (shiftState & IsEitherShiftPressed) != 0 ? Keys.Shift : Keys.None;
        keys |= (shiftState & IsEitherControlPressed) != 0 ? Keys.Control : Keys.None;
        keys |= (shiftState & IsEitherAltPressed) != 0 ? Keys.Alt : Keys.None;
        keys |= (shiftState & IsHankakuKeyPressed) != 0 ? Keys.HanjaMode : Keys.None; // TODO: Is this correct?

        return keys;
    }

    public string KeysToString(Keys keys) => new KeysConverter().ConvertToString(keys);

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
