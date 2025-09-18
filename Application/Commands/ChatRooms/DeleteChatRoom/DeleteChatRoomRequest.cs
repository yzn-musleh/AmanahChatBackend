using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ChatRooms.DeleteChatRoom
{
    public class DeleteChatRoomRequest : IRequest<ApiResult<Guid>>
    {
        public Guid ChatRoomId { get; set; }
        public Guid RoomMemberId { get; set; }
    }
}
