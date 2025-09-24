using Application.Common;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Workspaces.AddWorkspace
{
    public class AddWorkspaceHandler(
       IApplicationDbContext _applicationDbContext,
       IMapper _mapper, TenantContext _tenantContext)
       : IRequestHandler<AddWorkspaceRequest, ApiResult<Guid>>
    {
        public async Task<ApiResult<Guid>> Handle(AddWorkspaceRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<Guid>();

            var workspace = _mapper.Map<Workspace>(request);
            workspace.TenantId = _tenantContext.TenantId;

            _applicationDbContext.Workspaces.Add(workspace);

            await _applicationDbContext.SaveChangesAsync(new CancellationToken());

            return await Task.FromResult(result.CreateSuccess(workspace.Id));
        }
    }
}
