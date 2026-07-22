using ClinicProjectApplication.PatientsCommandQueries.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.PatientsCommandQueries.Queries
{
    public record GetPatientByIdQuery (Guid id): IRequest<PatientDto>;
  
}
