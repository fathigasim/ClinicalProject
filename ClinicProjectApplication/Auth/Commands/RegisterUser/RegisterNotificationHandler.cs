using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Interfaces;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.RegisterUser
{
    public class RegisterNotificationHandler(IEmailSender _emailSender, IBackgroundJobClient _backgroundJobClient
        , ILogger<RegisterNotificationHandler> _logger
        , IConfiguration _configuration) :IEventHandler<RegisterNotification>
    {
    
//        public async Task Handle(RegisterNotification notification, CancellationToken cancellationToken)
//        {
//            try
//            {
//                // Build confirmation link — pass token + email to your confirm endpoint
//                var encodedToken = WebEncoders.Base64UrlEncode(
//                    Encoding.UTF8.GetBytes(notification.ConfirmToken));

//                var confirmUrl = $"{_configuration["Frontend:BaseUrl"]}/auth/confirm-email" +
//                                 $"?email={Uri.EscapeDataString(notification.Email)}" +
//                                 $"&token={encodedToken}";
////             
////);

//                _backgroundJobClient.Enqueue<IEmailSender>(x => x.SendEmailAsync(notification.Email,
//               "Confirm your email",
//               $"Click to confirm your account: <a href='{confirmUrl}'>Confirm Email</a>",true,CancellationToken.None));

//                _logger.LogInformation("Email sent To {Email} successfully", notification.Email,default);
//                await _messagePublisher.PublishAsync(new RegisterNotification(notification.Email, notification.ConfirmToken), "register.notification", cancellationToken);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogInformation("Failed to send to {email}", notification.Email);
//            }


//        }

        public  async Task HandleAsync(RegisterNotification @event, CancellationToken ct = default)
        {
            try
            {
                // Build confirmation link — pass token + email to your confirm endpoint
                var encodedToken = WebEncoders.Base64UrlEncode(
                    Encoding.UTF8.GetBytes(@event.ConfirmToken));

                var confirmUrl = $"{_configuration["Frontend:BaseUrl"]}/auth/confirm-email" +
                                 $"?email={Uri.EscapeDataString(@event.Email)}" +
                                 $"&token={encodedToken}";
                //             
                //);

                _backgroundJobClient.Enqueue<IEmailSender>(x => x.SendEmailAsync(@event.Email,
               "Confirm your email",
               $"Click to confirm your account: <a href='{confirmUrl}'>Confirm Email</a>", true, CancellationToken.None));

                _logger.LogInformation("Email sent To {Email} successfully", @event.Email, default);
              
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed to send to {email}", @event.Email);
                throw;
            }
        }
    }
}
