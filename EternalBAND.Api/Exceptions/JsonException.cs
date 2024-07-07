using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalBAND.Api.Exceptions
{
    public class JsonException : Exception
    {
        public JsonException(string msg) : base(msg)
        {

        }
    }
}
