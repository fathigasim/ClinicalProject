using ClinicProjectApplication.Doctors.Dto;
using ClinicProjectDomain.Interfaces;
using ClinicProjectDomain.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Doctors.Queries
{
    public class GetDoctorsAvailableSlotsQueryHandler : IRequestHandler<GetDoctorsAvailableSlotsQuery, List<DoctorsAvailableSlotDto>>
    {
       private readonly ScheduleService _scheduleService;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        public GetDoctorsAvailableSlotsQueryHandler(ScheduleService scheduleService, IAppointmentRepository appointmentRepository, IDoctorRepository doctorRepository)
        {
            _scheduleService = scheduleService;
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<List<DoctorsAvailableSlotDto>> Handle(
     GetDoctorsAvailableSlotsQuery request,
     CancellationToken cancellationToken)
        {
            var doctorWeeklySchedule = await _doctorRepository
                .DoctorWeeklySchedule(request.DoctorId, request.DayOfWeek, cancellationToken);
                

            if (doctorWeeklySchedule == null || !doctorWeeklySchedule.IsActive)
                return new List<DoctorsAvailableSlotDto>();

            // ✅ Await if this is actually async
            var doctorAppointments = await _appointmentRepository
                .GetAppointmentsByDoctorIdAsync(request.DoctorId, request.DayOfWeek,cancellationToken);

            var availableSlots = _scheduleService.GetAvailableSlots(
                doctorWeeklySchedule, doctorAppointments);

            return availableSlots
                .Select(slot => new DoctorsAvailableSlotDto { AvailableSlot = slot })
                .ToList();
        }


    }
}
