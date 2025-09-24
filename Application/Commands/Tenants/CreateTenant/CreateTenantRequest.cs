using Domain.Common;
using MediatR;

namespace Application.Commands.Tenants.CreateTenant
{
    public class CreateTenantRequest : IRequest<ApiResult<string>>
    {
        public string Name { get; set; }
        public string Email { get; set; }

    }
}
