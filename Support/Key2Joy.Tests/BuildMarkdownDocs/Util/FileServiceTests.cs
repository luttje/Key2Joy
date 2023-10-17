using System;
using System.IO;
using System.Linq;
using BuildMarkdownDocs.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.BuildMarkdownDocs.Util;

[TestClass]
public class FileServiceTests
{
    private const string TestFilePath = "test.txt";
    private const string TestDirectoryPath = "testDirectory";
    private static readonly string TestFileInDirectoryPath = Path.Combine(TestDirectoryPath, "test.txt");
    private static readonly string TestXmlFilePath = Path.Combine(TestDirectoryPath, "test.xml");

    [TestMethod]
    public void ReadFile_ShouldReadFileSuccessfully()
    {
        const string expectedContent = "Hello, world!";
        File.WriteAllText(TestFilePath, expectedContent);

        var fileService = new FileService();
        var result = fileService.ReadFile(TestFilePath);

        Assert.AreEqual(expectedContent, result);

        File.Delete(TestFilePath);
    }

    [TestMethod]
    public void WriteToFile_ShouldWriteSuccessfully()
    {
        const string content = "Hello, world!";

        var fileService = new FileService();
        fileService.WriteToFile(TestFilePath, content);
        var result = File.ReadAllText(TestFilePath);

        Assert.AreEqual(content, result);

        File.Delete(TestFilePath);
    }

    [TestMethod]
    public void WriteToFile_ShouldCreateDirectoryAndWriteSuccessfully()
    {
        const string content = "Hello, world!";

        var fileService = new FileService();
        fileService.WriteToFile(TestFileInDirectoryPath, content);
        var result = File.ReadAllText(TestFileInDirectoryPath);

        Assert.AreEqual(content, result);

        Directory.Delete(TestDirectoryPath, true);
    }

    [TestMethod]
    public void WriteToFile_ShouldNormalizeNewlinesToLF()
    {
        const string content = "Hello,\r\nworld!";
        const string expectedContent = "Hello,\nworld!";

        var fileService = new FileService();
        fileService.WriteToFile(TestFilePath, content);
        var result = File.ReadAllText(TestFilePath);

        Assert.AreEqual(expectedContent, result);

        File.Delete(TestFilePath);
    }

    [TestMethod]
    public void GetXmlFiles_ShouldReturnXmlFilesOnly()
    {
        Directory.CreateDirectory(TestDirectoryPath);
        File.WriteAllText(TestXmlFilePath, "<root></root>");
        File.WriteAllText(TestFileInDirectoryPath, "test");

        var fileService = new FileService();
        var xmlFiles = fileService.GetXmlFiles(TestDirectoryPath).ToList();

        Assert.AreEqual(1, xmlFiles.Count);
        Assert.IsTrue(xmlFiles.Contains(TestXmlFilePath));

        Directory.Delete(TestDirectoryPath, true);
    }

    [TestMethod]
    [ExpectedException(typeof(FileNotFoundException))]
    public void ReadFile_ShouldThrowExceptionForNonExistentFile()
        => new FileService().ReadFile("nonexistent.txt");
}
