namespace Key2Joy.Mapping
{
    public enum AxisDirection
    {
        None = 0x000000,
        
        // This is purposefully a lot higher than the highest values in System.Windows.Forms.Keys (so they wont conflict and we can mix them both)
        Forward = 0xFFFF00, 
        Right = 0xFFFF01, 
        Backward = 0xFFFF02, 
        Left = 0xFFFF03
    }
}
