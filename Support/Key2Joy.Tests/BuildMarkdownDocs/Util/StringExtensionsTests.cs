using System;
using BuildMarkdownDocs.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.BuildMarkdownDocs.Util;

[TestClass]
public class StringExtensionsTests
{
    [TestMethod]
    public void FirstCharToUpper_InputIsNull_ThrowsArgumentNullException()
    {
        string input = null;

        Assert.ThrowsException<ArgumentNullException>(input.FirstCharToUpper);
    }

    [TestMethod]
    public void FirstCharToUpper_InputIsEmpty_ThrowsArgumentException()
    {
        var input = "";

        Assert.ThrowsException<ArgumentException>(input.FirstCharToUpper);
    }

    [TestMethod]
    public void FirstCharToUpper_InputIsSingleCharacter_ReturnsUppercaseCharacter()
    {
        var input = "a";

        var result = input.FirstCharToUpper();

        Assert.AreEqual("A", result);
    }

    [TestMethod]
    public void FirstCharToUpper_InputIsMixedCase_ReturnsStringWithFirstCharUppercase()
    {
        var input = "helloWorld";

        var result = input.FirstCharToUpper();

        Assert.AreEqual("HelloWorld", result);
    }

    [TestMethod]
    public void FirstCharToUpper_InputIsAlreadyUppercase_ReturnsSameString()
    {
        var input = "HELLO";

        var result = input.FirstCharToUpper();

        Assert.AreEqual("HELLO", result);
    }

    [TestMethod]
    public void FirstCharToUpper_InputWithTrailingWhitespace_ReturnsStringWithFirstCharUppercase()
    {
        var input = "world   ";

        var result = input.FirstCharToUpper();

        Assert.AreEqual("World   ", result);
    }

    [TestMethod]
    public void FirstCharToUpper_InputWithSpecialCharacters_ReturnsStringWithFirstCharUppercase()
    {
        var input = "@hello#";

        var result = input.FirstCharToUpper();

        Assert.AreEqual("@hello#", result);
    }

    [TestMethod]
    public void FirstCharToUpper_InputWithNumbers_ReturnsStringWithFirstCharUppercase()
    {
        var input = "123abc";

        var result = input.FirstCharToUpper();

        Assert.AreEqual("123abc", result);
    }

    [TestMethod]
    public void FirstCharToUpper_ValidInput_CapitalizesFirstCharacter()
    {
        var input = "hello";
        var expected = "Hello";

        var result = input.FirstCharToUpper();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void FirstCharToUpper_NullInput_ThrowsArgumentNullException()
    {
        string input = null;
        input.FirstCharToUpper();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FirstCharToUpper_EmptyString_ThrowsArgumentException()
    {
        var input = "";
        input.FirstCharToUpper();
    }

    [TestMethod]
    public void TrimEachLine_InputIsEmpty_ReturnsEmptyString()
    {
        var input = "";

        var result = input.TrimEachLine();

        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void TrimEachLine_InputHasNoLeadingWhitespace_ReturnsSameString()
    {
        var input = "Line 1\nLine 2\nLine 3";

        var result = input.TrimEachLine();

        Assert.AreEqual("Line 1\nLine 2\nLine 3", result);
    }

    [TestMethod]
    public void TrimEachLine_InputHasLeadingWhitespace_ReturnsStringWithLeadingWhitespaceRemoved()
    {
        var input = "   Line 1\n   Line 2\n   Line 3";

        var result = input.TrimEachLine();

        Assert.AreEqual("Line 1\nLine 2\nLine 3", result);
    }

    [TestMethod]
    public void TrimEachLine_InputHasInternalWhitespace_ReturnsStringWithInternalWhitespaceTrimmed()
    {
        var input = "   Line 1\n   Line 2\n   Line 3";

        var result = input.TrimEachLine();

        Assert.AreEqual("Line 1\nLine 2\nLine 3", result);
    }

    [TestMethod]
    public void TrimEachLine_InputHasSpecialCharacters_ReturnsStringWithSpecialCharacters()
    {
        var input = "@Line 1\n#Line 2\n$Line 3";

        var result = input.TrimEachLine();

        Assert.AreEqual("@Line 1\n#Line 2\n$Line 3", result);
    }

    [TestMethod]
    public void TrimEachLine_InputHasMixedWhitespaceCharacters_ReturnsStringWithWhitespaceTrimmed()
    {
        var input = "\t   Line 1\n  \t   Line 2\n      Line 3";

        var result = input.TrimEachLine();

        Assert.AreEqual("Line 1\nLine 2\nLine 3", result);
    }

    [TestMethod]
    public void TrimEachLine_ValidInput_TrimsStartOfEachLine()
    {
        var input = "  hello\n  world";
        var expected = "hello\nworld";

        var result = input.TrimEachLine();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TrimEachLine_NoExtraWhitespaceAtStart_ReturnsSameString()
    {
        var input = "hello\nworld";
        var expected = input;

        var result = input.TrimEachLine();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TrimEachLine_OnlyOneWhitespaceAtStart_ReturnsSameString()
    {
        var input = " hello\n world";
        var expected = input;

        var result = input.TrimEachLine();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TrimEndEachLine_ValidInput_TrimsEndOfEachLine()
    {
        var input = "hello  \nworld  ";
        var expected = "hello\nworld";

        var result = input.TrimEndEachLine();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TrimEndEachLine_NoExtraWhitespaceAtEnd_ReturnsSameString()
    {
        var input = "hello\nworld";
        var expected = input;

        var result = input.TrimEndEachLine();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TrimEndEachLine_OnlyNewLineAtEnd_ReturnsSameString()
    {
        var input = "hello\nworld\n";
        var expected = input;

        var result = input.TrimEndEachLine();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void NormalizeNewlinesToLF_ValidInput_ReplacesWithLF()
    {
        var input = "hello\r\nworld\nhi\rthere";
        var expected = "hello\nworld\nhi\nthere";

        var result = input.NormalizeNewlinesToLF();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void NormalizeNewlinesToLF_NoCarriageReturn_ReturnsSameString()
    {
        var input = "hello\nworld";
        var expected = input;

        var result = input.NormalizeNewlinesToLF();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void NormalizeNewlinesToLF_OnlyCarriageReturn_ReplacesWithLF()
    {
        var input = "hello\rworld";
        var expected = "hello\nworld";

        var result = input.NormalizeNewlinesToLF();

        Assert.AreEqual(expected, result);
    }
}
