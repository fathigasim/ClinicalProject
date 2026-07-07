using ClinicProjectApplication.Doctors.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Doctors.Queries
{
    public class GetDoctorsAvailableSlotsQuery :IRequest<List<DoctorsAvailableSlotDto>>
    {
       
        public Guid DoctorId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}
