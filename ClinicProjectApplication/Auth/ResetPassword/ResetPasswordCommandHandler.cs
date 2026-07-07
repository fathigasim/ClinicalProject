using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ResetPasswordCommandHandler> _logger;
        public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager, ILogger<ResetPasswordCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.user);
            if (user == null)
            {
               return Result<string>.Failure("User not exist");
            }
            try
            {
                var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.token));
                var result = await _userManager.ResetPasswordAsync(user,decodedToken, request.newpassord);
                if (result.Succeeded)
                {
                    return Result<string>.Success($"Password for user {request.user} changed successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Checking result exception error{error}", ex.Message);
            }
            return Result<string>.Failure("Error reseting password");
        }
    }
}
