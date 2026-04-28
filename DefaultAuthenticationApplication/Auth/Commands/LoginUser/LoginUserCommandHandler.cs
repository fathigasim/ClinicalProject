
using ClinicProjectApplication.Common.Exceptions;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.LoginUser
{
    // Application/Auth/Commands/LoginUser/LoginUserCommandHandler.cs


  

    public class LoginUserCommandHandler(
        UserManager<ApplicationUser> userManager,
        IUserRepository userRepository,
        TokenIssuer tokenIssuer)
        : IRequestHandler<LoginUserCommand, AuthTokenPair>
    {
        public async Task<AuthTokenPair> Handle(LoginUserCommand req, CancellationToken ct)
        {
            var user = await userRepository.GetByEmailAsync(req.Email, ct)
                ?? throw new UnauthorizedException("Invalid credentials.");

            if (!await userManager.CheckPasswordAsync(user, req.Password))
                throw new UnauthorizedException("Invalid credentials.");

            return await tokenIssuer.IssueAsync(user, req.IpAddress, ct);
        }
    }
}
