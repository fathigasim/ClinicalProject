using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class WeeklySchedule
    {
        public int Id { get; set; }
        public Guid DoctorId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }      // Monday, Tuesday, etc.
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int SlotDurationMinutes { get; set; }  // e.g., 15 or 30 minutes
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Doctor Doctor { get; set; }
         
           public bool IsHoliday (DayOfWeek dayOfWeek)
        {

            if (dayOfWeek == DayOfWeek.Friday || dayOfWeek == DayOfWeek.Saturday)
            {
                return true;
            }
            return false;   
        }
            public bool IsTimeSlotAvailable(DateTime appointmentDateTime)
            {
                if (!IsActive)
                    return false;
                if (appointmentDateTime.DayOfWeek != DayOfWeek)
                    return false;
                var appointmentTime = appointmentDateTime.TimeOfDay;
                return appointmentTime >= StartTime && appointmentTime + TimeSpan.FromMinutes(SlotDurationMinutes) <= EndTime;
        }
        public IEnumerable<TimeSpan> GetAvailableTimeSlots()
            {
                var timeSlots = new List<TimeSpan>();
                var currentTime = StartTime;
    
                while (currentTime + TimeSpan.FromMinutes(SlotDurationMinutes) <= EndTime)
                {
                    timeSlots.Add(currentTime);
                    currentTime = currentTime.Add(TimeSpan.FromMinutes(SlotDurationMinutes));
                }
    
                return timeSlots;
        }
    }
}
