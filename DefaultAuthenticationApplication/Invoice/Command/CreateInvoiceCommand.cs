using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using MediatR;


namespace ClinicProjectApplication.Invoice.Command
{
    public record CreateInvoiceCommand(string AppointmentNo, decimal TotalAmount) :IRequest<Result<string>>,ITransactionalRequest;
 
}
