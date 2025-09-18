using Application.Common.Interfaces;
using Domain.Common;
using MediatR;

namespace Application.Commands.Messages.BroadcastMessage
{
    public class BroadcastMessageRequest : IRequest<ApiResult<Guid>>, IUserContext
    {
        public Guid UserId { get; set; }
        public Guid WorkspaceId { get; set; }

        public string Message { get; set; }

        public List<Guid> RecipientsId { get; set; }
    }
}
