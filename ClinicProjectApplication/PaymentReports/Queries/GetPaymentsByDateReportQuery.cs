using ClinicProjectApplication.Common;
using ClinicProjectApplication.PaymentReports.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.PaymentReports.Queries
{
    public record GetPaymentsByDateReportQuery(DateTime date) :IRequest<Result<List<PaymentReportDto>>>;
   
}
