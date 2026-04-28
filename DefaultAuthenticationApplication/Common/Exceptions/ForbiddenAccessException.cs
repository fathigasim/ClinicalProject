using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Common.Exceptions
{
    // Application/Common/Exceptions/ForbiddenAccessException.cs
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message = "Forbidden.")
            : base(message) { }
    }
}
