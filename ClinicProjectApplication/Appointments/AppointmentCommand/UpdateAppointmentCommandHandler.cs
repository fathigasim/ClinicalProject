using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Enums;
using ClinicProjectDomain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Appointments.AppointmentCommand
{
    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand,Result<string>>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentsRepository;
        public UpdateAppointmentCommandHandler(IDoctorRepository doctorRepository, IAppointmentRepository appointmentsRepository)
        {
           _doctorRepository = doctorRepository;   
            _appointmentsRepository = appointmentsRepository;
        }
        public async Task<Result<string>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentsRepository.GetByAppointmentNumberAsync(request.AppointmentNo,cancellationToken);
            if (appointment == null) {
                return Result<string>.Failure("No such appointment number");
            }
            var schedule = await _doctorRepository
                 .DoctorSchedule(appointment.DoctorId, request.AppointmentDate, cancellationToken);
            if (schedule == null) {
                return Result<string>.Failure("Doctor is not available on this day");
            }

            // Validate time is within working hours
            if (!schedule.IsTimeSlotValid(request.StartTime))
                return Result<string>.Failure(
                    $"Time {request.StartTime} is outside working hours " +
                    $"{schedule.StartTime}–{schedule.EndTime}.");

            //  Validate slot is still available
            var existingAppointments = await _appointmentsRepository
                .GetAppointmentsByDoctorIdAsync(appointment.DoctorId, request.AppointmentDate, cancellationToken);

            var isSlotTaken = existingAppointments
            .Where(a => a.Status != AppointmentStatus.Cancelled)
            .Any(a => schedule.IsOverlapping(
                request.StartTime,
                request.StartTime.AddMinutes(30),
                a.StartTime,
                a.StartTime.AddMinutes(a.DurationMinutes)));

            if (isSlotTaken)
                return Result<string>.Failure("This slot is already booked.");
            //var patientAppointment = await _appointmentsRepository
            //    .GetByAppointmentNumberAsync(appointment.DoctorId, appointment.PatientId,request.DayOfWeek,cancellationToken);

           
            //    appointment.StartTime = request.StartTime;
                _appointmentsRepository.Update(appointment);
            

            return Result<string>.Success("Appointment updated successfully");
        }
    }
}
