using System.Reflection;
using System;
using System.Security.Cryptography.X509Certificates;

namespace EternalBAND.Helpers;

public static class ImageHelper
{
    private static string PostImagePath = @"images\ilan\";
    private static string BlogImagePath = @"images\blog\";

    public static string GetGeneratedAbsolutePostImagePath(string? seoLink, string fileName)
    {
        return @$"{PostImagePath}{seoLink}\{GeneratePhotoFileName(fileName)}";
    }

    public static string GetGeneratedAbsoluteBlogImagePath(string folderName, string fileName)
    {
        return @$"{BlogImagePath}{folderName}\{GeneratePhotoFileName(fileName)}";
    }
    // TODO delete folder if all images deleted
    public static int DeletePhotos(string rootpath, List<string>? filesToDelete, bool deleteFolderFlag = false)
    {
        int deletedImageCount = 0;
        try
        {
            if (filesToDelete != null && filesToDelete.Count > 0)
            {
                foreach (var fileName in filesToDelete)
                {
                    var filePath = Path.Combine(rootpath, fileName);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        deletedImageCount++;
                    }
                }
                var fullPath = Path.Combine(rootpath, filesToDelete.First());
                if (deleteFolderFlag && IsDirectoryEmpty(Path.GetDirectoryName(fullPath)))
                {
                    Directory.Delete(fullPath);
                }

            }
            return deletedImageCount;
        }
        catch (Exception)
        {
            return deletedImageCount;
        }
    }

    public static void CleanUpBlogDirectory(string rootpath,string relativePath )
    {
        var fullPath = Path.Combine(rootpath, BlogImagePath, relativePath);
        Directory.Delete(fullPath,true);
    }

    public static void CleanUpPostDirectory(string rootpath, string relativePath)
    {
        var fullPath = Path.Combine(rootpath, PostImagePath, relativePath);
        Directory.Delete(fullPath,true);
    }

    private static bool IsDirectoryEmpty(string path)
    {
        return Directory.GetFiles(path).Length == 0 && Directory.GetDirectories(path).Length == 0;
    }

    //TODO add check for "is filename exist in folder"
    private static string GeneratePhotoFileName(string fileName)
    {
        return $"{Guid.NewGuid().ToString()}-{new Random().Next(0, 10000)}{Path.GetExtension(fileName)}";
    }
}