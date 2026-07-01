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


    public class RedeemRecoveryCodeCommandHandler(
    IUserRepository userRepository,
    UserManager<ApplicationUser> userManager,
    TokenIssuer tokenIssuer,
    IMfaChallengeTokenService mfaTokenService,
    IMfaAttemptLimiter attemptLimiter,
    IPublisher publisher)   // MediatR's notification publisher
    : IRequestHandler<RedeemRecoveryCodeCommand, Result<LoginResponse>>
    {
        public async Task<Result<LoginResponse>> Handle(RedeemRecoveryCodeCommand request, CancellationToken ct)
        {
            if (!attemptLimiter.IsAllowed(request.MfaToken))
                return Result<LoginResponse>.Failure("Too many attempts. Please log in again.");

            var userId = mfaTokenService.ValidateAndGetUserId(request.MfaToken);
            if (userId is null)
                return Result<LoginResponse>.Failure("MFA session expired. Please log in again.");

            var user = await userRepository.GetByIdAsync(userId, ct);
            if (user is null)
                return Result<LoginResponse>.Failure("MFA session expired. Please log in again.");

            var result = await userManager.RedeemTwoFactorRecoveryCodeAsync(user, request.RecoveryCode);
            if (!result.Succeeded)
            {
                attemptLimiter.RecordFailure(request.MfaToken);
                return Result<LoginResponse>.Failure("Invalid or already-used recovery code.");
            }

            await publisher.Publish(new RecoveryCodeUsedEvent(user.Id, user.Email!, DateTime.UtcNow), ct);

            var tokenPair = await tokenIssuer.IssueAsync(user, request.IpAddress, ct, amr: "mfa_recovery");
            return Result<LoginResponse>.Success(new LoginResponse(false, null, tokenPair));
        }
    }
}
