using ClinicProjectDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class Doctor :BaseEntity, IAuditableEntity
    {
     
        public string FirstName { get; set; }=default!;
        public string LastName { get; set; }=default!;
        public string Specialization { get; set; }=default!;
        public string Phone { get; set; }=default!;
        public string Email { get; set; }=default!;
       public DateTime CreatedAt { get; set; }
        public ICollection<Appointment?> Appointments { get; set; }
        public ICollection<WeeklySchedule> WeeklySchedules { get; set; }

    }
}
