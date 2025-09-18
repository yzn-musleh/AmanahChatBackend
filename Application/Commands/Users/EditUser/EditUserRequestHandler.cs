using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Commands.Users.EditUser
{
    public class EditUserRequestHandler(
        IApplicationDbContext _applicationDbContext,
        IMapper _mapper,
        IChatHubService _chatHubService) : IRequestHandler<EditUserRequest, ApiResult<Guid>>
    {
        public async Task<ApiResult<Guid>> Handle(EditUserRequest request, CancellationToken cancellationToken)
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

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;

                _applicationDbContext.Users.Update(user);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return result.CreateSuccess(user.Id);
            }catch (Exception ex) { 
                return result.CreateError(ErrorCodeEnum.DatabaseError, ex.Message);
            }
        }
    }
}
    