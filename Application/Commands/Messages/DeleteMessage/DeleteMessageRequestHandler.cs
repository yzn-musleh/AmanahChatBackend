using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Messages.DeleteMessage
{
    public class DeleteMessageRequestHandler(
        IApplicationDbContext _applicationDbContext,
        IChatHubService _chatHubService) : IRequestHandler<DeleteMessageRequest, ApiResult<Guid>>
    {
        public async Task<ApiResult<Guid>> Handle(DeleteMessageRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<Guid>();

            try
            {
                // Validate that the room member exists and belongs to the chat room
                var roomMember = await _applicationDbContext.RoomMembers
                    .Include(rm => rm.User)
                    .FirstOrDefaultAsync(rm => rm.Id == request.RoomMemberId, cancellationToken);

                if (roomMember == null)
                {
                    return result.CreateError(ErrorCodeEnum.NotFound, "Room member not found");
                }

                // Verify the room member belongs to the specified chat room
                var chatRoom = await _applicationDbContext.ChatRooms
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

                var message = await _applicationDbContext.ChatMessages.FirstOrDefaultAsync(m => m.Id == request.MessageId);
                if (message == null)
                    return result.CreateError(ErrorCodeEnum.NotFound, "Message not found");

                if (message.RoomMemberId != request.RoomMemberId)
                {
                    return result.CreateError(ErrorCodeEnum.Unauthorized, "User is not the message sender");
                }

                message.IsDeleted = true;
                message.LastActionDate = DateTime.UtcNow;

                _applicationDbContext.ChatMessages.Update(message);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);
                return result.CreateSuccess(message.Id);
            }
            catch (Exception ex)
            {
                return result.CreateError(ErrorCodeEnum.DatabaseError, $"Error updating message: {ex.Message}");
            }
        }
    }
}
