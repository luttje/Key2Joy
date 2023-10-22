namespace Key2Joy.Contracts.Util;

public static class StringExtensions
{
    /// <summary>
    /// Ellipsizes the string to the specified length, adding ... at the end
    /// </summary>
    /// <param name="input"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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

    /// <summary>
    /// Places newlines in the string to make it fit the specified width
    /// </summary>
    /// <param name="input"></param>
    /// <param name="maxCharLength"></param>
    /// <returns></returns>
    public static string Wrap(this string input, int maxCharLength)
    {
        if (input == null)
        {
            return null;
        }

        if (maxCharLength <= 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(maxCharLength));
        }

        if (input.Length <= maxCharLength)
        {
            return input;
        }

        var sb = new System.Text.StringBuilder();
        var words = input.Split(' ');
        var lineLength = 0;
        foreach (var word in words)
        {
            if (lineLength + word.Length > maxCharLength)
            {
                sb.AppendLine();
                lineLength = 0;
            }

            sb.Append(word);
            sb.Append(' ');
            lineLength += word.Length + 1;
        }

        return sb.ToString().TrimEnd();
    }
}
