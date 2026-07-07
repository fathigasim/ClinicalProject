using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.Mfa.Commands
{
    public record MfaSetupResponse(string SharedKey, string AuthenticatorUri);


}
