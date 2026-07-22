using ClinicProjectDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.PatientsCommandQueries.Dto
{
    public  record PatientDto
    {
  
        public Guid PatientId { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateTime DOB { get; set; }
        public string Phone { get; set; } = default!;
        public string Gender { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        
    }
}
