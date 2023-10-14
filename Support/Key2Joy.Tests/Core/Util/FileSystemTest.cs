using System.IO;
using Key2Joy.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Core.Util;

[TestClass]
public class FileSystemTest
{
    private const string TestDirectory = "TestDirectory";

    [TestInitialize]
    public void TestInitialize()
    {
        if (!Directory.Exists(TestDirectory))
        {
            Directory.CreateDirectory(TestDirectory);
        }
    }

    [TestCleanup]
    public void TestCleanup()
    {
        if (Directory.Exists(TestDirectory))
        {
            Directory.Delete(TestDirectory, true);
        }
    }

    [TestMethod]
    public void FindNonExistingFile_WhenPathDoesNotExist_ReturnsOriginalPath()
    {
        var pathFormat = $"{TestDirectory}\\File_%VERSION%.txt";

        var result = FileSystem.FindNonExistingFile(pathFormat);

        Assert.AreEqual($"{TestDirectory}\\File_1.txt", result);
    }

    [TestMethod]
    public void FindNonExistingFile_WhenPathExistsOnce_ReturnsIncrementedPath()
    {
        var pathFormat = $"{TestDirectory}\\File_%VERSION%.txt";
        var existingFilePath = $"{TestDirectory}\\File_1.txt";
        File.Create(existingFilePath).Close();

        var result = FileSystem.FindNonExistingFile(pathFormat);

        Assert.AreEqual($"{TestDirectory}\\File_2.txt", result);

        File.Delete(existingFilePath);
    }

    [TestMethod]
    public void FindNonExistingFile_WithCustomStartVersion_ReturnsPathWithCustomStartVersion()
    {
        var pathFormat = $"{TestDirectory}\\File_%VERSION%.txt";

        var result = FileSystem.FindNonExistingFile(pathFormat, 5);

        Assert.AreEqual($"{TestDirectory}\\File_5.txt", result);
    }

    [TestMethod]
    public void FindNonExistingFile_WhenPathWithCustomStartVersionExists_ReturnsIncrementedPath()
    {
        var pathFormat = $"{TestDirectory}\\File_%VERSION%.txt";
        var existingFilePath = $"{TestDirectory}\\File_5.txt";
        File.Create(existingFilePath).Close();

        var result = FileSystem.FindNonExistingFile(pathFormat, 5);

        Assert.AreEqual($"{TestDirectory}\\File_6.txt", result);

        File.Delete(existingFilePath);
    }
}
