using FluentAssertions.Common;

namespace ClinicProjectTest
{
    public class IsTimeSlotValidTests
    {
        private readonly WorkingHours _sut;

        public IsTimeSlotValidTests()
        {
            // Assume 09:00 – 17:00, 30-min slots
            _sut = new WorkingHours
            {
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(17, 0),
                SlotDurationMinutes = 30
            };
        }

        //  Happy path
        [Fact]
        public void ValidSlot_WithinRange_ReturnsTrue()
        {
            var result = _sut.IsTimeSlotValid(new TimeOnly(10, 0));
            Assert.True(result);
        }

        //  Boundary: exactly at StartTime
        [Fact]
        public void ValidSlot_AtStartTime_ReturnsTrue()
        {
            var result = _sut.IsTimeSlotValid(new TimeOnly(9, 0));
            Assert.True(result);
        }

        //  Boundary: slot ends exactly at EndTime
        [Fact]
        public void ValidSlot_EndingExactlyAtEndTime_ReturnsTrue()
        {
            var result = _sut.IsTimeSlotValid(new TimeOnly(16, 30));
            Assert.True(result);
        }

        //  Boundary: slot would exceed EndTime
        [Fact]
        public void InvalidSlot_ExceedsEndTime_ReturnsFalse()
        {
            var result = _sut.IsTimeSlotValid(new TimeOnly(16, 31));
            Assert.False(result);
        }

        //  Before working hours
        [Fact]
        public void InvalidSlot_BeforeStartTime_ReturnsFalse()
        {
            var result = _sut.IsTimeSlotValid(new TimeOnly(8, 0));
            Assert.False(result);
        }

        //  After working hours
        [Fact]
        public void InvalidSlot_AfterEndTime_ReturnsFalse()
        {
            var result = _sut.IsTimeSlotValid(new TimeOnly(17, 30));
            Assert.False(result);
        }

        [Theory]
        [InlineData(9, 0, true)]   // exactly at start
        [InlineData(10, 0, true)]   // mid-range
        [InlineData(16, 30, true)]   // slot ends exactly at EndTime
        [InlineData(16, 31, false)]  // slot bleeds past EndTime
        [InlineData(8, 0, false)]  // before start
        [InlineData(17, 30, false)]  // after end
        public void IsTimeSlotValid_ReturnsExpectedResult(int hour, int minute, bool expected)
        {
            var sut = new WorkingHours
            {
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(17, 0),
                SlotDurationMinutes = 30
            };

            var result = sut.IsTimeSlotValid(new TimeOnly(hour, minute));

            Assert.Equal(expected, result);
        }
    }
}