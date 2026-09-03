using ClinicProjectApplication.DoctorsCommandQueries.Command.DoctorWeeklySchedule;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectTest.DoctorTest
{
    public class UpdateDoctorScheduleCommandHandlerTests
    {
        private readonly Mock<IDoctorScheduleRepository> _doctorScheduleRepository;
        private readonly UpdateDoctorScheduleCommandHandler _handler;

        public UpdateDoctorScheduleCommandHandlerTests()
        {
            _doctorScheduleRepository = new Mock<IDoctorScheduleRepository>();
            _handler = new UpdateDoctorScheduleCommandHandler(_doctorScheduleRepository.Object);
        }
        [Theory]
        [InlineData("40B2FCA2-58AA-4D43-A13E-A9065CF563AA","2026-08-03","04:00","04:30")]
        public async Task UpdateDoctorScheduleShouldReturnNotFound(string doctorId,string scheduleDate,string startTime,string endTime)
        {

            _doctorScheduleRepository.Setup(r => r.DoctorsScheduleById(Guid.Parse(doctorId), It.IsAny<CancellationToken>()))
             .ReturnsAsync((DoctorSchedule?)null);
            var command = new UpdateDoctorScheduleCommand(Guid.Parse(doctorId),DateOnly.Parse(scheduleDate),TimeOnly.Parse(startTime),TimeOnly.Parse(endTime));
            var result =await _handler.Handle(command,CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("No Schedule has been found", result.ErrorMessage);
        }

        [Theory]
        [InlineData("40B2FCA2-58AA-4D43-A13E-A9065CF563AA", "2026-08-03", "08:00", "12:00", "09:00", "13:00")]
        public async Task UpdateDoctorSchedule_ShouldSucceed_WhenScheduleExists(
    string docId,
    string scheduleDate,
    string originalStartTime,
    string originalEndTime,
    string newStartTime,
    string newEndTime)
        {
            // Arrange
            var doctorId = Guid.Parse(docId);

            // 1. Create the existing schedule entity
            var existingSchedule = DoctorSchedule.Create(
                doctorId: doctorId,
                startTime: TimeOnly.Parse(originalStartTime),
                endTime: TimeOnly.Parse(originalEndTime),
                scheduledDate: DateOnly.Parse(scheduleDate),
                slotDurationMinutes: 30
            );

            // 2. Setup Moq with the exact ID your handler queries
            _doctorScheduleRepository
                .Setup(r => r.DoctorsScheduleById(doctorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSchedule);

            // 3. Command with NEW updated values
            var command = new UpdateDoctorScheduleCommand(
                Id: doctorId,
                ScheduleDate: DateOnly.Parse(scheduleDate),
                StartTime: TimeOnly.Parse(newStartTime),
                EndTime: TimeOnly.Parse(newEndTime)
            );

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            //Assert.Equal(TimeOnly.Parse(newStartTime), existingSchedule.StartTime); // Verify state updated
         }
    }
}
