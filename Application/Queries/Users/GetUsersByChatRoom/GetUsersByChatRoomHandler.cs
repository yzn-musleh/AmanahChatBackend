using Application.Common.Interfaces;
using Application.Queries.Messages.GetMessages;
using AutoMapper;
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

namespace Application.Queries.Users.GetUsersByChatRoom
{
    public class GetUsersByChatRoomHandler(IApplicationDbContext _applicationDbContext, IMapper _mapper) : IRequestHandler<GetUsersByChatRoomQuery, ApiResult<List<GetUserDto>>>
    {
        public async Task<ApiResult<List<GetUserDto>>> Handle(GetUsersByChatRoomQuery request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<List<GetUserDto>>();
            try
            {
                // Verify chat room exists
                var chatRoomExists = await _applicationDbContext.ChatRooms
                    .AnyAsync(cr => cr.Id == request.ChatRoomId, cancellationToken);

                if (!chatRoomExists)
                {
                    return result.CreateError(ErrorCodeEnum.NotFound, "Chat room not found");
                }

                // Build query for users
                var roomMembers = await _applicationDbContext.RoomMembers
                    .Include(rm => rm.User) // ensure User is loaded
                    .Where(rm => rm.ChatRoomId == request.ChatRoomId && rm.UserId != request.UserId)
                    .ToListAsync();

                var users = _mapper.Map<List<GetUserDto>>(roomMembers);

                return result.CreateSuccess(users);
            }
            catch (Exception ex)
            {
                return result.CreateError(ErrorCodeEnum.DatabaseError, $"Error retrieving messages: {ex.Message}");
            }
        }
    }
}
