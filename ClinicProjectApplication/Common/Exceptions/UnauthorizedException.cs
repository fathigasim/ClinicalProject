using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Common.Exceptions
{
    // Application/Common/Exceptions/UnauthorizedException.cs
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "Unauthorized.")
            : base(message) { }
    }

   
}
