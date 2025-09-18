using Application.Common.Interfaces;
using Domain.Common;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ChatRooms.RemoveRoomMember
{
    public class RemoveRoomMemberRequestHandler
        (IApplicationDbContext _applicationDbContext,
        IChatHubService _chatHubService) : IRequestHandler<RemoveRoomMemberRequest, ApiResult<Guid>>
    {
        public async Task<ApiResult<Guid>> Handle(RemoveRoomMemberRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<Guid>();
            try
            {

                var chatRoomExists = await _applicationDbContext.ChatRooms
                   .AnyAsync(cr => cr.Id == request.ChatRoomId, cancellationToken);

                if (!chatRoomExists)
                {
                    return result.CreateError(ErrorCodeEnum.NotFound, "Chat room not found");
                }

                //Verify room member exists
                var userExists = await _applicationDbContext.Users
                    .AnyAsync(u => u.Id == request.UserId, cancellationToken);

                if (!userExists)
                {
                    return result.CreateError(ErrorCodeEnum.NotFound, "User not found");
                }

                // Check if user is already a member
                var existingMember = await _applicationDbContext.RoomMembers
                    .FirstOrDefaultAsync(rm => rm.ChatRoomId == request.ChatRoomId && rm.UserId == request.UserId, cancellationToken);

                if (existingMember == null)
                {
                    return result.CreateError(ErrorCodeEnum.NotFound, "User is not a member of this chat room");
                }

                existingMember.IsDeleted = true;

                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return result.CreateSuccess(existingMember.Id);
            }
            catch (Exception ex)
            {
                return result.CreateError(ErrorCodeEnum.DatabaseError, ex.Message);
            }
        }
    }
}
