using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectTest
{
    public class WorkingHours
    {
        public TimeOnly StartTime { get; set; }// = new TimeOnly(9, 0);
        public TimeOnly EndTime { get; set; }// = new TimeOnly(17, 0);
        public int SlotDurationMinutes { get; set; } = 30;
        public bool IsTimeSlotValid(TimeOnly time)
        {
            return time >= StartTime &&
                   time.AddMinutes(SlotDurationMinutes) <= EndTime;
        }
    }
}
