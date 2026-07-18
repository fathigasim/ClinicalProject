using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Prescription.Dtos;
using ClinicProjectDomain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Prescription
{
    public record CreatePrescriptionCommand(Guid patientId, string MedicationName,string dosage, int frequency,int durationInDays) : IRequest<Result<string>>,ITransactionalRequest;

   
}
