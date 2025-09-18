using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ChatRooms.UpdateChatRoom
{
    public class UpdateChatRoomRequest : IRequest<ApiResult<Guid>>
    {
        public Guid ChatRoomId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
