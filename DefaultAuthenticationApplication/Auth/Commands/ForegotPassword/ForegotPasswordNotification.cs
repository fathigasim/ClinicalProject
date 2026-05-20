using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.ForegotPassword
{
    public record ForegotPasswordNotification(string email,string subject,string message) :INotification
    {
     
    }
}
