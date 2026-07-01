using ClinicProjectApplication.Auth.Commands.Mfa.Commands;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.Mfa.Events
{
    public class SendRecoveryCodeUsedEmailHandler(IEmailSender emailSender)
       : INotificationHandler<RecoveryCodeUsedEvent>
    {
        public async Task Handle(RecoveryCodeUsedEvent notification, CancellationToken ct)
        {
            await emailSender.SendEmailAsync(
                notification.Email,
                subject: "A recovery code was used to sign in to your account",
                htmlMessage: $"A recovery code was used to access your account at {notification.UsedAtUtc:u} UTC. " +
                      "If this wasn't you, change your password immediately and review your account security.");
        }
    }
    //Fire it from RedeemRecoveryCodeCommandHandler, right after successful redemption:
}
