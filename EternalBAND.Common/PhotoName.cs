using System.Globalization;
using System.Text;

namespace EternalBAND.Helpers;

public class PhotoName
{
    public string GeneratePhotoName(string tempStr)
    {
        return String.Join("", tempStr.Normalize(NormalizationForm.FormD)
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)).Replace(" ", "-")
            .ToLower();
    }
}