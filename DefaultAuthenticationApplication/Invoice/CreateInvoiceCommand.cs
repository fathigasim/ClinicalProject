using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;

using MediatR;


namespace ClinicProjectApplication.Invoice
{
    public record CreateInvoiceCommand:IRequest<Result<Unit>>,ITransactionalRequest;
   
}
