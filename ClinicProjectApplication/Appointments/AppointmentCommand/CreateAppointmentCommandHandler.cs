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
                  .DoctorSchedule(request.DoctorId,request.AppointmentDate ,cancellationToken);
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
                .GetAppointmentsByDoctorIdAsync(request.DoctorId, request.AppointmentDate, cancellationToken);

            var isSlotTaken = existingAppointments
            .Where(a => a.Status != AppointmentStatus.Cancelled)
            .Any(a => schedule.IsOverlapping(
                request.StartTime,
                request.StartTime.AddMinutes(30),
                a.StartTime,
                a.StartTime.AddMinutes(a.DurationMinutes)));

            if (isSlotTaken)
                return Result<string>.Failure("This slot is already booked.");
            var sequence =await _sequenceSerivce.GenerateOrderNumberAsync();


            //var appointment = new Appointment
            //{
            //    AppointmentNumber = sequence,
            //    PatientId = request.PatientId,
            //    DoctorId = request.DoctorId,
            //    AppointmentDate = request.AppointmentDate,
            //    StartTime = request.StartTime,
            //    Notes = request.Notes,
            //};
            var appointment =  Appointment.CreateAppointment(sequence, request.PatientId, request.DoctorId,
                request.AppointmentDate, request.StartTime, request.Notes);

            await _appointmentRepository.AddAsync(appointment);
            return Result<string>.Success($"Appointment Confirmed for "+$"{appointment.AppointmentNumber}");
        }
    }
}
