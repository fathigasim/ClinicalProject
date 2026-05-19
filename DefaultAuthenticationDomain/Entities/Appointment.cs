using ClinicProjectDomain.Enums;
using ClinicProjectDomain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class Appointment :BaseEntity,IAuditableEntity
    {
   
        public string AppointmentNumber { get; set; } = default!;
        public int DurationMinutes { get; set; } = 30;
        public TimeOnly StartTime { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public DayOfWeek DayOfWeek { get; set; } 
        public  string Notes  { get; set; }
        public DateTime CreatedAt { get; set; }=DateTime.Now;
        public AppointmentStatus status { get; set; } = AppointmentStatus.Scheduled;
        public MedicalRecords MedicalRecord { get; set; }
        public Invoices Invoices { get; set; }
       

        public void Cancel()
        {
                 if( status != AppointmentStatus.Completed)
            {
                  status = AppointmentStatus.Cancelled;
            }
        }


    }
}
