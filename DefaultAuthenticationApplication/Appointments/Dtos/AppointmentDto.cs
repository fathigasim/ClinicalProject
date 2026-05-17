using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Appointments.Dtos
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public string AppointmentNumber { get; set; } = default!;
        public int DurationMinutes { get; set; } 
        public TimeOnly StartTime { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public string Notes { get; set; }
        public AppointmentStatus status { get; set; } = AppointmentStatus.Scheduled;
    }
}
