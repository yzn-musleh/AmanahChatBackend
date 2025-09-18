using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.ChatRooms.AddChatRoom
{
    public class AddChatRoomHandler(
        IApplicationDbContext _applicationDbContext,
        IMapper _mapper)
        : IRequestHandler<AddChatRoomRequest, ApiResult<Guid>>
    {
        public async Task<ApiResult<Guid>> Handle(AddChatRoomRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<Guid>();

            try
            {
                // Verify workspace exists
                // TODO CHECK IF DELETED
                var workspaceExists = await _applicationDbContext.Workspaces
                      .AnyAsync(w => w.Id == request.WorkspaceId, cancellationToken);

                //if (!workspaceExists)
                //{
                //    return result.CreateError(ErrorCodeEnum.NotFound, "Workspace not found");
                //}

                // Create the chat room
                var chatRoom = new ChatRoom
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    Description = request.Description,
                    WorkspaceId = request.WorkspaceId,
                    LastActionDate = DateTimeOffset.UtcNow,
                    RoomMembers = new List<RoomMember>()
                };

                // Add the creator as admin
                RoomMember adminMember = new RoomMember
                {
                    Id = Guid.NewGuid(),
                    ChatRoomId = chatRoom.Id,
                    UserId = request.UserId,
                    IsAdmin = true,
                };

                chatRoom.RoomMembers.Add(adminMember);

                // Add other members if provided
                if (request.RoomMembers != null)
                {
                    foreach (var roomMemberDto in request.RoomMembers)
                    {
                        var member = new RoomMember
                        {
                            Id = Guid.NewGuid(),
                            ChatRoomId = chatRoom.Id,
                            UserId = roomMemberDto.UserId,
                            IsAdmin = false,
                        };

                        chatRoom.RoomMembers.Add(member);
                    }
                }

                _applicationDbContext.ChatRooms.Add(chatRoom);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return result.CreateSuccess(chatRoom.Id);
            }
            catch (Exception ex)
            {
                return result.CreateError(ErrorCodeEnum.DatabaseError, $"Error creating chat room: {ex.Message}");
            }
        }
    }
}
