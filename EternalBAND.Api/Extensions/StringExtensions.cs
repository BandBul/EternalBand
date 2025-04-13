using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace EternalBAND.Api.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidForFilter(this string str) 
        {
            return !string.IsNullOrEmpty(str) && str != "0";
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? collection)
        {
            return collection == null || !collection.Any();
        }

        public static string ToJsSuitablePath(this string str)
        {
            return str.Replace("\\", "/");
        }
    }
}
