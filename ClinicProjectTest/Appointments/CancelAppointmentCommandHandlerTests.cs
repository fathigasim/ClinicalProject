using ClinicProjectApplication.Appointments.AppointmentCommand;
using ClinicProjectApplication.Appointments.Dtos;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Enums;
using ClinicProjectDomain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectTest.Appointments
{
    public class CancelAppointmentCommandHandlerTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepo;
        private readonly CancelAppointmentCommandHandler _handler;
        public CancelAppointmentCommandHandlerTests()
            {
            _appointmentRepo = new Mock<IAppointmentRepository>();
            _handler= new CancelAppointmentCommandHandler(_appointmentRepo.Object);
                }
        [Theory]
        [InlineData("App-14")]
        public async Task CancelAppointmentShouldFailWhenNoAppointment(string AppointmentNo)
        {

            _appointmentRepo.Setup(r => r.GetByAppointmentNumberAsync(AppointmentNo,It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Appointment?)null);

            var command=new CancelAppointmentCommand(AppointmentNo);

            var result =await _handler.Handle(command, CancellationToken.None);
            //assert
            Assert.False(result.IsSuccess);
            
            Assert.Equal("Appointment cannot be canceled", result.ErrorMessage);
        }

        [Theory]
        [InlineData("App-14","40B2FCA2-58AA-4D43-A13E-A9065CF563AA", "40B2FCA2-58AA-4D43-A13E-A9065CF563CC","2026-08-03","08:00","Pain in joints",15)]
        public async Task CancelAppointmentShouldSucceedWhenAppointmentExist(string appointmentNo,string patientId,string doctorId,string appointmentDate,string startTime,string notes,int duration)
        {
            var appointment = Appointment.CreateAppointment(appointmentNo,Guid.Parse(patientId),Guid.Parse(doctorId),DateOnly.Parse(appointmentDate), TimeOnly.Parse(startTime), notes, duration);

            _appointmentRepo.Setup(r => r.GetByAppointmentNumberAsync(appointmentNo, It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);
            var command = new CancelAppointmentCommand(appointmentNo);
            var result = await _handler.Handle(command, CancellationToken.None);

              Assert.True(result.IsSuccess);
            Assert.Equal("Appointment canceled successfully", result.Data);
            // Verify side effects
            //_appointmentRepo.Verify(r => r.Update(appointment), Times.Once);
            Assert.Equal(AppointmentStatus.Cancelled, appointment.Status);
        }
        }
}
