using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Invoice.Dtos;
using MediatR;


namespace ClinicProjectApplication.Invoice.Command
{
    public record CreateInvoiceCommand(string AppointmentNo, decimal TotalAmount) :IRequest<Result<InvoicesDto>>,ITransactionalRequest;
 
}
