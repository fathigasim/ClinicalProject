using AutoMapper;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.MedicalRecord.Command;
using ClinicProjectApplication.MedicalRecord.Dtos;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectTest.MedicalTests
{
    public class CreateMedicalRecordCommandHandlerTests
    {
        private readonly Mock< IMedicalRecordRepository> _repository;
        private readonly Mock<ISequenceService> _sequenceService;
        private readonly Mock<IAppointmentRepository> _appointmentRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly CreateMedicalRecordCommandHandler _handler;
        public CreateMedicalRecordCommandHandlerTests()
        {
            _repository = new Mock<IMedicalRecordRepository>();
            _sequenceService = new Mock<ISequenceService>();
            _appointmentRepository= new Mock<IAppointmentRepository>();
            _mapper = new Mock<IMapper>();
            _handler = new CreateMedicalRecordCommandHandler(_repository.Object,_appointmentRepository.Object,_mapper.Object,_sequenceService.Object);
        }

        [Theory]
        [InlineData("App-202", "0721C236-C508-4398-AE77-01D150CA6E71", "0721C236-C508-4398-AE77-01D150CA6E81",
            "2026-08-04","09:00","Pain"
            )]
        [InlineData("App-202", "0721C236-C508-4398-AE77-01D150CA6E71", "0721C236-C508-4398-AE77-01D150CA6E81",
            "2026-08-04", "09:00", "Pain"
            )]
        public async Task MedicalRecordShouldRetrunSucceedWhenAppointmentNotNull(string appointmentNumber
            ,string patientId,string doctorId,string appointmentDate,string startTime,string notes
            )
        {
        var appointment=    Appointment.CreateAppointment(appointmentNumber: appointmentNumber, patientId: Guid.Parse(patientId),
                doctorId: Guid.Parse(doctorId),appointmentDate:DateOnly.Parse(appointmentDate) ,startTime:TimeOnly.Parse(startTime),notes:notes,durationMinutes: 30);

            _appointmentRepository.Setup(r => r.GetByAppointmentNumberAsync(appointment.AppointmentNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);

            var command = new CreateMedicalRecordCommand(AppointmentNumber: appointment.AppointmentNumber, Diagnosis: "Pain", MedicationName: "Panadol", Dosage: "Twice", Frequency: 2, Duration: 15);
           var result=await _handler.Handle(command,CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal($"Medical record created",result.Data);
        }
    }
}
