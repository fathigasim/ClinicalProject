
using ClinicProjectApplication.Common;
using MediatR;


namespace ClinicProjectApplication.Auth.Commands.LoginUser
{
    // Application/Auth/Commands/LoginUser/LoginUserCommand.cs
    //public record LoginUserCommand(
    // string Email, string Password,
    // string? IpAddress) : IRequest<AuthTokenPair>;
    public record LoginUserCommand(
    string Email, string Password,
    string? IpAddress) : IRequest<Result<LoginResponse>>;   // was IRequest<AuthTokenPair>


    //public record LoginResponse(
    //bool MfaRequired,
    //string? MfaToken,
    //AuthTokenPair? Tokens);
    public record LoginResponse(bool MfaRequired,string? MfaToken,AuthTokenPair? Tokens);
    //{
    //    public bool MfaRequired { get; init; }
    //    public string? MfaToken { get; init; } // only set when MfaRequired == true
    //    public AuthTokenPair? Tokens { get; init; } // only set when MfaRequired == false
    //}
}
