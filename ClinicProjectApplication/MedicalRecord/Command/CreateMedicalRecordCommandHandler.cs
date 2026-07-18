
using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
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

namespace ClinicProjectApplication.MedicalRecord.Command
{
    public class CreateMedicalRecordCommandHandler:IRequestHandler<CreateMedicalRecordCommand, Result<string>>
    {
        private readonly IMedicalRecordRepository _repository;
        private readonly ISequenceService _sequenceService;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public CreateMedicalRecordCommandHandler(IMedicalRecordRepository repository,
            IAppointmentRepository appointmentRepository, IMapper mapper, ISequenceService sequenceService)
        {
            _repository = repository;
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
            _sequenceService = sequenceService;
        }

        public async Task<Result<string>> Handle(CreateMedicalRecordCommand request, CancellationToken cancellationToken)
        {
        var appointment=   await _appointmentRepository.GetByAppointmentNumberAsync(request.AppointmentNumber, cancellationToken);
            if (appointment == null)
            {
                return Result<string>.Failure("Patient with this appointment number is not available");
            }
            var medicalRecordDto = new MedicalRecordDto
            {

                AppointmentId = appointment.Id,
                Diagnosis = request.Diagnosis,
                
              //  CreatedAt = DateTime.UtcNow,
            

            };
            var MedicalRecordNumber = await _sequenceService.GenerateMedicalNumberAsync();
            var medicalRecored=   MedicalRecords.Create(medicalRecordDto.AppointmentId, MedicalRecordNumber, medicalRecordDto.Diagnosis);
         var prescriptions=   Prescriptions.CreatePrescription(medicalRecored.Id);
          //  PrescriptionItems.Create(prescriptions.Id, request.MedicationName, request.Dosage, request.Frequency, request.duration);
            
            prescriptions.AddItem( request.MedicationName, request.Dosage, request.Frequency, request.duration);
            medicalRecored.AddPrescription(prescriptions);

            await  _repository.AddAsync(medicalRecored);
        
            return Result<string>.Success($"Medical record created with ID: {medicalRecored.AppointmentId}");
        }
    }
}
