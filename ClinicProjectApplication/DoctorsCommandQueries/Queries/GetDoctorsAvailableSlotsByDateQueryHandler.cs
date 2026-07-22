using ClinicProjectApplication.Doctors.Dto;
using ClinicProjectApplication.DoctorsCommandQueries.Dto;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using ClinicProjectDomain.Services;
using MediatR;
using MediatR.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.DoctorsCommandQueries.Queries
{
    public class GetDoctorsAvailableSlotsByDateQueryHandler(
        IDoctorRepository doctorRepository, IAppointmentRepository appointmentRepository,
         ScheduleService scheduleService
        ) : IRequestHandler<GetDoctorsAvailableSlotsByDateQuery, List<DoctorsAvailableSlotDto>>
    {

        public async Task<List<DoctorsAvailableSlotDto>> Handle(GetDoctorsAvailableSlotsByDateQuery request, CancellationToken cancellationToken)
        {
            var doctor = await doctorRepository.GetByIdAsync(request.DoctorId);
            if (doctor == null)
            {
                throw new KeyNotFoundException(nameof(doctor));
            }
            var doctorSchedule = await doctorRepository
    .DoctorScheduleDate(request.DoctorId, request.date, cancellationToken);

            if (doctorSchedule == null)
            {
                // Either doctor doesn't exist OR has no schedule for this date
                return new List<DoctorsAvailableSlotDto>();
            }

            var doctorAppointments = await appointmentRepository
              .GetDatedAppointmentsByDoctorIdAsync(request.DoctorId, request.date, cancellationToken);

            var availableSlots = scheduleService.GetAvailableSlots(
                doctorSchedule, doctorAppointments);

            return availableSlots
                .Select(slot => new DoctorsAvailableSlotDto { AvailableSlot = slot })
                .ToList();
        }


    }
}


