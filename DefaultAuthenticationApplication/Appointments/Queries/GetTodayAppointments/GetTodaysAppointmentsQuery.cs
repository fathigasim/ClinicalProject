using ClinicProjectApplication.Appointments.Dtos;
using ClinicProjectApplication.Common;
using ClinicProjectDomain.Common.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Appointments.Queries.GetTodayAppointments
{
    public record GetTodaysAppointmentsQuery(int page,int pageSize):IRequest<Result<PagedResult<AppointmentDto>>>
    {
    }
}
