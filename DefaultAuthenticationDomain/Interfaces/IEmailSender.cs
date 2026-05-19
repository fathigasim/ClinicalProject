using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Interfaces
{
    public interface IEmailSender
    {

        Task SendEmailAsync(
         string toEmail,
         string subject,
         string htmlMessage,
         CancellationToken cancellationToken = default);
    }
}
