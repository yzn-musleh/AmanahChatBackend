using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Workspaces.AddWorkspace
{
    public class AddWorkspaceHandler(
       IApplicationDbContext _applicationDbContext,
       IMapper _mapper)
       : IRequestHandler<AddWorkspaceRequest, ApiResult<Guid>>
    {
        public async Task<ApiResult<Guid>> Handle(AddWorkspaceRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<Guid>();

            var workspace = _mapper.Map<Workspace>(request);
            _applicationDbContext.Workspaces.Add(workspace);

            await _applicationDbContext.SaveChangesAsync(new CancellationToken());

            return await Task.FromResult(result.CreateSuccess(workspace.Id));
        }
    }
}
