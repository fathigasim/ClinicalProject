using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication
{
    public record AuthTokenPair(
      string AccessToken,
      string RefreshToken,
      DateTime RefreshTokenExpires,
      DateTime AccessTokenExpires);

}

