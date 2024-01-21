using System.Globalization;
using System.Text;

namespace EternalBAND.Business;

public class StrConvert
{
    public string TRToEnDeleteAllSpacesAndToLower(string TempStr)
    {
        TempStr = String.Join("", TempStr.Normalize(NormalizationForm.FormD)
            .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)).Replace(" ", "-").ToLower();
        

        string source = "ığüşöçĞÜŞİÖÇ";
        string destination = "igusocGUSIOC";
        string result = TempStr;
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
}