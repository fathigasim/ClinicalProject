using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Doctors.Dto
{
    public record DoctorsScheduleDto
    {
 
        public Dictionary<DateOnly, List<DaySchedule>> Schedule { get; set; }
    }
    public class DaySchedule
    {
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateOnly ScheduleDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int SlotDurationMinutes { get; set; } = 15;
        public bool IsActive { get; set; }
    }
}
