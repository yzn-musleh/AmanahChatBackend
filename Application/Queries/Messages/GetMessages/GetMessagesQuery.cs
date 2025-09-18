using Domain.Common;
using MediatR;

namespace Application.Queries.Messages.GetMessages
{
    public class GetMessagesQuery : IRequest<ApiResult<List<MessageDto>>>
    {
        public Guid ChatRoomId { get; set; }
        
        public DateTime? FromDate { get; set; }
        
        public DateTime? ToDate { get; set; }
    }
}

