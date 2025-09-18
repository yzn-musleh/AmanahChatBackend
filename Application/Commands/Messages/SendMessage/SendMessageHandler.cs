using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Messages.SendMessage
{
    public class SendMessageHandler(
        IApplicationDbContext _applicationDbContext,
        IMapper _mapper,
        IChatHubService _chatHubService)
        : IRequestHandler<SendMessageRequest, ApiResult<Guid>>
    {
        public async Task<ApiResult<Guid>> Handle(SendMessageRequest request, CancellationToken cancellationToken)
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

                // Create the chat message
                var chatMessage = new ChatMessage
                {
                    Id = Guid.NewGuid(),
                    RoomMemberId = request.RoomMemberId,
                    Message = request.Message,
                    FilePath = request.FilePath,
                    IsRead = false,
                    LastActionDate = DateTimeOffset.UtcNow
                };

                chatRoom.LastActionDate = DateTimeOffset.UtcNow;
                // Save to database
                _applicationDbContext.ChatMessages.Add(chatMessage);
                _applicationDbContext.ChatRooms.Update(chatRoom);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                await _chatHubService.SendMessageToRoom(request.ChatRoomId, new
                {
                    MessageId = chatMessage.Id,
                    RoomMemberId = request.RoomMemberId,
                    SenderName = roomMember.User?.FirstName + " " + roomMember.User?.LastName,
                    Message = request.Message,
                    FilePath = request.FilePath,
                    Timestamp = chatMessage.LastActionDate,
                    ChatRoomId = request.ChatRoomId
                });

                return result.CreateSuccess(chatMessage.Id);
            }
            catch (Exception ex)
            {
                var fullError = GetFullExceptionMessage(ex);
                return result.CreateError(ErrorCodeEnum.DatabaseError, $"Error sending message: {ex.Message}: {fullError}");
            }
        }

        private static string GetFullExceptionMessage(Exception ex)
        {
            var messages = new List<string>();
            var currentEx = ex;

            while (currentEx != null)
            {
                messages.Add($"{currentEx.GetType().Name}: {currentEx.Message}");
                currentEx = currentEx.InnerException;
            }

            return string.Join(" -> ", messages);
        }
    }
}

