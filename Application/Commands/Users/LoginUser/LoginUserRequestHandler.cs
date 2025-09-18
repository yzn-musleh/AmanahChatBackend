using Application.Common.Interfaces;
using Domain.Common;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Users.LoginUser
{
    public class LoginUserRequestHandler
        (IApplicationDbContext _applicationDbContext,
        IJwtWebTokenGenerator _jwtGenerator) : IRequestHandler<LoginUserRequest, ApiResult<GetLoggedUserDto>>
    {

        public async Task<ApiResult<GetLoggedUserDto>> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<GetLoggedUserDto>();

            try
            {
                var user = await _applicationDbContext.Users
                    .FirstOrDefaultAsync(u => u.Username == request.UserName && u.Password == request.Password);

                if (user == null)
                {
                    return result.CreateError(ErrorCodeEnum.NotFound, "User was not found");
                }

                var jwt = _jwtGenerator.GenerateToken(user.Id, user.WorkspaceId, user.Username);

                var loggedUser = new GetLoggedUserDto
                {
                    UserId = user.Id,
                    UserName = user.Username,
                    WorkspaceId = user.WorkspaceId,
                    isAdmin = user.IsAdmin,
                    jwt = jwt
                };


                return result.CreateSuccess(loggedUser);
            }
            catch (Exception ex)
            {
                return result.CreateError(ErrorCodeEnum.DatabaseError, ex.Message);
            }
        }
    }
}
