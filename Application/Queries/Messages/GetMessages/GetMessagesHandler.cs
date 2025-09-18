using Application.Common.Interfaces;
using Domain.Common;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Messages.GetMessages
{
    public class GetMessagesHandler(IApplicationDbContext _applicationDbContext)
        : IRequestHandler<GetMessagesQuery, ApiResult<List<MessageDto>>>
    {
        public async Task<ApiResult<List<MessageDto>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<List<MessageDto>>();

            try
            {
                // Verify chat room exists
                var chatRoomExists = await _applicationDbContext.ChatRooms
                    .AnyAsync(cr => cr.Id == request.ChatRoomId, cancellationToken);

                if (!chatRoomExists)
                {
                    return result.CreateError(ErrorCodeEnum.NotFound, "Chat room not found");
                }

                // Build query for messages
                // Build query for messages
                var query = _applicationDbContext.ChatMessages.Where(m => !m.IsDeleted)
      .Include(cm => cm.RoomMember)
          .ThenInclude(rm => rm.User)
      .Include(cm => cm.ChatMessageReplies)
          .ThenInclude(cmr => cmr.RoomMember)
              .ThenInclude(rm => rm.User)
      .Where(cm => _applicationDbContext.RoomMembers
          .IgnoreQueryFilters() // Bypass global IsDeleted filter
          .Any(rm => rm.Id == cm.RoomMemberId &&
                    rm.ChatRoomId == request.ChatRoomId) );

                // Apply date filters if provided
                if (request.FromDate.HasValue)
                {
                    query = query.Where(cm => cm.LastActionDate >= request.FromDate.Value);
                }

                if (request.ToDate.HasValue)
                {
                    query = query.Where(cm => cm.LastActionDate <= request.ToDate.Value);
                }

                // Apply pagination and ordering
                var messages = await query
                    .OrderByDescending(cm => cm.LastActionDate)
                    .Select(cm => new MessageDto
                    {
                        Id = cm.Id,
                        RoomMemberId = cm.RoomMemberId,
                        Message = cm.Message,
                        FilePath = cm.FilePath,
                        IsRead = cm.IsRead,
                        Timestamp = cm.LastActionDate,
                        SenderName = (cm.RoomMember.User!.FirstName ?? "") + " " + (cm.RoomMember.User.LastName ?? ""),
                        SenderProfileImage = cm.RoomMember.User.ProfileImageUrl,
                        Replies = cm.ChatMessageReplies.Select(cmr => new MessageReplyDto
                        {
                            Id = cmr.Id,
                            RoomMemberId = cmr.RoomMemberId,
                            Message = cmr.Message,
                            FilePath = cmr.FilePath,
                            Timestamp = cmr.LastActionDate,
                            SenderName = (cmr.RoomMember.User!.FirstName ?? "") + " " + (cmr.RoomMember.User.LastName ?? ""),
                            SenderProfileImage = cmr.RoomMember.User.ProfileImageUrl
                        }).OrderBy(r => r.Timestamp).ToList()
                    })
                    .ToListAsync(cancellationToken);

                // Reverse to get chronological order (oldest first)
                messages.Reverse();

                return result.CreateSuccess(messages);
            }
            catch (Exception ex)
            {
                return result.CreateError(ErrorCodeEnum.DatabaseError, $"Error retrieving messages: {ex.Message}");
            }
        }
    }
}

