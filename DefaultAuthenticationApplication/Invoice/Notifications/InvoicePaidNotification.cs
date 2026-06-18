using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Notifications
{
    public record InvoicePaidNotification(Guid InvoiceId,string? Email=null):INotification,ITransactionalRequest
    {
    }
    
}
