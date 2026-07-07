using ClinicProjectApplication.Common;
using ClinicProjectApplication.Doctors.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Doctors.Queries
{
    public class GetTodaysDoctorShiftQuery :IRequest<Result<List<DoctorDto>>>
    {
    }
}
