using ClinicProjectApplication.Auth.Commands.Mfa.Commands;
using ClinicProjectApplication.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.Mfa.Queries
{
    public record GetMfaStatusQuery(string UserId) : IRequest<Result<MfaStatusResponse>>;
}
