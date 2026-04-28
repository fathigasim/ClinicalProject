using ClinicProjectApplication.Auth.Vm;
using ClinicProjectDomain.Interfaces;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Queries
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserVm>
    {
        private readonly ICurrentUserService _currentUserService;
        public GetCurrentUserQueryHandler(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }
        async Task<UserVm> IRequestHandler<GetCurrentUserQuery, UserVm>.Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user =  _currentUserService.Email;
            return new UserVm(user);
        }
    }
}
