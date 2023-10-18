using System;
using Key2Joy.Contracts.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Contracts.Util;

[TestClass]
public class StringExtensionsTests
{
    [TestMethod]
    public void Ellipsize_ReturnsInputString_WhenMaxLengthIsGreaterThanStringLength()
    {
        var input = "Hello, World!";
        var maxLength = 15;

        var result = input.Ellipsize(maxLength);

        Assert.AreEqual(input, result);
    }

    [TestMethod]
    public void Ellipsize_ReturnsTruncatedStringWithEllipsis_WhenMaxLengthIsLessThanStringLength()
    {
        var input = "This is a long text.";
        var maxLength = 10;
        var expectedResult = "This is...";

        var result = input.Ellipsize(maxLength);

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public void Ellipsize_ReturnsEmptyString_WhenInputStringIsEmpty()
    {
        var input = string.Empty;
        var maxLength = 5;

        var result = input.Ellipsize(maxLength);

        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void Ellipsize_ReturnsEllipsis_WhenMaxLengthIsEqualToStringLength()
    {
        var input = "Sample Text";
        var maxLength = input.Length;
        var expectedResult = "Sample Text";

        var result = input.Ellipsize(maxLength);

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Ellipsize_Throws_WhenMaxLengthIsNegative()
    {
        var input = "Test";
        var maxLength = -5;

        input.Ellipsize(maxLength);
    }

    [TestMethod]
    public void Ellipsize_ReturnsEllipsis_WhenInputStringContainsOnlyWhitespace()
    {
        var input = "   ";
        var maxLength = 2;
        var expectedResult = "...";

        var result = input.Ellipsize(maxLength);

        Assert.AreEqual(expectedResult, result);
    }
}
