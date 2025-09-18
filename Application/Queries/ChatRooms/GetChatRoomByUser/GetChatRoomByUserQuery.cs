using Application.Common.Interfaces;
using Domain.Common;
using MediatR;

namespace Application.Queries.ChatRooms.GetChatRoomByUser
{
    public class GetChatRoomByUserQuery : IRequest<ApiResult<List<GetChatRoomDto>>>, IUserContext
    {
        public Guid UserId { get; set; }
        public Guid WorkspaceId { get; set; }
    }
}
