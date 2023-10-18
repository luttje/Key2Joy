using BuildMarkdownDocs.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Key2Joy.Tests.BuildMarkdownDocs.Util;

[TestClass]
public class CommandLineArgsParserTests
{
    [TestMethod]
    public void Parse_WithTooFewArgs_ReturnsNull()
    {
        var args = new string[] { "assemblyDir" };

        var result = CommandLineArgsParser.Parse(args);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void Parse_WithMinimumRequiredArgs_ShouldParseCorrectly()
    {
        var args = new string[] { "assemblyDir", "xmlFilesDir", "outputDir" };

        var result = CommandLineArgsParser.Parse(args);

        Assert.AreEqual("assemblyDir", result.AssemblyDirectory);
        Assert.AreEqual("xmlFilesDir", result.XmlFilesDirectory);
        Assert.AreEqual("outputDir", result.OutputDirectory);
    }

    [TestMethod]
    public void Parse_WithTemplate_ShouldParseTemplateCorrectly()
    {
        var args = new string[] { "assemblyDir", "xmlFilesDir", "outputDir", "--template", "templateValue" };

        var result = CommandLineArgsParser.Parse(args);

        Assert.AreEqual("templateValue", result.Template);
    }

    [TestMethod]
    public void Parse_WithFilter_ShouldParseFilterCorrectly()
    {
        var args = new string[] { "assemblyDir", "xmlFilesDir", "outputDir", "--filter", "filterValue" };

        var result = CommandLineArgsParser.Parse(args);

        Assert.AreEqual("filterValue", result.Filter);
    }

    [TestMethod]
    public void Parse_WithBothTemplateAndFilter_ShouldParseBothCorrectly()
    {
        var args = new string[] { "assemblyDir", "xmlFilesDir", "outputDir", "--template", "templateValue", "--filter", "filterValue" };

        var result = CommandLineArgsParser.Parse(args);

        Assert.AreEqual("templateValue", result.Template);
        Assert.AreEqual("filterValue", result.Filter);
    }

    [TestMethod]
    [ExpectedException(typeof(IndexOutOfRangeException))]
    public void Parse_WithTemplateFlagButNoValue_ShouldThrowException()
    {
        var args = new string[] { "assemblyDir", "xmlFilesDir", "outputDir", "--template" };

        var result = CommandLineArgsParser.Parse(args);
    }

    [TestMethod]
    [ExpectedException(typeof(IndexOutOfRangeException))]
    public void Parse_WithFilterFlagButNoValue_ShouldThrowException()
    {
        var args = new string[] { "assemblyDir", "xmlFilesDir", "outputDir", "--filter" };

        var result = CommandLineArgsParser.Parse(args);
    }

    [TestMethod]
    public void Parse_WithUnrecognizedFlag_ShouldIgnoreIt()
    {
        var args = new string[] { "assemblyDir", "xmlFilesDir", "outputDir", "--unknownFlag", "value" };

        var result = CommandLineArgsParser.Parse(args);

        Assert.IsNull(result.Template);
        Assert.IsNull(result.Filter);
    }

    [TestMethod]
    public void Parse_WithMixedOrderFlags_ShouldParseCorrectly()
    {
        var args = new string[] { "assemblyDir", "xmlFilesDir", "outputDir", "--filter", "filterValue", "--template", "templateValue" };

        var result = CommandLineArgsParser.Parse(args);

        Assert.AreEqual("templateValue", result.Template);
        Assert.AreEqual("filterValue", result.Filter);
    }

    [TestMethod]
    public void Parse_WithDuplicatedFlags_ShouldUseLastValue()
    {
        var args = new string[] { "assemblyDir", "xmlFilesDir", "outputDir", "--template", "template1", "--template", "template2" };

        var result = CommandLineArgsParser.Parse(args);

        Assert.AreEqual("template2", result.Template);
    }
}
