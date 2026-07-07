using ClinicProjectApplication.Common;
using ClinicProjectApplication.Invoice.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Queries.GetInvoiceById
{
    public record GetInvoiceByInvoiceNumberQuery(string invoiceNo) :IRequest<Result<InvoicesDto>>;
   
}
