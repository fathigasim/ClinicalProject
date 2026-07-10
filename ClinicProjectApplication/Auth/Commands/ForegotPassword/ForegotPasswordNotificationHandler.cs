using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.ForegotPassword
{
    public class ForegotPasswordNotificationHandler: INotificationHandler<ForegotPasswordNotification>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ForegotPasswordNotificationHandler> _logger;
        public ForegotPasswordNotificationHandler(UserManager<ApplicationUser> userManager, IEmailSender emailSender,
             ILogger<ForegotPasswordNotificationHandler> logger
            )
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;   
        }
        public async Task Handle(ForegotPasswordNotification notification, CancellationToken cancellationToken)
        {
            try
            {
                await _emailSender.SendEmailAsync(notification.email, notification.subject, notification.message, true,cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Falied to send email {error}",ex.Message);
            }
  
        }
    }
}
