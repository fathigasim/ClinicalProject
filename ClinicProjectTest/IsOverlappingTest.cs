using ClinicProjectDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectTest
{
    public class IsOverlappingTests
    {
        private readonly WeeklySchedule _sut = new();

        //  True overlaps
        [Theory]
        [InlineData("09:00", "10:00", "09:30", "10:30")] // partial overlap
        [InlineData("09:00", "11:00", "09:30", "10:30")] // one contains the other
        [InlineData("09:30", "10:30", "09:00", "11:00")] // reverse containment
        [InlineData("09:00", "10:00", "09:00", "10:00")] // identical slots
        public void IsOverlapping_WhenSlotsOverlap_ReturnsTrue(
            string s1, string e1, string s2, string e2)
        {
            var result = _sut.IsOverlapping(
                TimeOnly.Parse(s1), TimeOnly.Parse(e1),
                TimeOnly.Parse(s2), TimeOnly.Parse(e2));

            Assert.True(result);
        }

        //  Non-overlapping
        [Theory]
        [InlineData("09:00", "09:30", "09:30", "10:30")] // adjacent (touching)
        [InlineData("09:00", "09:30", "10:00", "11:00")] // gap between them
        [InlineData("10:00", "11:00", "09:00", "09:30")] // second is before first
        public void IsOverlapping_WhenSlotsAreAdjacentOrSeparate_ReturnsFalse(
            string s1, string e1, string s2, string e2)
        {
            var result = _sut.IsOverlapping(
                TimeOnly.Parse(s1), TimeOnly.Parse(e1),
                TimeOnly.Parse(s2), TimeOnly.Parse(e2));

            Assert.False(result);
        }
    }

}
          
  
