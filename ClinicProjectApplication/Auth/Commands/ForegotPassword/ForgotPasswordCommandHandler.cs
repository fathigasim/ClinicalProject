using ClinicProjectApplication.Common;
using ClinicProjectDomain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.ForegotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IPublisher _publish;
        public ForgotPasswordCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration
            , IPublisher publish)
        {
           _userManager = userManager;   
            _configuration = configuration;
            _publish = publish;
        }
        public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.email);
            if (user == null)
            {
                // Don't reveal that the user doesn't exist
                return Result<string>.Success("If the email exists, a password reset link has been sent.");
                //{
                //    Success = true,
                //    Message = "If the email exists, a password reset link has been sent.",
                //    Data = true
                //};
            }
            var token =await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var resetLink = $"{_configuration["Frontend:BaseUrl"]}/auth/reset-password?email={user.Email}&token={encodedToken}";
            var subject = "Reset Your Password";

            var message = $@"
    <h2>Password Reset Request</h2>
    <p>Hello {user.UserName},</p>
    <p>You requested to reset your password.</p>
    <p>Click the button below to reset your password:</p>
    
    <p>
        <a href='{resetLink}' 
           style='padding:10px 20px; background-color:#007bff; color:white; text-decoration:none; border-radius:5px;'>
           Reset Password
        </a>
    </p>

    <p>If you did not request this, please ignore this email.</p>
    <p>This link will expire soon for security reasons.</p>
";


          await  _publish.Publish(new ForegotPasswordNotification(request.email,subject,message), cancellationToken);
          
            return Result<string>.Success("If the email exists, a password reset link has been sent.");


        }
    }
}
