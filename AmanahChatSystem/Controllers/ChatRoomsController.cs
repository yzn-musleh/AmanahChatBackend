using Application.Commands.ChatRooms.AddChatRoom;
using Application.Commands.ChatRooms.AddRoomMember;
using Application.Commands.ChatRooms.DeleteChatRoom;
using Application.Commands.ChatRooms.RemoveRoomMember;
using Application.Commands.ChatRooms.UpdateChatRoom;
using Application.Queries.ChatRooms.GetChatRoomByUser;
using Application.Queries.ChatRooms.GetChatRoomsByWorkspace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatSystem.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatRoomsController : BaseController
    {
        public ChatRoomsController(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
        }

        [HttpGet("GetChatRoomsByWorkspace")]
        public async Task<IActionResult> GetChatRoomsByWorkspace(CancellationToken cancellationToken = default)
        {

            var query = new GetChatRoomByWorkspaceQuery();

            var result = await _Mediator.Send(query, cancellationToken);
            return CreateResponseResult(result);
        }

        [HttpGet("GetChatRoomsByUser")]
        public async Task<IActionResult> GetChatRoomsByUser(CancellationToken cancellationToken = default)
        {
            var query = new GetChatRoomByUserQuery();

            var result = await _Mediator.Send(query, cancellationToken);
            return CreateResponseResult(result);
        }
            
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync([FromBody] AddChatRoomRequest addChatRoomRequest,
           CancellationToken cancellationToken)
        {
            var result = await _Mediator.Send(addChatRoomRequest, cancellationToken);
            return CreateResponseResult(result);
        }


        [HttpPut("Update")]
        public async Task<IActionResult> UpdateChatRoom(
            [FromBody] UpdateChatRoomRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _Mediator.Send(request, cancellationToken);
            return CreateResponseResult(result);
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteChatRoom([FromQuery] DeleteChatRoomRequest request, CancellationToken cancellationToken)
        {
            var result = await _Mediator.Send(request, cancellationToken);
            return Ok(result);
        }


        // room members operations

        [HttpPost("Members")]
        public async Task<IActionResult> AddMemberAsync(
            [FromBody] AddRoomMembersRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _Mediator.Send(request, cancellationToken);
            return CreateResponseResult(result);
        }


        [HttpDelete("RemoveMembers")]
        public async Task<IActionResult> AddMemberAsync(
           [FromBody] RemoveRoomMemberRequest request,
           CancellationToken cancellationToken)
        {
            var result = await _Mediator.Send(request, cancellationToken);
            return CreateResponseResult(result);
        }

        //[HttpPut("Edit")]
        //public async Task<IActionResult> EditAsync([FromForm] AddChatRoomRequest MedicalTestPackage,
        //   CancellationToken cancellationToken)
        //{
        //    return CreateResponseResult(await Mediator.Send(MedicalTestPackage, cancellationToken));
        //}

        //[HttpDelete("Delete")]
        //public async Task<IActionResult> DeleteAsync(
        //    [FromQuery] long id,
        //    CancellationToken cancellationToken)
        //{
        //    return CreateResponseResult(await Mediator.Send(new DeleteLabPackageRequestDTO { Id = id }, cancellationToken));
        //}
    }
}
