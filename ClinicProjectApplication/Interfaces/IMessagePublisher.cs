using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Interfaces
{
    // YourApp.Application/Abstractions/IMessagePublisher.cs
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default);
    }
}
