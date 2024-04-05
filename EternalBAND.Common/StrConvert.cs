using System.Globalization;
using System.Text;

namespace EternalBAND.Common;

public static class StrConvert
{
    public static string TRToEnDeleteAllSpacesAndToLower(string seoLink)
    {
        seoLink = String.Join("", seoLink.Normalize(NormalizationForm.FormD)
            .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)).Replace(" ", "-").ToLower();

        string source = "ığüşöçĞÜŞİÖÇ";
        string destination = "igusocGUSIOC";
        string result = seoLink;
        for (int i = 0; i < source.Length; i++)
        {
            result = result.Replace(source[i], destination[i]);
        }
        string temp = "-";
        for (int i = 0; i < 10; i++)
        {
          result=  result.Replace(temp, "-");
            temp = temp + "-";
        }
        return result.Replace(".","-").ToLower();
    }

    public static bool IsInjectionString(string seoLink)
    {
        return Uri.IsWellFormedUriString(seoLink, UriKind.Absolute);
    }
}