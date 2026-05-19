using ClinicProjectDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class Patient :BaseEntity, IAuditableEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }=default!;
        public string LastName { get; set; }=default!;  
        public DateTime DOB { get; set; }
        public string Phone { get; set; }=default!;
        public string Gender { get; set; }=default!;
        public DateTime CreatedAt { get; set; }
        public ICollection<Appointment?> Appointments { get; set; }
      

    }
}
