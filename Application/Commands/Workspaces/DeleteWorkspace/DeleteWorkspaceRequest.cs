using Application.Common.Interfaces;
using Domain.Common;
using MediatR;

namespace Application.Commands.Workspaces.DeleteWorkspace
{
    public class DeleteWorkspaceRequest : IRequest<ApiResult<Guid>>, IUserContext
    {
        public Guid WorkspaceId { get; set; }
        public Guid UserId { get; set; }

    }
}
