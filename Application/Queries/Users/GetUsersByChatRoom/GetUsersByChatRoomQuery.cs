using Application.Common.Interfaces;
using Domain.Common;
using MediatR;

namespace Application.Queries.Users.GetUsersByChatRoom
{
    public class GetUsersByChatRoomQuery : IRequest<ApiResult<List<GetUserDto>>>, IUserContext
    {
        public Guid ChatRoomId { get; set; }
        public Guid UserId { get; set; }
        public Guid WorkspaceId { get; set; }
    }
}
