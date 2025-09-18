using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Commands.Workspaces.DeleteWorkspace
{
    public class DeleteWorkspaceRequestHandler(
        IApplicationDbContext _applicationDbContext,
        IChatHubService _chatHubService) : IRequestHandler<DeleteWorkspaceRequest, ApiResult<Guid>>
    {
        public async Task<ApiResult<Guid>> Handle(DeleteWorkspaceRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<Guid>();

            try
            {
                var workspace = _applicationDbContext.Workspaces
                    .FirstOrDefault(w => w.Id == request.WorkspaceId);

                if (workspace == null)
                    return result.CreateError(ErrorCodeEnum.NotFound, "Workspace was not found");

                var user = _applicationDbContext.Users
                    .FirstOrDefault(u => u.Id == request.UserId && u.WorkspaceId == request.WorkspaceId);

                if (user == null)
                    return result.CreateError(ErrorCodeEnum.NotFound, "User was not found");

                workspace.IsDeleted = true;
                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return result.CreateSuccess(workspace.Id);
            } catch (Exception ex)
            {
                return result.CreateError(ErrorCodeEnum.DatabaseError, ex.Message);
            }

        }
    }
}
