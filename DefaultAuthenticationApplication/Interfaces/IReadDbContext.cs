using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Interfaces
{
    // Application/Common/Interfaces/IReadDbContext.cs
    // Expose only the read surface — queries depend on this, not AppDbContext directly
    public interface IReadDbContext
    {
        IQueryable<T> ReadSet<T>() where T : class;
    }

    // AppDbContext already implements this — just register the interface:
    // services.AddScoped<IReadDbContext>(sp => sp.GetRequiredService<AppDbContext>());
}
