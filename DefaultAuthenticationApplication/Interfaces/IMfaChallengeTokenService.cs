using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Interfaces
{
    public interface IMfaChallengeTokenService
    {
        
        string GenerateChallengeToken(string userId);
        string? ValidateAndGetUserId(string token); // returns null if invalid/expired
    }
}
