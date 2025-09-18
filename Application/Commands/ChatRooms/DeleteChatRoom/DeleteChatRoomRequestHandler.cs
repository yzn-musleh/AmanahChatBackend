using Application.Common.Interfaces;
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

namespace Application.Commands.ChatRooms.DeleteChatRoom
{
    public class DeleteChatRoomRequestHandler(
        IApplicationDbContext _applicationDbContext,
        IChatHubService _chatHubService) : IRequestHandler<DeleteChatRoomRequest, ApiResult<Guid>>
    {
        public async Task<ApiResult<Guid>> Handle(DeleteChatRoomRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<Guid>();

            try
            {

                var chatRoom = await _applicationDbContext.ChatRooms
                    .Include(rm => rm.RoomMembers)
                    .FirstOrDefaultAsync(ch => ch.Id == request.ChatRoomId, cancellationToken);

                if (chatRoom == null)
                {
                    return result.CreateError(ErrorCodeEnum.NotFound, "Chat room was not found");
                }

                var roomMember = chatRoom.RoomMembers.FirstOrDefault(rm => rm.Id == request.RoomMemberId && rm.IsAdmin);

                if (roomMember is null)
                {
                    return result.CreateError(ErrorCodeEnum.Unauthorized, "Unauthorized Action");
                }

                chatRoom.IsDeleted = true;

                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return result.CreateSuccess(chatRoom.Id);
            }
            catch (Exception ex) { 
                return result.CreateError(ErrorCodeEnum.DatabaseError, ex.Message);
            }
        }
    }
}
