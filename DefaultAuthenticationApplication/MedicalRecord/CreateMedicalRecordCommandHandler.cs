
using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.MedicalRecord;
using ClinicProjectApplication.MedicalRecord.Dtos;
using ClinicProjectApplication.Prescription.Dtos;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultAutheClinicProjectApplicationnticationApplication.MedicalRecord
{
    public class CreateMedicalRecordCommandHandler:IRequestHandler<CreateMedicalRecordCommand, Result<string>>
    {
        private readonly IMedicalRecordRepository _repository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public CreateMedicalRecordCommandHandler(IMedicalRecordRepository repository,
            IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _repository = repository;
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(CreateMedicalRecordCommand request, CancellationToken cancellationToken)
        {
        var appointment=   await _appointmentRepository.GetByAppointmentNumberAsync(request.AppointmentNumber, cancellationToken);
            if (appointment == null)
            {
                return Result<string>.Failure("Appointment not available");
            }
            var medicalRecord = new MedicalRecordDto
            {

                AppointmentId = appointment.Id,
                Diagnosis = request.Diagnosis,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow,
            

            };
            
            var medicalRecordEntity = _mapper.Map<MedicalRecords>(medicalRecord);
            medicalRecordEntity.Prescriptions = new List<Prescriptions> {
                   new Prescriptions { MedicalRecordId=medicalRecordEntity.Id,
                   PrescriptionItems=new List<PrescriptionItems>
                   {
                       new PrescriptionItems
                       {
                           MedicationName=request.MedicationName,
                           Dosage=request.Dosage,
                           Frequency=request.Frequency,
                           Duration=request.Duration
                       }
                   }
                   
                      
                   }

                };
            await  _repository.AddAsync(medicalRecordEntity);
        
            return Result<string>.Success($"Medical record created with ID: {medicalRecordEntity.AppointmentId}");
        }
    }
}
