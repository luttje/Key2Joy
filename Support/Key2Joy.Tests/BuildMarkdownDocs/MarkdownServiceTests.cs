using BuildMarkdownDocs;
using BuildMarkdownDocs.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;

namespace Key2Joy.Tests.BuildMarkdownDocs;

[TestClass]
public class MarkdownServiceTests
{
    private static readonly string SampleXmlFile = Path.Combine("BuildMarkdownDocs", "sample.xml");
    private Mock<IFileService> fileServiceMock;
    private MarkdownService markdownService;

    [TestInitialize]
    public void Setup()
    {
        this.fileServiceMock = new Mock<IFileService>();
        this.markdownService = new MarkdownService(this.fileServiceMock.Object);

        this.fileServiceMock
            .Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns<string>(File.ReadAllText);
    }

    [TestMethod]
    public void BuildDocumentation_SampleXmlFile_CallsWriteAtLeastOnce()
    {
        var args = new CommandLineArgs
        {
            XmlFilesDirectory = "some/directory",
            AssemblyDirectory = "", // Same as test assembly
            OutputDirectory = "output/directory",
            Filter = "markdown-doc"
        };

        this.fileServiceMock.Setup(fs => fs.GetXmlFiles(args.XmlFilesDirectory)).Returns(new List<string> { SampleXmlFile });
        this.fileServiceMock.Setup(fs => fs.WriteToFile(It.IsAny<string>(), It.IsAny<string>()));

        this.markdownService.BuildDocumentation(args);

        this.fileServiceMock.Verify(fs => fs.WriteToFile(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
    }
}
