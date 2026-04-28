
using ClinicProjectApplication.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.DeleteUser
{
    //  Admin-only command
    public record DeleteUserCommand(string UserId)
        : IRequest<Unit>, ITransactionalRequest
    {
       
    }
}
