


using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using ClinicProjectInfrastructure.Services;

namespace ClinicProjectInfrastructure.Persistence.Repositories
{
    public class AppointmentRepository :Repository<Appointment> ,IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context):base(context)
        {
            
        }
    }
}
