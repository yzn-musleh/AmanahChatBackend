using Application.Commands.Messages.DeleteMessage;
using Application.Commands.Messages.SendMessage;
using Application.Commands.Messages.UpdateMessage;
using Application.Queries.Messages.GetMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatSystem.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : BaseController
    {
        public MessagesController(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
        }

        /// <summary>
        /// Send a message to a chat room
        /// </summary>
        [HttpPost("Send")]
        public async Task<IActionResult> SendMessageAsync([FromBody] SendMessageRequest request,
           CancellationToken cancellationToken)
        {
            var result = await _Mediator.Send(request, cancellationToken);
            return CreateResponseResult(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateMessageAsync([FromBody] UpdateMessageRequest request
            , CancellationToken cancellationToken)
        {
            var result = await _Mediator.Send(request, cancellationToken);
            return CreateResponseResult(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteMessageAsync([FromBody] DeleteMessageRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _Mediator.Send(request, cancellationToken);
            return CreateResponseResult(result);
        }

        /// <summary>
        /// Get messages from a chat room with pagination
        /// </summary>
        [HttpGet("RoomMessages/{chatRoomId}")]
        public async Task<IActionResult> GetMessagesAsync(
            Guid chatRoomId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = new GetMessagesQuery
            {
                ChatRoomId = chatRoomId,
                FromDate = fromDate,
                ToDate = toDate
            };

            var result = await _Mediator.Send(query, cancellationToken);
            return CreateResponseResult(result);
        }
    }
}

