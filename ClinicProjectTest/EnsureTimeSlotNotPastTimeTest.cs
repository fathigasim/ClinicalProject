using ClinicProjectDomain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectTest
{
    public class EnsureTimeSlotNotPastTimeTest
    {
        [Theory]
        [InlineData("2026-06-10", "07:30",false)]

        public void Validate_TimeSlotValid(string scheduledDate,string startTime,bool expected)
        {
           var result=    WeeklySchedule.IsTimeSlotValid(DateOnly.Parse(scheduledDate),TimeOnly.Parse(startTime));
          
              result.Should().Be(expected);
        
             
        }

        //[Theory]
        //[InlineData("08:30", false)]
        //public void Is_EnsureTimeSlotNotPastTime(string startTime, bool expected)
        //{
        //    var result = new WeeklySchedule().EnsureTimeSlotNotPastTime(TimeOnly.Parse(startTime));
        //    result.Should().Be(expected);
        // //   Assert.True(result);

        //}
    }
}
