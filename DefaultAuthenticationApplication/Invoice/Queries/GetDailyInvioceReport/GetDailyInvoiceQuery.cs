using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Invoice.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Queries.GetDailyInvioceReport
{
    public record GetDailyInvoiceQuery : IRequest<Result<List<DailyInvoiceDto>>>, ICacheableQuery
    {
        public string CacheKey=> "DailyInvoice";

        public TimeSpan? Expiration => TimeSpan.FromMinutes(15);

        public bool BypassCache => false;
    }
}
