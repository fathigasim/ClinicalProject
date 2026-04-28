
using ClinicProjectApplication.Auth.Vm;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Queries
{
    public record GetCurrentUserQuery : IRequest<UserVm>;
}
