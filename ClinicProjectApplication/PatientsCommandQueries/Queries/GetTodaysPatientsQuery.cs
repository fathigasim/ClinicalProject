using ClinicProjectApplication.Common;
using ClinicProjectApplication.PatientsCommandQueries.Dto;
using ClinicProjectApplication.Payment.Dtos;
using ClinicProjectDomain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.PatientsCommandQueries.Queries
{
    public class GetTodaysPatientsQuery :IRequest<Result<List<PatientDto>>>
    {
    }
}
