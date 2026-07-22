using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.DoctorsCommandQueries.Dto
{
    public class WeeklyScheduleDto
    {
        public Guid DoctorId { get; set; }
        public  string DoctorName { get; set; }
        public DateOnly ScheduleDate { get; set; }
        public DayOfWeek DayOfWeek { get; set; }      // Monday, Tuesday, etc.
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int SlotDurationMinutes { get; set; }=15;   // e.g., 15 or 30 minutes
        public bool IsActive { get; set; }



      //  public Dictionary<DayOfWeek, List<DaySchedule>> Schedule { get; set; }
    }
 

}
