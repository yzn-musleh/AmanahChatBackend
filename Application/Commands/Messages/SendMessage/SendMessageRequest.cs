using Domain.Common;
using MediatR;

namespace Application.Commands.Messages.SendMessage
{
    public class SendMessageRequest : IRequest<ApiResult<Guid>>
    {
        public Guid RoomMemberId { get; set; }
        
        public string? Message { get; set; }

        public DateTimeOffset LastActionDate { get; set; }


        public string? FilePath { get; set; }
        
        public Guid ChatRoomId { get; set; }
    }
}

