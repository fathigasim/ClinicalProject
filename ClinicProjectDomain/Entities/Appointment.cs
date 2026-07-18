using ClinicProjectDomain.Enums;
using ClinicProjectDomain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class Appointment :BaseEntity,IAuditableEntity
    {
        private Appointment(string appointmentNumber, Guid patientId, Guid doctorId,
           DateOnly appointmentDate, TimeOnly startTime, string notes)
        {
            AppointmentNumber = appointmentNumber;
            PatientId = patientId;
            DoctorId = doctorId;
            AppointmentDate = appointmentDate;
            StartTime = startTime;
            Notes = notes;
        }

        private Patient _Patient;
        public Patient Patient => _Patient;

        private Invoices _Invoice;
        public Invoices Invoice => _Invoice;

        private Doctor _Doctor;
        public Doctor Doctor => _Doctor;
        public string AppointmentNumber { get;private set; } = default!;
        public int DurationMinutes { get;private set; } = 30;
        public DateOnly AppointmentDate { get;private set; }
        public TimeOnly StartTime { get;private set; }
        public Guid PatientId { get;private set; }
     //   public Patient Patient { get; set; }
        public Guid DoctorId { get;private set; }
    //    public Doctor Doctor { get; set; }
        public DayOfWeek DayOfWeek { get;private set; } 
        public  string Notes  { get;private set; }
        public DateTime CreatedAt { get;private set; }=DateTime.Now;
        public AppointmentStatus status { get;private set; } = AppointmentStatus.Scheduled;
     //   public MedicalRecords MedicalRecord { get; set; }
     //   public Invoices Invoices { get; set; }

        //AppointmentNumber = sequence,
        //        PatientId = request.PatientId,
        //        DoctorId = request.DoctorId,
        //        AppointmentDate = request.AppointmentDate,
        //        StartTime = request.StartTime,
        //        Notes = request.Notes,
        public static Appointment CreateAppointment(string AppointmentNumber,Guid PatientId,Guid DoctorId,
           DateOnly AppointmentDate,TimeOnly StartTime,string Notes )
        {
        //    UpdateStatus(status);
           return  new Appointment( AppointmentNumber,  PatientId,  DoctorId,
            AppointmentDate,  StartTime,  Notes);
        }
        public static void UpdateStatus(AppointmentStatus status)
        {
            status = AppointmentStatus.Completed;
        }
        public static bool beAppointmentValidDate(DateOnly appointmentDate)
        {
            if(appointmentDate < DateOnly.FromDateTime( DateTime.UtcNow))
            {
                return false;
            }
            return true;
        }
        public void Cancel()
        {
                 if( status != AppointmentStatus.Completed)
            {
                  status = AppointmentStatus.Cancelled;
            }
        }


    }
}
