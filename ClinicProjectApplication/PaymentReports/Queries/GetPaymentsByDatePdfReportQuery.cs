using ClinicProjectApplication.Payment.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.PaymentReports.Queries
{
    public record GetPaymentsByDatePdfReportQuery(DateTime date):IRequest<List<PaymentDto>>;
   
}
