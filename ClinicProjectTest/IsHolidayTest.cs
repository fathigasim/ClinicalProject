using ClinicProjectDomain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectTest
{
    public class IsHolidayTest
    {

        [Theory]
        [InlineData(DayOfWeek.Friday,true)]
        [InlineData(DayOfWeek.Monday, false)]
        public void IsHoliday_ReturnsExpectedResult(DayOfWeek dayOfWeek,bool excpected)
        {
           var result=  DoctorSchedule.IsHoliday(dayOfWeek);
            //Assert.True(result);
            //Assert.False(result);

            result.Should().Be(excpected);
        }
    }
}
