using System.Reflection;
using System;

namespace EternalBAND.Helpers;

public static class ImageHelper
{
    private static string ImagePath = "images/ilan/";
    private static string BlogPath = "images/blog/";

    public static string GetGeneratedAbsolutePostImagePath(int postId, string fileName)
    {
        return $"{ImagePath}{postId.ToString()}/{GeneratePhotoFileName(fileName)}";
    }

    public static string GetGeneratedAbsoluteBlogImagePath(int postId, string fileName)
    {
        return $"{BlogPath}{postId.ToString()}/{GeneratePhotoFileName(fileName)}";
    }

    public static bool DeletePhotos(string rootpath, List<string>? filesToDelete)
    {
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
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
    private static string GeneratePhotoFileName(string fileName)
    {
        return $"{Guid.NewGuid().ToString()}-{new Random().Next(0, 10000)}{Path.GetExtension(fileName)}";
    }
}