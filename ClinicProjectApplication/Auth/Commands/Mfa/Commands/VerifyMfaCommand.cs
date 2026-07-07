using ClinicProjectApplication.Auth.Commands.LoginUser;
using ClinicProjectApplication.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.Mfa.Commands
{
    //public record VerifyMfaCommand(string MfaToken, string Code) : IRequest<Result<LoginResponse>>;
    public record VerifyMfaCommand(
    string MfaToken, string Code, string? IpAddress) : IRequest<Result<LoginResponse>>;
  
}
