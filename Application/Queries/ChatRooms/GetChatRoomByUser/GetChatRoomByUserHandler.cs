using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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

namespace Application.Queries.ChatRooms.GetChatRoomByUser
{
    public class GetChatRoomByUserHandler
    : IRequestHandler<GetChatRoomByUserQuery, ApiResult<List<GetChatRoomDto>>>
    {
        private readonly IApplicationDbContext _context;

        public GetChatRoomByUserHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<List<GetChatRoomDto>>> Handle(GetChatRoomByUserQuery request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<List<GetChatRoomDto>>();

            try
            {
                var isUserExists = await _context.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken);
                if (!isUserExists)
                    return result.CreateError(ErrorCodeEnum.NotFound, "User was not found");

                var chatRooms = await _context.RoomMembers
                    .Where(rm => rm.UserId == request.UserId)
                    .Select(rm => new GetChatRoomDto
                    {
                        RoomMemberId = rm.Id,
                        ChatRoomId = rm.ChatRoom.Id,

                        // For direct chats: set title as the other user's name
                        Title = rm.ChatRoom.Type == ChatRoomType.Direct
                            ? rm.ChatRoom.RoomMembers
                                .Where(m => m.UserId != request.UserId)
                                .Select(m => m.User.FirstName + " " + (m.User.LastName ?? ""))
                                .FirstOrDefault()
                            : rm.ChatRoom.Title,

                        Description = rm.ChatRoom.Type == ChatRoomType.Group
                            ? rm.ChatRoom.Description
                            : null,

                        TotalMembers = rm.ChatRoom.RoomMembers.Count(),

                        TotalMessages = -1,

                        LastActionDate = rm.ChatRoom.LastActionDate,
                        IsAdmin = rm.IsAdmin
                    })
                    .OrderByDescending(r => r.LastActionDate)
                    .ToListAsync(cancellationToken);

                return result.CreateSuccess(chatRooms);
            }
            catch (Exception ex)
            {
                return result.CreateError(ErrorCodeEnum.DatabaseError, ex.Message);
            }
        }
    }

}
