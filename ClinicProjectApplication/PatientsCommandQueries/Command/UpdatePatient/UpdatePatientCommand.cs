
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.PatientsCommandQueries.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.PatientsCommandQueries.Command.UpdatePatient
{
    public record UpdatePatientCommand : IRequest<string>,ITransactionalRequest
    {
       public Guid Id { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public DateTime DOB { get;  set; }
        public string Phone { get;  set; } = default!;
        public string Gender { get; set; } = default!;

    }
  
}
