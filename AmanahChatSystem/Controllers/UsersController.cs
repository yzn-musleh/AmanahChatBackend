using Application.Commands.Users.AddUser;
using Application.Commands.Users.AddUserList;
using Application.Commands.Users.DeleteUser;
using Application.Commands.Users.LoginUser;
using Application.Commands.Workspaces.DeleteWorkspace;
using Application.Queries.Messages.GetMessages;
using Application.Queries.Users.GetUsersByChatRoom;
using Application.Queries.Users.GetUsersByWorkspace;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatSystem.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        public UsersController(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
        }

        [HttpGet("GetUsersByWorkspace")]
        public async Task<IActionResult> GetUsersByWorkspace(
            CancellationToken cancellationToken = default)
        {
            var query = new GetUsersByWorkspaceQuery {};

            var result = await _Mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("GetUsersByChatRoom")]
        public async Task<IActionResult> GetUsersByChatRoom(
            Guid chatRoomId,
            CancellationToken cancellationToken = default)
        {
            var query = new GetUsersByChatRoomQuery
            {
                ChatRoomId = chatRoomId
            };

            var result = await _Mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _Mediator.Send(request, cancellationToken);
            return CreateResponseResult(result);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync([FromBody] AddUserRequest addUserRequest,
           CancellationToken cancellationToken)
        {
            var result = await _Mediator.Send(addUserRequest, cancellationToken);
            return CreateResponseResult(result);
        }

        [HttpPost("AddEditList")]
        public async Task<IActionResult> AddEditListAsync([FromBody] AddEditUserListRequest addEditUserListRequest,
            CancellationToken cancellationToken)
        {
            var result = await _Mediator.Send(addEditUserListRequest, cancellationToken);
            return Ok(true);
        }

        //[HttpPost("AddEditList")]
        //public async Task<IActionResult> AddListAsync([FromBody] AddEditUserListRequest addEditUserListRequest,
        //   CancellationToken cancellationToken)
        //{
        //    var result = await _Mediator.Send(addEditUserListRequest, cancellationToken);
        //    return CreateResponseResult(result);
        //}

        [HttpPut("Edit")]
        public async Task<IActionResult> EditAsync([FromForm] AddUserRequest request,
           CancellationToken cancellationToken)
        {
            return CreateResponseResult(await _Mediator.Send(request, cancellationToken));
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(
            [FromQuery] DeleteUserRequest request,
            CancellationToken cancellationToken)
        {
            return CreateResponseResult(await _Mediator.Send( request, cancellationToken));
        }
    }
}
