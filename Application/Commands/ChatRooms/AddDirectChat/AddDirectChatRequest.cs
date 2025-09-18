using Application.Common.Interfaces;
using Domain.Common;
using MediatR;

namespace Application.Commands.ChatRooms.AddDirectChat
{
    public class AddDirectChatRequest : IRequest<ApiResult<Guid>>, IUserContext
    {
        public Guid UserId { get; set; }
        public Guid UserId2 { get; set; }
        public Guid WorkspaceId { get; set; }
    }
}
