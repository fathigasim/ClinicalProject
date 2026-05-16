using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Enums;
using ClinicProjectDomain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Notifications
{
    public class InvoicePaidNotificationHandler : INotificationHandler<InvoicePaidNotification>
    {
         private readonly IRepository<Invoices> _repository;
        private readonly ILogger<InvoicePaidNotificationHandler> _logger;
        public InvoicePaidNotificationHandler(IRepository<Invoices> repository, ILogger<InvoicePaidNotificationHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task Handle(InvoicePaidNotification notification, CancellationToken cancellationToken)
        {
        var invoice=   await _repository.GetByIdAsync(notification.InvoiceId, cancellationToken);
            if (invoice == null)
            {
                _logger.LogWarning("Invoice {InvoiceId} not found", notification.InvoiceId);
                return;
            }
            invoice.status = InvoiceStatus.Paid;
            _repository.Update(invoice);
            _logger.LogInformation("Invoice {InvoiceId} marked as paid", notification.InvoiceId);
        }
    }
}
