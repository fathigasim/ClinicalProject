
using ClinicProjectDomain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace  ClinicProjectInfrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
            => _httpContextAccessor = httpContextAccessor;

        private ClaimsPrincipal? User
            => _httpContextAccessor.HttpContext?.User;

        public string? UserId
            => User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public string? Email
            => User?.FindFirstValue(ClaimTypes.Email);

        public bool IsAuthenticated
            => User?.Identity?.IsAuthenticated == true;

        public bool IsInRole(string role)
            => User?.IsInRole(role) == true;
    }
}
