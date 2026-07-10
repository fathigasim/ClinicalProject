using ClinicProjectApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Interfaces
{
    public interface IEmailSender
    {

        //    Task SendEmailAsync(
        //     string toEmail,
        //     string subject,
        //     string htmlMessage,
        //     CancellationToken cancellationToken = default);
        //}
        Task SendEmailAsync(
             string to,
             string subject,
             string body,
             bool isHtml = true,
             CancellationToken cancellationToken = default);
     //   Task SendTemplateEmailAsync(BrevoTemplateEmailRequest request, CancellationToken ct = default);
    }
}
