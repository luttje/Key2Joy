namespace KeyToJoy.Input
{
    internal enum AxisDirection
    {
        // This is purposefully a lot higher than the highest values in System.Windows.Forms.Keys
        Up = 0xFFFF00, 
        Right = 0xFFFF01, 
        Down = 0xFFFF02, 
        Left = 0xFFFF03
    }
}
