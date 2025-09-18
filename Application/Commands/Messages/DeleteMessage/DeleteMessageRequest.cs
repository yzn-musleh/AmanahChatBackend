using Domain.Common;
using MediatR;

namespace Application.Commands.Messages.DeleteMessage
{
    public class DeleteMessageRequest : IRequest<ApiResult<Guid>>
    {
        public Guid MessageId { get; set; }
        public Guid RoomMemberId { get; set; }
        public Guid ChatRoomId { get; set; }
    }
}
