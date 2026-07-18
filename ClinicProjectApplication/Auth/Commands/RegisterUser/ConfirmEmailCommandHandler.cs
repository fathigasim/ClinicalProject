using ClinicProjectApplication.Common;
using ClinicProjectApplication.Common.Exceptions;
using ClinicProjectApplication.Exceptions;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.RegisterUser
{
    

    public class ConfirmEmailHandler(
        UserManager<ApplicationUser> userManager,
        IUserRepository userRepository,
        TokenIssuer tokenIssuer)
        : IRequestHandler<ConfirmEmailCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ConfirmEmailCommand req, CancellationToken ct)
        {
            var user = await userManager.FindByEmailAsync(req.Email)
                ?? throw new NotFoundException(nameof(ApplicationUser), req.Email);

            // Decode token
            var decodedToken = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(req.Token));

            var result = await userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
            //throw new ApiValidationException("Confirmation failed",
            //    result.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));
            {
                return Result<string>.Failure("Confirmation failed");
            }
      
            return Result<string>.Success("Email confirmed successfully");
        }
    }
}
