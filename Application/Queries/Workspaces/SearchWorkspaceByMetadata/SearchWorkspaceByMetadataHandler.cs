using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Queries.Workspaces.SearchWorkspaceByMetadata
{
    public class SearchWorkspaceByMetadataHandler(
        IApplicationDbContext _applicationDbContext,
        IMapper _mapper)
        : IRequestHandler<SearchWorkspaceByMetadataRequest, ApiResult<Guid>>
    {
        public async Task<ApiResult<Guid>> Handle(SearchWorkspaceByMetadataRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<Guid>();

            var chatRoom = _mapper.Map<ChatRoom>(request);
            //_applicationDbContext.ChatRooms.Add(chatRoom);

            //await _applicationDbContext.SaveChangesAsync(new CancellationToken());

            return await Task.FromResult(result.CreateSuccess());
        }
    }
}
