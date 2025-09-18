using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Users.DeleteUser
{
    public class DeleteUserRequestHandler(
        IApplicationDbContext _applicationDbContext,
        IMapper _mapper,
        IChatHubService _chatHubService) : IRequestHandler<DeleteUserRequest, ApiResult<Guid>>
    {
        public async Task<ApiResult<Guid>> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<Guid>();

            try
            {
                // check if user exists 
                // requires more validating
                var user = _applicationDbContext.Users
                    .FirstOrDefault(u => u.Id == request.UserId && !u.IsDeleted);

                if (user == null)
                {
                    return result.CreateError(ErrorCodeEnum.NotFound, "User was not found");
                }

                user.IsDeleted = true;

                _applicationDbContext.Users.Update(user);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return result.CreateSuccess(user.Id);

            }catch (Exception ex)
            {
                return result.CreateError(ErrorCodeEnum.DatabaseError, ex.Message);
            }
        }
    }
}
