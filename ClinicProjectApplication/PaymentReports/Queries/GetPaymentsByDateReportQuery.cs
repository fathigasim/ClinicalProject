using ClinicProjectApplication.Common;
using ClinicProjectApplication.PaymentReports.Dto;
using ClinicProjectDomain.Common.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.PaymentReports.Queries
{
    public record GetPaymentsByDateReportQuery(DateTime date,int page,int pageSize) :IRequest<Result<PagedResult<PaymentReportDto>>>;
   
}
