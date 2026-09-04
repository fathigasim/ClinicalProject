
using ClinicProjectApplication.Auth.Commands.RegisterUser;
using ClinicProjectApplication.Common.Exceptions;
using ClinicProjectApplication.Exceptions;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace ClinicProjectApplication.RegisterUser
{
   


        public class RegisterUserHandler(
     UserManager<ApplicationUser> userManager,
     IUserRepository userRepository,
     TokenIssuer tokenIssuer,
     IPublisher publisher, IMessagePublisher _messagePublisher)
     : IRequestHandler<RegisterUserCommand, string>
        {

        public async Task<string> Handle(RegisterUserCommand req, CancellationToken ct)
        {
            if (await userManager.FindByEmailAsync(req.Email) != null)
                throw new ApiConflictException($"Email '{req.Email}' is already registered.");

            var user = new ApplicationUser { Email = req.Email, UserName = req.Email };
            var result = await userManager.CreateAsync(user, req.Password);

            if (!result.Succeeded)
                throw new ApiValidationException("Validation",
                    result.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));

            await userManager.AddToRoleAsync(user, "User");

            // Generate confirmation token
            var confirmToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

            // Publish notification (your email handler picks this up)
           // await publisher.Publish(new RegisterNotification(req.Email, confirmToken), ct);
            await _messagePublisher.PublishAsync(new RegisterNotification(req.Email, confirmToken), "register.notification", ct);
            // Don't issue auth tokens yet — user must confirm email first
            return "Registration successful. Please check your email to confirm your account.";
        }

    }
    }

