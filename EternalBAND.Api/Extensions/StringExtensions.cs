using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalBAND.Api.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidForFilter(this string str) 
        {
            return !str.IsNullOrEmpty() && str != "0";
        }
    }
}
