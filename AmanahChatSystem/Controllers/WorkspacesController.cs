using Application.Commands.Workspaces.AddWorkspace;
using Application.Commands.Workspaces.DeleteWorkspace;
using Application.Common;
using Application.Queries.Workspaces.GetWorkspaces;
using Application.Queries.Workspaces.SearchWorkspaceByMetadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatSystem.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkspacesController : BaseController
    {
        public WorkspacesController(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkspaces( CancellationToken cancellationToken)
        {
            var request = new GetWorkspaceQuery { };
            var result = await _Mediator.Send(request, cancellationToken);
            return CreateResponseResult(result);
        }

        [HttpPost("SearchByMetadata")]
        public async Task<IActionResult> GetByMetadataAsync([FromBody] SearchWorkspaceByMetadataRequest searchWorkspaceByMetadataRequest,
           CancellationToken cancellationToken)
        {
            var result = await _Mediator.Send(searchWorkspaceByMetadataRequest, cancellationToken);
            return CreateResponseResult(result);
        }

        [HttpPost("Add")]
        [AdminOnly]
        public async Task<IActionResult> AddAsync([FromBody] AddWorkspaceRequest addWorkspaceRequest,
           CancellationToken cancellationToken)
        {
            var result = await _Mediator.Send(addWorkspaceRequest, cancellationToken);
            return CreateResponseResult(result);
        }
        
        //[HttpPut("Edit")]
        //public async Task<IActionResult> EditAsync([FromForm] AddWorkspaceRequest MedicalTestPackage,
        //   CancellationToken cancellationToken)
        //{
        //   return CreateResponseResult(await _Mediator.Send(MedicalTestPackage, cancellationToken));
        //}

       [HttpDelete("Delete")]
       [AdminOnly]
        public async Task<IActionResult> DeleteAsync(
           [FromBody] DeleteWorkspaceRequest request,
            CancellationToken cancellationToken)
        {
            return CreateResponseResult(await _Mediator.Send(request, cancellationToken));
        }
    }
}
