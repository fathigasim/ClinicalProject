using ClinicProjectDomain.Enums;
using ClinicProjectDomain.Exceptions;
using ClinicProjectDomain.Interfaces;
using System;

namespace ClinicProjectDomain.Entities
{
    public class Appointment : BaseEntity, IAuditableEntity
    {
        private Appointment(
            string appointmentNumber,
            Guid patientId,
            Guid doctorId,
            DateOnly appointmentDate,
            TimeOnly startTime,
            string notes,
            int durationMinutes)
        {
            AppointmentNumber = appointmentNumber;
            PatientId = patientId;
            DoctorId = doctorId;
            AppointmentDate = appointmentDate;
            DayOfWeek = appointmentDate.DayOfWeek; // Automatically set
            StartTime = startTime;
            Notes = notes;
            DurationMinutes = durationMinutes > 0 ? durationMinutes : 30;
        }

        // Backing fields for EF Core DDD navigation mapping
        private Patient _Patient = default!;
        public Patient Patient => _Patient;

        private Invoices _Invoice = default!;
        public Invoices Invoice => _Invoice;

        private Doctor _Doctor = default!;
        public Doctor Doctor => _Doctor;

        public string AppointmentNumber { get; private set; } = default!;
        public int DurationMinutes { get; private set; } = 30;
        public DateOnly AppointmentDate { get; private set; }
        public TimeOnly StartTime { get; private set; }
        public Guid PatientId { get; private set; }
        public Guid DoctorId { get; private set; }

        public DayOfWeek DayOfWeek { get; private set; }
        public string Notes { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public AppointmentStatus Status { get; private set; } = AppointmentStatus.Scheduled;
        public bool IsBooked { get; private set; }

        // Factory Method
        public static Appointment CreateAppointment(
            string appointmentNumber,
            Guid patientId,
            Guid doctorId,
            DateOnly appointmentDate,
            TimeOnly startTime,
            string notes,
            int durationMinutes = 30)
        {
            if (!BeAppointmentValidDate(appointmentDate))
            {
                throw new DomainException("Cannot book an appointment in the past.");
            }

            return new Appointment(
                appointmentNumber,
                patientId,
                doctorId,
                appointmentDate,
                startTime,
                notes,
                durationMinutes);
        }
        public void update(Appointment appointment)
        {
            if (Status == AppointmentStatus.Cancelled)
            {
                throw new DomainException("Cannot update canceled appointment");
            }
             PatientId= appointment.PatientId;
            DoctorId= appointment.DoctorId;
            AppointmentDate = appointment.AppointmentDate;
            StartTime = appointment.StartTime;
        }
        public void ConfirmBooking()
        {
            if (IsBooked)
            {
                throw new DomainException("Already booked appointment.");
            }
            IsBooked = true;
        }

        public static bool BeAppointmentValidDate(DateOnly appointmentDate)
        {
            return appointmentDate >= DateOnly.FromDateTime(DateTime.UtcNow);
        }

        public void Cancel()
        {
            if (Status == AppointmentStatus.Cancelled)
            {
                throw new DomainException("Cannot cancel an already canceled appointment.");
            }
            if (Status == AppointmentStatus.Completed)
            {
                throw new DomainException("Cannot cancel an already completed appointment.");
            }

            Status = AppointmentStatus.Cancelled;
        }

        public void Complete()
        {
            if (Status == AppointmentStatus.Cancelled)
            {
                throw new DomainException("Cannot complete a cancelled appointment.");
            }

            Status = AppointmentStatus.Completed;
        }
    }
}