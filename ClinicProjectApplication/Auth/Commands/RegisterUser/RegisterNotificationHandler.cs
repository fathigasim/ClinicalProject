using ClinicProjectDomain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.RegisterUser
{
    public class RegisterNotificationHandler : INotificationHandler<RegisterNotification>
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<RegisterNotificationHandler> _logger;
        private readonly IConfiguration _configuration;
        public RegisterNotificationHandler(IEmailSender emailSender,
            ILogger<RegisterNotificationHandler> logger, IConfiguration configuration)
        {
            _emailSender= emailSender;
            _logger= logger;
            _configuration= configuration;
        }
        public async Task Handle(RegisterNotification notification, CancellationToken cancellationToken)
        {
            try
            {
                // Build confirmation link — pass token + email to your confirm endpoint
                var encodedToken = WebEncoders.Base64UrlEncode(
                    Encoding.UTF8.GetBytes(notification.ConfirmToken));

                var confirmUrl = $"{_configuration["Frontend:BaseUrl"]}/auth/confirm-email" +
                                 $"?email={Uri.EscapeDataString(notification.Email)}" +
                                 $"&token={encodedToken}";
                await _emailSender.SendEmailAsync(
                 notification.Email,
                subject: "Confirm your email",
                 $"Click to confirm your account: <a href='{confirmUrl}'>Confirm Email</a>"
);

                _logger.LogInformation("Email sent To {Email} successfully", notification.Email);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed to send to {email}", notification.Email);
            }


        }
    }
}
