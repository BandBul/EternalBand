using EternalBAND.Common;
using System;
namespace EternalBAND.Api.Helpers
{
    public static class SeoLinkHelper
    {
        public static string CreateSeo(string phrase)
        {
            var parsedStr = StrConvert.TRToEnDeleteAllSpacesAndToLower(phrase);
            return 
                parsedStr
                + new Random().Next(0, 9999999)
                + new Random().Next(0, 9999);
        }
    }
}
