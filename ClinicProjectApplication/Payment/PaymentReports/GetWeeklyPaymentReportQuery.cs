using ClinicProjectApplication.Payment.Dtos;
using ClinicProjectDomain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.PaymentReports
{
    public record GetWeeklyPaymentReportQuery:IRequest<List<WeeklyPaymentSummaryDto>>;
  
}
