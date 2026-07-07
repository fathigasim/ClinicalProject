using ClinicProjectApplication.Auth.Commands.LoginUser;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.Mfa.Commands
{
  
    public record RedeemRecoveryCodeCommand(
     string MfaToken, string RecoveryCode, string? IpAddress) : IRequest<Result<LoginResponse>>;
}

