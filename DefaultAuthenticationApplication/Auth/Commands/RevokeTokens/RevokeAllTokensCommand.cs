using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Commands.RevokeTokens
{
    public record RevokeAllTokensCommand(
     string UserId, string? IpAddress) : IRequest;
}
