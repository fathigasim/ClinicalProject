
using ClinicProjectApplication.MedicalRecord;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultAutheClinicProjectApplicationnticationApplication.MedicalRecord
{
    public class CreateMedicalRecordCommandHandler:IRequestHandler<CreateMedicalRecordCommand, Guid>
    {
        private readonly IMedicalRecordRepository _repository;

        public CreateMedicalRecordCommandHandler(IMedicalRecordRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateMedicalRecordCommand request, CancellationToken cancellationToken)
        {

            var medicalRecord = new MedicalRecords
            {
               
                AppointmentId = request.AppointmentId,
                Diagnosis = request.Diagnosis,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow
            };

            await  _repository.AddAsync(medicalRecord);
        
            return medicalRecord.Id;
        }
    }
}
