using Application.Common.Interfaces;
using Domain.Common;
using MediatR;

namespace Application.Commands.ChatRooms.AddRoomMember
{
    public class AddRoomMembersRequest : IRequest<ApiResult<List<Guid>>>, IUserContext
    {
        public Guid ChatRoomId { get; set; }

        public List<AddRoomMemberDto> RoomMemberDto { get; set; }

        public Guid UserId { get; set; }
        public Guid WorkspaceId { get; set; }
    }

    public class AddRoomMemberDto
    {
        public Guid UserId { get; set; }
        public bool isAdmin { get; set; }
    }
}

