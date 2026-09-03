using ClinicProjectApplication.Appointments.AppointmentCommand;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.DoctorsCommandQueries.Command.DoctorWeeklySchedule;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using Hangfire.States;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectTest.DoctorTest
{
    public class CreateDoctorScheduleCommandHandlerTests
    {
        private readonly Mock<IDoctorScheduleRepository> _doctorScheduleRepo;
        private readonly Mock<IDoctorRepository> _doctorRepo;
        private readonly CreateDoctorScheduleCommandHandler _handler;
        public CreateDoctorScheduleCommandHandlerTests()
        {
            _doctorScheduleRepo=new Mock<IDoctorScheduleRepository>();
            _doctorRepo = new Mock<IDoctorRepository>();
            _handler = new CreateDoctorScheduleCommandHandler(_doctorRepo.Object, _doctorScheduleRepo.Object);
        }


        [Theory]
        [InlineData("40B2FCA2-58AA-4D43-A13E-A9065CF563AA","2026-08-03","04:30","05:00")]
        public async Task CreateDoctorSchedulShouldFailWhenDoctorNotFound(string doctorId,string scheduleDate
            
            ,string startTime,string endTime)
        {

        
               _doctorRepo.Setup(r => r.GetByIdAsync(Guid.Parse(doctorId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            var command = new CreateDoctorScheduleCommand(Guid.Parse(doctorId), DateOnly.Parse(scheduleDate), TimeOnly.Parse(startTime), TimeOnly.Parse(endTime));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Doctor not found.",result.ErrorMessage);
        }


        [Theory]
        [InlineData("40B2FCA2-58AA-4D43-A13E-A9065CF563AA", "2026-08-03", "04:30", "05:00")]
        public async Task CreateDoctorSchedulShouldSucceedDoctorWhenDoctorFound(string doctorId, string scheduleDate

            , string startTime, string endTime)
        {
            var parsedDoctorId = Guid.Parse(doctorId);
            var doctor = new Doctor()
            {
                Id = parsedDoctorId,
                FirstName = "Ahmed",
                LastName = "Ali",
                CreatedAt = DateTime.Now,
                Email = "Ahmed@gmail.com",
                Specialization = "Diabetes",
                Phone = "+249951357426"
            };
            _doctorRepo.Setup(r => r.GetByIdAsync(doctor.Id, It.IsAny<CancellationToken>()))
           .ReturnsAsync(doctor);
            var doctorSchedule = DoctorSchedule.Create(doctor.Id, TimeOnly.Parse(startTime), TimeOnly.Parse(endTime), DateOnly.Parse(scheduleDate), 30);
          

            var command = new CreateDoctorScheduleCommand(doctor.Id, DateOnly.Parse(scheduleDate), TimeOnly.Parse(startTime), TimeOnly.Parse(endTime));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal("Doctor schedule created successfully.", result.Data);
        }
    }
    }

