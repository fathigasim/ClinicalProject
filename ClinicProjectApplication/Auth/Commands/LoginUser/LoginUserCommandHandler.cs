
using ClinicProjectApplication.Auth.Vm;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Common.Exceptions;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.LoginUser
{
    public class LoginUserCommandHandler(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ILogger<LoginUserCommandHandler> logger,
    IUserRepository userRepository,
    TokenIssuer tokenIssuer,
    IMfaChallengeTokenService mfaChallengeTokenService)
    : IRequestHandler<LoginUserCommand, Result<LoginResponse>>
    {
        public async Task<Result<LoginResponse>> Handle(LoginUserCommand req, CancellationToken ct)
        {
          
            var user = await userRepository.GetByEmailAsync(req.Email, ct);
            if (user is null)
                return Result<LoginResponse>.Failure("Invalid credentials.");

            var signInResult = await signInManager.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: true);

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                    return Result<LoginResponse>.Failure("Account locked due to too many failed attempts. Try again later.");

                if (signInResult.IsNotAllowed)
                    return Result<LoginResponse>.Failure("Please confirm your email before logging in.");

                return Result<LoginResponse>.Failure("Invalid credentials.");
            }

            if (user.TwoFactorEnabled)
            {
                var mfaToken = mfaChallengeTokenService.GenerateChallengeToken(user.Id);
                return Result<LoginResponse>.Success(new LoginResponse(true, mfaToken, null));
            }

            var tokens = await tokenIssuer.IssueAsync(user, req.IpAddress, ct, amr: "pwd");
            return Result<LoginResponse>.Success(new LoginResponse(false, null, tokens));
        }
    }
}
