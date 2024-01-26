using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalBAND.Api.Exceptions
{
    public class ProblemException : Exception
    {
        public ProblemException(string msg) : base(msg)
        {

        }
    }
}
