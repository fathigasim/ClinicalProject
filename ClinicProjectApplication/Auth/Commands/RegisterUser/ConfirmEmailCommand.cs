using ClinicProjectApplication.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.RegisterUser
{
    public record ConfirmEmailCommand(string Email, string Token) : IRequest<Result<string>>;
}
