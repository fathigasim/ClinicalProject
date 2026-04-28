
using DefaultAuthenticationApplication;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.RegisterUser
{
    public record RegisterUserCommand(
 
      string Email, string Password,
      string? IpAddress) : IRequest<AuthTokenPair>;
}
