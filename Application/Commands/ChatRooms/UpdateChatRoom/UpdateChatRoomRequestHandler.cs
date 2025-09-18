using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ChatRooms.UpdateChatRoom
{
    public class UpdateChatRoomRequestHandler(
        IApplicationDbContext _applicationDbContext,
        IMapper _mapper,
        IChatHubService _chatHubService) : IRequestHandler<UpdateChatRoomRequest, ApiResult<Guid>>
    {
        public async Task<ApiResult<Guid>> Handle(UpdateChatRoomRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<Guid>();

            try
            {

                var chatRoom = _applicationDbContext.ChatRooms.FirstOrDefault(ch => ch.Id == request.ChatRoomId);

                if (chatRoom == null)
                    return result.CreateError(ErrorCodeEnum.NotFound, "Chat room was not found");

                chatRoom.Title = request.Title;
                chatRoom.Description = request.Description;
                chatRoom.LastActionDate = DateTime.UtcNow;

                _applicationDbContext.ChatRooms.Update(chatRoom);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return result.CreateSuccess(chatRoom.Id);
            }
            catch (Exception ex)
            {
                return result.CreateError(ErrorCodeEnum.DatabaseError, ex.Message);
            }
        }
    }
}
