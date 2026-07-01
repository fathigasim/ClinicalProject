using ClinicProjectApplication.Auth.Commands.Mfa.Commands;
using ClinicProjectApplication.Common;
using ClinicProjectDomain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.Mfa.Queries
{
    public class GetMfaStatusQueryHandler(UserManager<ApplicationUser> userManager)
       : IRequestHandler<GetMfaStatusQuery, Result<MfaStatusResponse>>
    {
        public async Task<Result<MfaStatusResponse>> Handle(GetMfaStatusQuery request, CancellationToken ct)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user is null)
                return Result<MfaStatusResponse>.Failure("User not found.");

            var enabled = await userManager.GetTwoFactorEnabledAsync(user);
            var remaining = enabled ? await userManager.CountRecoveryCodesAsync(user) : 0;

            return Result<MfaStatusResponse>.Success(new MfaStatusResponse(enabled, remaining));
        }
    }
}
