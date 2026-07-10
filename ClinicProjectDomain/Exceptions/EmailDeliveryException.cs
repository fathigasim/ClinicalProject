using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Exceptions
{
    public class EmailDeliveryException : Exception
    {
        public EmailDeliveryException(string message, Exception? inner = null)
            : base(message, inner) { }
    }
}
