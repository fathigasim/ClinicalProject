using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands
{
    public record AuthResponse(string AccessToken, string RefreshToken,DateTime RefreshTokenExpires);
  
}
