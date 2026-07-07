using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Auth.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<string>>
    {
       private readonly  IUserManagerService _userManager;
        public DeleteUserCommandHandler(IUserManagerService userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
          var user=  await  _userManager.FindByEmailAsync(request.Email, cancellationToken);
            if (user == null) {
             return   Result<string>.Failure($"Error deleting User {request.Email} ");
            }
             await   _userManager.DeleteUserAsync(user);
            return Result<string>.Success($"User {request.Email} deleted successfully");
            
        }
    }
}
