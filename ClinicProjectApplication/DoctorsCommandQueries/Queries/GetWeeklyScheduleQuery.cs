using ClinicProjectApplication.Doctors.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.DoctorsCommandQueries.Queries
{
    public record GetWeeklyScheduleQuery :IRequest <DoctorsScheduleDto>
    {

    }
}
