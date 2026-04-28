
using MediatR;


namespace ClinicProjectApplication.Auth.Commands.LoginUser
{
    // Application/Auth/Commands/LoginUser/LoginUserCommand.cs
    public record LoginUserCommand(
     string Email, string Password,
     string? IpAddress) : IRequest<AuthTokenPair>;




}
