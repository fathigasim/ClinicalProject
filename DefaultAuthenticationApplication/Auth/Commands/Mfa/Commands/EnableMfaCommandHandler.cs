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
    public class EnableMfaCommandHandler(UserManager<ApplicationUser> userManager)
       : IRequestHandler<EnableMfaCommand, Result<List<string>>>
    {
        public async Task<Result<List<string>>> Handle(EnableMfaCommand request, CancellationToken ct)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user is null)
                return Result<List<string>>.Failure("User not found.");

            var isValid = await userManager.VerifyTwoFactorTokenAsync(
                user, userManager.Options.Tokens.AuthenticatorTokenProvider, request.Code);

            if (!isValid)
                return Result<List<string>>.Failure("Invalid code.");

            await userManager.SetTwoFactorEnabledAsync(user, true);
            var recoveryCodes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

            return Result<List<string>>.Success(recoveryCodes!.ToList());
        }
    }

}
