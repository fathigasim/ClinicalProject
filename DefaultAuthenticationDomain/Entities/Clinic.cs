using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class Clinic
    {
        public TimeOnly OpenTime { get; set; } =new TimeOnly(8, 0); // Default opening time at 8:00 AM
        public TimeOnly CloseTime { get; set; } = new TimeOnly(21, 30); // Default closing time at 9:30 PM
    }
}
