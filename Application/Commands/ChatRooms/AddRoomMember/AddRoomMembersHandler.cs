using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.ChatRooms.AddRoomMember
{
    public class AddRoomMembersHandler(IApplicationDbContext _applicationDbContext)
        : IRequestHandler<AddRoomMembersRequest, ApiResult<List<Guid>>>
    {
        public async Task<ApiResult<List<Guid>>> Handle(AddRoomMembersRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<List<Guid>>();

            try
            {
                // Verify chat room exists
                var chatRoomExists = await _applicationDbContext.ChatRooms
                    .AnyAsync(cr => cr.Id == request.ChatRoomId, cancellationToken);

                if (!chatRoomExists)
                {
                    return result.CreateError(ErrorCodeEnum.NotFound, "Chat room not found");
                }

                // Verify user exists
                // Extract user IDs from request
                var userIds = request.RoomMemberDto.Select(rm => rm.UserId).ToList();

                // Step 1: Verify all user IDs exist
                var existingUserIds = await _applicationDbContext.Users
                    .Where(u => userIds.Contains(u.Id))
                    .Select(u => u.Id)
                    .ToListAsync(cancellationToken);

                var missingUserIds = userIds.Except(existingUserIds).ToList();

                if (missingUserIds.Any())
                {
                    return result.CreateError(ErrorCodeEnum.NotFound, $"The following user IDs were not found: {string.Join(", ", missingUserIds)}");
                }

                // Step 2: Check if any of the users are already members
                var alreadyMemberIds = await _applicationDbContext.RoomMembers
                    .Where(rm => rm.ChatRoomId == request.ChatRoomId && userIds.Contains(rm.UserId))
                    .Select(rm => rm.UserId)
                    .ToListAsync(cancellationToken);

                if (alreadyMemberIds.Any())
                {
                    return result.CreateError(ErrorCodeEnum.DuplicateEntry,
                        $"The following users are already members of this chat room: {string.Join(", ", alreadyMemberIds)}");
                }

                // Step 3: Create room members with per-user admin flag
                var newRoomMembers = request.RoomMemberDto
                    .Where(dto => !alreadyMemberIds.Contains(dto.UserId))
                    .Select(dto => new RoomMember
                    {
                        Id = Guid.NewGuid(),
                        ChatRoomId = request.ChatRoomId,
                        UserId = dto.UserId,
                        IsAdmin = dto.isAdmin,
                        LastActionDate = DateTimeOffset.UtcNow
                    })
                    .ToList();

                _applicationDbContext.RoomMembers.AddRange(newRoomMembers);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return result.CreateSuccess(newRoomMembers.Select(rm => rm.Id).ToList());


            }
            catch (Exception ex)
            {
                // Get full exception details including inner exceptions
                var fullErrorMessage = GetFullExceptionMessage(ex);
                return result.CreateError(ErrorCodeEnum.DatabaseError, $"Error adding room member: {fullErrorMessage}");
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

