using System;
using System.Linq;

namespace BuildMarkdownDocs.Util
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            return input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input[0].ToString().ToUpper() + input.Substring(1),
            };
        }

        public static string TrimEachLine(this string input)
        {
            return string.Join("\n", input.Split('\n').Select(line => line.Trim()));
        }
    }
}
