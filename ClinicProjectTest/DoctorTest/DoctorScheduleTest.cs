using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectTest.DoctorTests
{
    public class DoctorScheduleTest
    {

        [Theory]
        [InlineData("C308143B-9FC3-427D-B569-05F436EEE338", "08:30", "08:00", "2026-07-30")]

        public void CreateDoctorScheduleShouldThrowExceptionWhenStartTimeBiggerThanEndTime(string scheduleId, string startTime, string endTime, string scheduleDate)
        {


            Assert.Throws<DomainException>(() => DoctorSchedule.Create(Guid.Parse(scheduleId), TimeOnly.Parse(startTime), TimeOnly.Parse(endTime), DateOnly.Parse(scheduleDate)));
        }
    }
}
