using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using Key2Joy.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Core.Util;

[TestClass]
public class FileSystemTests
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

    public static IEnumerable<object[]> ImageFormatsTestData
        => new List<object[]>
        {
            new object[]{ ".jpg", ImageFormat.Jpeg },
            new object[]{ ".png", ImageFormat.Png },
            new object[]{ ".bmp", ImageFormat.Bmp },
            new object[]{ ".gif", ImageFormat.Gif },
            new object[]{ ".ico", ImageFormat.Icon },
            new object[]{ ".emf", ImageFormat.Emf },
            new object[]{ ".exif", ImageFormat.Exif },
            new object[]{ ".tiff", ImageFormat.Tiff },
            new object[]{ ".wmf", ImageFormat.Wmf },
        };

    [DataTestMethod]
    [DynamicData(nameof(ImageFormatsTestData))]
    public void GetImageFormat_WhenExtensionIsValid_ReturnsImageFormat(string extension, ImageFormat expected)
    {
        var result = FileSystem.GetImageFormatFromExtension(extension);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetImageFormat_WhenExtensionIsInvalid_ThrowsArgumentException()
    {
        FileSystem.GetImageFormatFromExtension(".invalid");

        Assert.Fail(); // Should not reach this line
    }
}
