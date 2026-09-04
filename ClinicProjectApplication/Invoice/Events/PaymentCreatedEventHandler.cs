using ClinicProjectApplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Events
{
    // YourApp.Application/Payment/EventHandlers/PaymentCreatedEventHandler.cs
    public class PaymentCreatedEventHandler : IEventHandler<PaymentCreatedEvent>
    {
        //private readonly IInventoryService _inventoryService;

        //public PaymentCreatedEventHandler(IInventoryService inventoryService)
        //{
        //    _inventoryService = inventoryService;
        //}

        public async Task HandleAsync(PaymentCreatedEvent @event, CancellationToken ct = default)
        {
            // pure business logic — fully unit-testable, no RabbitMQ in sight
           // await _inventoryService.ReserveStockAsync(@event.Id, ct);
        }
    }
}
