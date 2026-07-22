using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.DoctorsCommandQueries.Dto
{
    public record DoctorsAvailableSlotDto
    {
       public  TimeOnly AvailableSlot { get; set; }
}
}
