using Application.Commands.Messages.SendMessage;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Messages.UpdateMessage
{
    public class UpdateMessageHandler(
        IApplicationDbContext _context,
        IMapper _mapper,
        IChatHubService _chatHub) : IRequestHandler<UpdateMessageRequest, ApiResult<Guid>>
    {

        public async Task<ApiResult<Guid>> Handle(UpdateMessageRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<Guid>();

            try
            {
                // Validate that the room member exists and belongs to the chat room
                var roomMember = await _context.RoomMembers
                    .Include(rm => rm.User)
                    .FirstOrDefaultAsync(rm => rm.Id == request.RoomMemberId, cancellationToken);

                if (roomMember == null)
                {
                    return result.CreateError(ErrorCodeEnum.NotFound, "Room member not found");
                }

                // Verify the room member belongs to the specified chat room
                var chatRoom = await _context.ChatRooms
                    .Include(cr => cr.RoomMembers)
                    .FirstOrDefaultAsync(cr => cr.Id == request.ChatRoomId, cancellationToken);

                if (chatRoom == null)
                {
                    return result.CreateError(ErrorCodeEnum.NotFound, "Chat room not found");
                }

                if (!chatRoom.RoomMembers.Any(rm => rm.Id == request.RoomMemberId))
                {
                    return result.CreateError(ErrorCodeEnum.Unauthorized, "User is not a member of this chat room");
                }

                // Verify that the message exists and belongs to the chat and room member

                var message = await _context.ChatMessages.FirstOrDefaultAsync(m => m.Id == request.MessageId);
                if (message == null)
                    return result.CreateError(ErrorCodeEnum.NotFound, "Message not found");

                if (message.RoomMemberId != request.RoomMemberId)
                {
                    return result.CreateError(ErrorCodeEnum.Unauthorized, "User is not the message sender");
                }

                message.Message = request.newMessage;
                message.LastActionDate = DateTime.UtcNow;

                _context.ChatMessages.Update(message);
                await _context.SaveChangesAsync(cancellationToken);
                return result.CreateSuccess(message.Id);
            }
            catch (Exception ex)
            {
                return result.CreateError(ErrorCodeEnum.DatabaseError, $"Error updating message: {ex.Message}");
            }

        }
    }
}
