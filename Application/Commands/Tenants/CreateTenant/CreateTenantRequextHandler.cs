
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Tenants.CreateTenant
{
    public class CreateTenantRequextHandler(
        IApplicationDbContext _applicationDbContext,
        IMapper _mapper) : IRequestHandler<CreateTenantRequest, ApiResult<string>>
    {
        async Task<ApiResult<string>> IRequestHandler<CreateTenantRequest, ApiResult<string>>.Handle(CreateTenantRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<string>();

            if (_applicationDbContext.Tenant.Any(t => t.Name == request.Name))
            {
                return result.CreateError(Domain.Enums.ErrorCodeEnum.DuplicateEntry, "Another tenant with the same name or email do exist");
            }

            var tenant = _mapper.Map<Tenant>(request);

            await _applicationDbContext.Tenant.AddAsync(tenant);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return result.CreateSuccess();
        }
    }
}
