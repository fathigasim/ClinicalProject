using ClinicProjectApplication.Commands.RevokeTokens;
using ClinicProjectApplication.Common.Exceptions;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;

using MediatR;


namespace ClinicProjectApplication.Auth.Commands.RevokeTokens
{
    public class RevokeAllTokensHandler(IUserRepository userRepository)
    : IRequestHandler<RevokeAllTokensCommand>
    {
        public async Task Handle(RevokeAllTokensCommand req, CancellationToken ct)
        {
            var user = await userRepository.GetByIdAsync(req.UserId,ct)
                ?? throw new NotFoundException(nameof(ApplicationUser), req.UserId);

            foreach (var token in user.RefreshTokens.Where(t => t.IsActive).ToList())
                token.Revoke(req.IpAddress);

            await userRepository.UpdateAsync(user, ct);
        }
    }
}
