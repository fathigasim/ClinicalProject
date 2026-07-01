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
    //public class VerifyMfaCommandHandler(
    //     UserManager<ApplicationUser> userManager,
    //     IUserRepository userRepository,
    //     IMfaChallengeTokenService mfaTokenService,
    //     TokenIssuer tokenIssuer)
    //     : IRequestHandler<VerifyMfaCommand, Result<LoginResponse>>
    //{
    //    public async Task<Result<LoginResponse>> Handle(VerifyMfaCommand request, CancellationToken ct)
    //    {
    //        var userId = mfaTokenService.ValidateAndGetUserId(request.MfaToken);
    //        if (userId is null)
    //            return Result<LoginResponse>.Failure("MFA session expired. Please log in again.");

    //        // Use the repository, not UserManager.FindByIdAsync, so RefreshTokens is loaded
    //        // (TokenIssuer.IssueAsync depends on user.RefreshTokens being populated)
    //        var user = await userRepository.GetByIdAsync(userId, ct);
    //        if (user is null)
    //            return Result<LoginResponse>.Failure("MFA session expired. Please log in again.");

    //        var isValid = await userManager.VerifyTwoFactorTokenAsync(
    //            user, userManager.Options.Tokens.AuthenticatorTokenProvider, request.Code);

    //        if (!isValid)
    //        {
    //            await userManager.AccessFailedAsync(user);
    //            return Result<LoginResponse>.Failure("Invalid code.");
    //        }

    //        await userManager.ResetAccessFailedCountAsync(user);

    //        var tokens = await tokenIssuer.IssueAsync(user, request.IpAddress, ct, amr: "mfa");

    //        return Result<LoginResponse>.Success(new LoginResponse(false, null, tokens));

    //    }
    //}
    public class VerifyMfaCommandHandler(
        IMfaAttemptLimiter mfaAttemptLimiter,
       UserManager<ApplicationUser> userManager,
       IUserRepository userRepository,
       IMfaChallengeTokenService mfaTokenService,
       TokenIssuer tokenIssuer)
       : IRequestHandler<VerifyMfaCommand, Result<LoginResponse>>
    {
        public async Task<Result<LoginResponse>> Handle(VerifyMfaCommand request, CancellationToken ct)
        {
            if (!mfaAttemptLimiter.IsAllowed(request.MfaToken))
                return Result<LoginResponse>.Failure("Too many attempts. Please log in again.");
            var userId = mfaTokenService.ValidateAndGetUserId(request.MfaToken);
            if (userId is null)
                return Result<LoginResponse>.Failure("MFA session expired. Please log in again.");

            // Use the repository, not UserManager.FindByIdAsync, so RefreshTokens is loaded
            // (TokenIssuer.IssueAsync depends on user.RefreshTokens being populated)
            var user = await userRepository.GetByIdAsync(userId, ct);
            if (user is null)
                return Result<LoginResponse>.Failure("MFA session expired. Please log in again.");

            var isValid = await userManager.VerifyTwoFactorTokenAsync(
                user, userManager.Options.Tokens.AuthenticatorTokenProvider, request.Code);

            if (!isValid)
            {
                mfaAttemptLimiter.RecordFailure(request.MfaToken);
                await userManager.AccessFailedAsync(user);
                return Result<LoginResponse>.Failure("Invalid code.");
            }
       

            await userManager.ResetAccessFailedCountAsync(user);

            var tokens = await tokenIssuer.IssueAsync(user, request.IpAddress, ct, amr: "mfa");

            return Result<LoginResponse>.Success(new LoginResponse(false, null, tokens));

        }
    }
}