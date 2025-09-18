using Domain.Common;
using MediatR;

namespace Application.Queries.ChatRooms.GetChatRoomsByWorkspace
{
    public class GetChatRoomByWorkspaceHandler : IRequestHandler<GetChatRoomByWorkspaceQuery, ApiResult<List<GetChatRoomDto>>>
    {

        public Task<ApiResult<List<GetChatRoomDto>>> Handle(GetChatRoomByWorkspaceQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
