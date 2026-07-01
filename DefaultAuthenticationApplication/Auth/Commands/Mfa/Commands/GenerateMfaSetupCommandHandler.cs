using ClinicProjectApplication.Common;
using ClinicProjectDomain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.Mfa.Commands
{
    public class GenerateMfaSetupCommandHandler(UserManager<ApplicationUser> userManager)
     : IRequestHandler<GenerateMfaSetupCommand, Result<MfaSetupResponse>>
    {
        public async Task<Result<MfaSetupResponse>> Handle(GenerateMfaSetupCommand request, CancellationToken ct)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user is null)
                return Result<MfaSetupResponse>.Failure("User not found.");

            var key = await userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(key))
            {
                await userManager.ResetAuthenticatorKeyAsync(user);
                key = await userManager.GetAuthenticatorKeyAsync(user);
            }

            var uri = $"otpauth://totp/ClinicApp:{Uri.EscapeDataString(user.Email!)}" +
                      $"?secret={key}&issuer=ClinicApp&digits=6";

            return Result<MfaSetupResponse>.Success(new MfaSetupResponse(key!, uri));
        }
    }

}
