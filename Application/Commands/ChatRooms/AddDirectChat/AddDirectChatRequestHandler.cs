using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ChatRooms.AddDirectChat
{
   

    public class CreateOrGetDirectChatHandler : IRequestHandler<AddDirectChatRequest, ApiResult<Guid>>
    {
        private readonly IApplicationDbContext _context;

        public CreateOrGetDirectChatHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<Guid>> Handle(AddDirectChatRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<Guid>();

            try
            {

                if (request.UserId == request.UserId2)
                {
                    return result.CreateError(ErrorCodeEnum.ValidationError, "You cannot chat with yourself");
                }

                var user2 = _context.Users
                    .FirstOrDefault(u => u.Id == request.UserId2 && u.WorkspaceId == request.WorkspaceId);

                if (user2 == null)
                    return result.CreateError(ErrorCodeEnum.NotFound, "User Not Found");


                var orderedIds = new[] { request.UserId, request.UserId2 }
                    .OrderBy(id => id)
                    .ToList();
                var key = $"{orderedIds[0]}_{orderedIds[1]}";

                var existingRoom = await _context.ChatRooms
                    .FirstOrDefaultAsync(r => r.Type == ChatRoomType.Direct && r.UniqueDirectKey == key, cancellationToken);

                if (existingRoom != null)
                    return result.CreateSuccess(existingRoom.Id);

                var chatRoom = new ChatRoom
                {
                    Id = Guid.NewGuid(),
                    Type = ChatRoomType.Direct,
                    UniqueDirectKey = key,
                    WorkspaceId = request.WorkspaceId,
                    LastActionDate = DateTimeOffset.UtcNow,
                    RoomMembers = new List<RoomMember>
                {
                    new RoomMember { Id = Guid.NewGuid(), UserId = orderedIds[0] },
                    new RoomMember { Id = Guid.NewGuid(), UserId = orderedIds[1] }
                }
                };

                _context.ChatRooms.Add(chatRoom);
                await _context.SaveChangesAsync(cancellationToken);

                return result.CreateSuccess(chatRoom.Id);
            }
            catch (Exception ex)
            {
                return result.CreateError(ErrorCodeEnum.DatabaseError, $"Error creating direct chat: {ex.Message},,, {ex.InnerException}");
            }
        }
    }

}
