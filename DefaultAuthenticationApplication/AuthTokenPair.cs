using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication
{
    public record AuthTokenPair(string accessToken,
           string newRefresh,
            DateTime Expires,
             DateTime DateTime);
    
    }

