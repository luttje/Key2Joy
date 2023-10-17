using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BuildMarkdownDocs.Util;

public static class StringExtensions
{
    public static string FirstCharToUpper(this string input) => input switch
    {
        null => throw new ArgumentNullException(nameof(input)),
        "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
        _ => input[0].ToString().ToUpper() + input.Substring(1),
    };

    /// <summary>
    /// Trims multiple whitespaces before each line, but allows a single space to exist
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string TrimEachLine(this string input)
        => string.Join("\n", input.Split('\n').Select(line => Regex.Replace(line, @"^\s{2,}", "")));

    /// <summary>
    /// Trims multiple whitespaces after each line
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string TrimEndEachLine(this string input)
        => string.Join("\n", input.Split('\n').Select(line => Regex.Replace(line, @"\s+$", "")));

    /// <summary>
    /// Replace different newline characters with LF
    /// </summary>
    /// <param name="text"></param>
    public static string NormalizeNewlinesToLF(this string text)
        => Regex.Replace(text, @"\r\n?|\n", "\n");
}
