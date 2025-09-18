using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ChatRooms.RemoveRoomMember
{
    public class RemoveRoomMemberRequest : IRequest<ApiResult<Guid>>
    {
        public Guid UserId { get; set; }
        public Guid ChatRoomId { get; set; }
    }
}
