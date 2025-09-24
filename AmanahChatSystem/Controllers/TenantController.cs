using Application.Commands.AccessKeys.AddAccessKey;
using Application.Commands.Tenants.CreateTenant;
using Microsoft.AspNetCore.Mvc;

namespace ChatSystem.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : BaseController
    {
        public TenantController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenant(CreateTenantRequest request,
         CancellationToken cancellationToken = default)
        {
            var result = await _Mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("issueKey")]
        public async Task<IActionResult> IssueApiKey (AddAccessKeyRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await _Mediator.Send(request, cancellationToken);
            return Ok(result);
        }
    }
}
