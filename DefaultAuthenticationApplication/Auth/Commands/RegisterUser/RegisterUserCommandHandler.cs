
using ClinicProjectApplication.Auth.Commands.RegisterUser;
using ClinicProjectApplication.Common.Exceptions;
using ClinicProjectApplication.Exceptions;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace ClinicProjectApplication.RegisterUser
{
   


        public class RegisterUserHandler(
     UserManager<ApplicationUser> userManager,
     IUserRepository userRepository,
     TokenIssuer tokenIssuer)
     : IRequestHandler<RegisterUserCommand, AuthTokenPair>
        {
            public async Task<AuthTokenPair> Handle(RegisterUserCommand req, CancellationToken ct)
            {
                if (await userManager.FindByEmailAsync(req.Email) != null)
                    throw new ApiConflictException($"Email '{req.Email}' is already registered.");

                var user = new ApplicationUser { Email = req.Email, UserName = req.Email };

                var result = await userManager.CreateAsync(user, req.Password);
                if (!result.Succeeded)
                    throw new ApiValidationException("Validation",
                        result.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));

                await userManager.AddToRoleAsync(user, "User");

                var tracked = await userRepository.GetByEmailAsync(req.Email, ct)
                    ?? throw new NotFoundException(nameof(ApplicationUser), req.Email);

                return await tokenIssuer.IssueAsync(tracked, req.IpAddress, ct);
            }

        }
    }

