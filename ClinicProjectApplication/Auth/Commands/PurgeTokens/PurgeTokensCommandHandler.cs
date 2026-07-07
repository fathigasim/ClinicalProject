
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.PurgeTokens
{
    

    public class PurgeExpiredTokensHandler(IUserRepository repo)
        : IRequestHandler<PurgeExpiredTokensCommand>
    {
        public async Task Handle(PurgeExpiredTokensCommand _, CancellationToken ct)
            => await repo.PurgeExpiredTokensAsync(ct);
    }
}
