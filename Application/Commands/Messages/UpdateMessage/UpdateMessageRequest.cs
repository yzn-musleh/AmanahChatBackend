using Domain.Common;
using MediatR;

namespace Application.Commands.Messages.UpdateMessage
{
    public class UpdateMessageRequest : IRequest<ApiResult<Guid>>
    {
        public Guid MessageId { get; set; }
        public string newMessage { get; set; }
        public Guid RoomMemberId { get; set; }
        public Guid ChatRoomId { get; set; }
    }
}
