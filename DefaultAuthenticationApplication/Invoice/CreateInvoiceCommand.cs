using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;

using MediatR;


namespace ClinicProjectApplication.Invoice
{
    public record CreateInvoiceCommand(Guid AppointmentId, decimal TotalAmount) :IRequest<Result<string>>,ITransactionalRequest;
 
}
