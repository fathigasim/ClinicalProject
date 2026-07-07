using ClinicProjectApplication.Auth.Commands.RevokeTokens;
using ClinicProjectApplication.Common.Exceptions;
using ClinicProjectDomain.Interfaces;
using MediatR;


namespace DefaultAuthenticationApplication.Auth.Commands.RevokeTokens
{


    public class RevokeTokenHandler(IUserRepository userRepository)
        : IRequestHandler<RevokeTokenCommand>
    {
        public async Task Handle(RevokeTokenCommand req, CancellationToken ct)
        {
            var user = await userRepository.GetByRefreshTokenAsync(req.RefreshToken, ct)
                ?? throw new UnauthorizedException("Token not found.");

            var token = user.RefreshTokens.Single(t => t.Token == req.RefreshToken);

            if (!token.IsActive)
                throw new UnauthorizedException("Token is already revoked or expired.");

            token.Revoke(req.IpAddress);
            await userRepository.UpdateAsync(user, ct);
        }
    }
}
