using Application.Common.Interfaces;
using Domain.Common;
using FluentValidation;
using MediatR;

namespace Application.Commands.ChatRooms.AddChatRoom
{
    public class AddChatRoomRequest : IRequest<ApiResult<Guid>>, IUserContext
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public List<KeyValuePair<string, string>>? Metadata { get; set; }

        public List<AddUserToChatRoomDto> RoomMembers { get; set; }
        public Guid UserId { get; set; }
        public Guid WorkspaceId { get; set; }
    }

    public class AddChatRoomRequestValidator : AbstractValidator<AddChatRoomRequest>
    {
        public AddChatRoomRequestValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty();
            RuleFor(x => x.WorkspaceId).NotEmpty();
        }
    }
}
