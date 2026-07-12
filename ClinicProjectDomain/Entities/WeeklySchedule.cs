using ClinicProjectDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class WeeklySchedule :BaseEntity, IAuditableEntity
    {
       
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public DayOfWeek DayOfWeek  { get; set; }

        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public int SlotDurationMinutes { get; set; }
        public bool IsActive { get; set; }

        public DateOnly ScheduledDate { get; set; }
        public bool IsClincOpened(Clinic clinic) { 
         
           if(StartTime<clinic.OpenTime || EndTime > clinic.CloseTime)
            {
                return false;
            }
         return true;
        }
        public bool IsHoliday (DayOfWeek day)
        {
            return  day == DayOfWeek.Friday;//day == DayOfWeek.Saturday ||
        }
        public bool IsValid()
        {
            return StartTime < EndTime &&
                   SlotDurationMinutes > 0 &&
                   (EndTime - StartTime).TotalMinutes % SlotDurationMinutes == 0;
        }
        public bool EnsureTimeIsValid()
        {
             var current = StartTime;
            var todaysDate = DateTime.Now;
            if (TimeOnly.FromDateTime(todaysDate) < current)
            {
                return true;
            }

            return false;
        }
        public static bool IsTimeSlotValid(DateOnly scheduleDate,TimeOnly startTime)
        {

            var now = DateTime.Now;
            if (scheduleDate < DateOnly.FromDateTime(now.Date))
            {
                return false;
            }
            if (scheduleDate == DateOnly.FromDateTime(now.Date ))
            {

                return startTime > TimeOnly.FromDateTime(now);
            }
        
         

            return true;
        }
        public IEnumerable<TimeOnly> GenerateSlots()
        {
            var slots = new List<TimeOnly>();
            var current = StartTime;
            var todaysDate = DateTime.Now;
           
            while (current.AddMinutes(SlotDurationMinutes) <= EndTime)
            {
                slots.Add(current);
                current = current.AddMinutes(SlotDurationMinutes);
            }

            return slots;
        }

        public bool IsTimeSlotValid(TimeOnly time)
        {
            return time >= StartTime &&
                   time.AddMinutes(SlotDurationMinutes) <= EndTime;
        }

        public bool IsOverlapping(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2)
        {
            return start1 < end2 && start2 < end1;
        }
    }
}
