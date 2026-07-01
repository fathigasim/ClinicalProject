using ClinicProjectApplication.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.Mfa.Commands
{
    public record GenerateMfaSetupCommand(string UserId) : IRequest<Result<MfaSetupResponse>>;
}
