using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Enums;
using ClinicProjectDomain.Interfaces;
using Hangfire.Server;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Notifications
{
    public class InvoicePaidNotificationHandler : INotificationHandler<InvoicePaidNotification>,ITransactionalRequest
    {
         private readonly IRepository<Invoices> _repository;
        private readonly ILogger<InvoicePaidNotificationHandler> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IMessagePublisher _publisher;
        public InvoicePaidNotificationHandler(IRepository<Invoices> repository, ILogger<InvoicePaidNotificationHandler> logger,
            IEmailSender emailSender, IMessagePublisher publisher)
        {
            _repository = repository;
            _logger = logger;
            _emailSender = emailSender;
            _publisher = publisher;
        }
        public async Task Handle(InvoicePaidNotification notification, CancellationToken cancellationToken)
        {
        var invoice=   await _repository.GetByIdAsync(notification.InvoiceId, cancellationToken);
            if (invoice == null)
            {
                _logger.LogWarning("Invoice {InvoiceId} not found", notification.InvoiceId);
                return;
            }
            //invoice.status = InvoiceStatus.Paid;
            invoice.UpdateStatus(InvoiceStatus.Paid);
            _repository.Update(invoice);
            _logger.LogInformation("Invoice {InvoiceId} marked as paid and Email{Emaiil}", notification.InvoiceId,notification.Email);
            await _emailSender.SendEmailAsync(notification.Email, "Invoice Paid Successfully", $"Dear Customer {notification.Email} your bill has been paid successfully best regards", true,cancellationToken);
            // await _publisher.PublishAsync(_emailSender.SendEmailAsync(notification.Email, "Invoice Paid Successfully", $"Dear Customer {notification.Email} your bill has been paid successfully best regards", "orders-queue",true ,cancellationToken));
            await _publisher.PublishAsync("new OrderCreatedEvent(order.Id, order.CustomerId)", "orders-queue", cancellationToken);
        }
    }
}
