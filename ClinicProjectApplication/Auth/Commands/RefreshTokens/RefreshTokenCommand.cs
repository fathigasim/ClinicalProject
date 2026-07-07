
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.RefreshTokens
{
    public record RefreshTokenCommand(
  string RefreshToken,
   string? IpAddress) : IRequest<AuthTokenPair>;

}
