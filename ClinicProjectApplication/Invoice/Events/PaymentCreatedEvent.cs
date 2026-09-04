using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Events
{
    public record PaymentCreatedEvent(Guid Id, string CustomerId);
   
}
