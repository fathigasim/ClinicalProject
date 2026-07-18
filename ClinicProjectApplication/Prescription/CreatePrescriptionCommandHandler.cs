using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Prescription.Dtos;
using ClinicProjectApplication.PrescriptionsItems.Dtos;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Prescription
{
    public class CreatePrescriptionCommandHandler : IRequestHandler<CreatePrescriptionCommand, Result<string>>
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IMapper _mapper;
        public CreatePrescriptionCommandHandler(IPrescriptionRepository prescriptionService, IMedicalRecordRepository medicalRecordRepository, IMapper mapper)
        {
            _prescriptionRepository = prescriptionService;
            _medicalRecordRepository = medicalRecordRepository;
            _mapper = mapper;
        }
        public async Task<Result<string>> Handle(CreatePrescriptionCommand request, CancellationToken cancellationToken)
        {
          var patientRecord=  await _medicalRecordRepository.PatientMedicalRecord(request.patientId);
            if (patientRecord != null)
            {
                var prescriptionDto = new PrescriptionsDto()
                {
                    MedicalRecordId = patientRecord.Id,
                    CreatedAt = DateTime.UtcNow,
                  
                };
            var prescription=    Prescriptions.CreatePrescription(prescriptionDto.MedicalRecordId);
                PrescriptionItems.Create(prescription.Id, request.MedicationName,request.dosage,request.frequency,request.durationInDays);
       //  var prescription=    _mapper.Map<Prescriptions>(prescriptionDto);
                
            //    prescription.PrescriptionItems.Add(new PrescriptionItems() { PrescriptionId = prescription.Id,MedicationName=request.MedicationName ,Dosage = request.dosage, Duration = DateTime.UtcNow, Frequency = request.frequency });
             await   _prescriptionRepository.AddAsync(prescription);
                return Result<string>.Success("Prescription added to patient");
            }
            return Result<string>.Failure("This patient has no medical record");

          
        }
    }
}
