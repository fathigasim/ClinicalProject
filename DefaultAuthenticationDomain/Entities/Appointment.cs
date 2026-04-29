using ClinicProjectDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public string AppointmentNumber { get; set; } = default!;
        public Guid PatiendId { get; set; }
        public Patient Patient { get; set; }
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public DateTime AppointmentDate { get; set; }
        public  string Notes  { get; set; }
        public AppointmentStatus status { get; set; } = AppointmentStatus.Scheduled;
        public MedicalRecords MedicalRecord { get; set; }
        public Invoices Invoices { get; set; }


        public void Schedule(DateTime scheduleTime)
        {
            if (scheduleTime.Date >= DateTime.Now.Date)
            {
                AppointmentDate = scheduleTime;
                status= AppointmentStatus.Scheduled;
            }
            else { 
                
                status = AppointmentStatus.Cancelled;
                AppointmentDate = scheduleTime;
            }
        }

        public void Cancel()
        {
            if (status == AppointmentStatus.Completed)
            {
                throw new InvalidOperationException("Cannot cancel completed appointment");
            }
            status= AppointmentStatus.Cancelled;
        }
    }
}
