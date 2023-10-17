namespace Key2Joy.Contracts.Util;

public static class StringExtensions
{
    public static string Ellipsize(this string input, int maxLength)
    {
        if (input == null)
        {
            return null;
        }

        if (input.Length <= maxLength)
        {
            return input;
        }

        if (maxLength < 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(maxLength));
        }

        if (maxLength <= 3)
        {
            return "...";
        }

        return input.Substring(0, maxLength - 3) + "...";
    }
}
