using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Exceptions
{
    public class CashLimitExceededException : Exception
    {
        public CashLimitExceededException()
       : base("Cash limit exceeded for this invoice.") { }
    }
}
