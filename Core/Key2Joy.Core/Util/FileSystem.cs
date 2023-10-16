using System;
using System.Drawing.Imaging;
using System.IO;

namespace Key2Joy.Util;

public static class FileSystem
{
    public const int MAX_PATH = 260;

    /// <summary>
    /// Tests the given pathFormat with a number to see if it exists, if it does it tries again while increasing the number until an available filename is found.
    /// </summary>
    /// <param name="pathFormat">A string containing %VERSION% that will become the filePath</param>
    /// <param name="startVersion">The number to place in %VERSION% on first attempt. Increments if not available.</param>
    /// <returns>The available file path</returns>
    public static string FindNonExistingFile(string pathFormat, int startVersion = 1)
    {
        string filePath;

        do
        {
            filePath = pathFormat.Replace("%VERSION%", startVersion.ToString());
            startVersion++;
        } while (File.Exists(filePath));

        return filePath;
    }

    /// <summary>
    /// Returns the image format for the given extension.
    /// </summary>
    /// <param name="extension"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static ImageFormat GetImageFormatFromExtension(string extension)
        => extension switch
        {
            ".jpg" => ImageFormat.Jpeg,
            ".png" => ImageFormat.Png,
            ".bmp" => ImageFormat.Bmp,
            ".gif" => ImageFormat.Gif,
            ".ico" => ImageFormat.Icon,
            ".emf" => ImageFormat.Emf,
            ".exif" => ImageFormat.Exif,
            ".tiff" => ImageFormat.Tiff,
            ".wmf" => ImageFormat.Wmf,
            _ => throw new ArgumentException($"Unknown extension, cannot deduce image format")
        };
}
