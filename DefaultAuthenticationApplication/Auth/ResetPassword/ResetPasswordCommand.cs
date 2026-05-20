using ClinicProjectApplication.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.ResetPassword
{
    public record ResetPasswordCommand (string user,string token,string newpassord) : IRequest<Result<string>>
    {
    }
}
