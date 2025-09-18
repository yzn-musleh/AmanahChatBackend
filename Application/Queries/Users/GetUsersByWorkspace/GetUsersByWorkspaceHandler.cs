using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users.GetUsersByWorkspace
{
    public class GetUsersByWorkspaceHandler
        (IApplicationDbContext _applicationDbContext, IMapper _mapper) : IRequestHandler<GetUsersByWorkspaceQuery, ApiResult<List<GetUserDto>>>
    {
        public async Task<ApiResult<List<GetUserDto>>> Handle(GetUsersByWorkspaceQuery request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<List<GetUserDto>>();

            try
            {
                // Verify chat workspace exists TODO
                var workspaceExists = await _applicationDbContext.Workspaces
                    .AnyAsync(w => w.Id == request.WorkspaceId, cancellationToken);


                // Build query for users
                var query = await _applicationDbContext.Users
                    .Where(rm => rm.WorkspaceId == request.WorkspaceId && rm.Id != request.UserId)
                    .ToListAsync();

                var users = _mapper.Map<List<GetUserDto>>(query);

                return result.CreateSuccess(users);
            }
            catch (Exception ex)
            {
                return result.CreateError(ErrorCodeEnum.DatabaseError, $"Error retrieving messages: {ex.Message}");
            }
        }
    }

}
