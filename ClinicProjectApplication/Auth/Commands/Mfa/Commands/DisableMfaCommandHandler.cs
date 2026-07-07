using ClinicProjectApplication.Auth.Commands.LoginUser;
using ClinicProjectApplication.Common;
using ClinicProjectDomain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.Mfa.Commands
{

    public class DisableMfaCommandHandler(UserManager<ApplicationUser> userManager)
        : IRequestHandler<DisableMfaCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(DisableMfaCommand request, CancellationToken ct)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user is null)
                return Result<bool>.Failure("User not found.");

            // Require password re-entry — a stolen access token alone shouldn't be able to turn off MFA
            if (!await userManager.CheckPasswordAsync(user, request.Password))
                return Result<bool>.Failure("Invalid password.");

            await userManager.SetTwoFactorEnabledAsync(user, false);
            await userManager.ResetAuthenticatorKeyAsync(user); // force re-enrollment if re-enabled later

            return Result<bool>.Success(true);
        }
    }
    



    public record MfaStatusResponse(bool Enabled, int RemainingRecoveryCodes);

  
  


public record RecoveryCodeUsedEvent(string UserId, string Email, DateTime UsedAtUtc) : INotification;

  

//csharp





}
