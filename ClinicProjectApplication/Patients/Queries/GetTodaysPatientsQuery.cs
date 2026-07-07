using ClinicProjectApplication.Common;
using ClinicProjectApplication.Patients.Dto;
using ClinicProjectApplication.Payment.Dtos;
using ClinicProjectDomain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Patients.Queries
{
    public class GetTodaysPatientsQuery :IRequest<Result<List<PatientDto>>>
    {
    }
}
