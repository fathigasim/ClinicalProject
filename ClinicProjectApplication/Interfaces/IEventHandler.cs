using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Interfaces
{
    // YourApp.Application/Abstractions/IEventHandler.cs
    public interface IEventHandler<in TEvent>
    {
        Task HandleAsync(TEvent @event, CancellationToken ct = default);
    }
}
