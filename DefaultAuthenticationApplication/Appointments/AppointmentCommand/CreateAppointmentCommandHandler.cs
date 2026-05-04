using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Enums;
using ClinicProjectDomain.Interfaces;
using MediatR;


namespace ClinicProjectApplication.Appointments.AppointmentCommand
{
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Result<string>>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ISequenceService _sequenceSerivce;
        public CreateAppointmentCommandHandler(IAppointmentRepository appointmentRepository, IDoctorRepository doctorRepository, ISequenceService sequenceSerivce)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _sequenceSerivce = sequenceSerivce;
        }
        public async Task<Result<string>> Handle (CreateAppointmentCommand request, CancellationToken cancellationToken)
        {

            var schedule = await _doctorRepository
                  .DoctorWeeklySchedule(request.DoctorId,request.DayOfWeek ,cancellationToken);
                 // .FirstOrDefaultAsync(s => s.DayOfWeek == request.DayOfWeek );

            if (schedule == null)
                return Result<string>.Failure("No schedule found for this day.");

            // Validate time is within working hours
            if (!schedule.IsTimeSlotValid(request.StartTime))
                return Result<string>.Failure(
                    $"Time {request.StartTime} is outside working hours " +
                    $"{schedule.StartTime}–{schedule.EndTime}.");

            //  Validate slot is still available
            var existingAppointments = await _appointmentRepository
                .GetAppointmentsByDoctorIdAsync(request.DoctorId, request.DayOfWeek, cancellationToken);

            var isSlotTaken = existingAppointments
            .Where(a => a.status != AppointmentStatus.Cancelled)
            .Any(a => schedule.IsOverlapping(
                request.StartTime,
                request.StartTime.AddMinutes(30),
                a.StartTime,
                a.StartTime.AddMinutes(a.DurationMinutes)));

            if (isSlotTaken)
                return Result<string>.Failure("This slot is already booked.");
            var sequence =await _sequenceSerivce.GenerateOrderNumberAsync();

            var appointment = new Appointment
            {
                AppointmentNumber = sequence,
                PatientId = request.PatiendId,
                DoctorId = request.DoctorId,
                DayOfWeek = request.DayOfWeek,
                StartTime = request.StartTime,
                Notes = request.Notes,
            };

            //if (!appointment.IsValidAppointment())
            //{
            //    return Result<string>.Failure("Invalid appointment date.");
            //}

            //appointment.Schedule(request.AppointmentDate);
            //await  _appointmentRepository.AddAsync(appointment);
            return Result<string>.Success($"Appointment Confirmed for "+$"{appointment.AppointmentNumber}");
        }
    }
}
