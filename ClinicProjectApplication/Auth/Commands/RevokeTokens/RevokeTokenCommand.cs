using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.RevokeTokens
{
    public record RevokeTokenCommand(
    string RefreshToken, string? IpAddress) : IRequest;
}
