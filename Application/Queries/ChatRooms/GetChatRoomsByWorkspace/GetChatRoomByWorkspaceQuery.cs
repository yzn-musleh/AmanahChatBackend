using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.ChatRooms.GetChatRoomsByWorkspace
{
    public class GetChatRoomByWorkspaceQuery : IRequest<ApiResult<List<GetChatRoomDto>>>
    {
        public Guid WorkspaceId { get; set; }
    }
}
