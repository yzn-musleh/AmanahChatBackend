using Application.Commands.ChatRooms.AddDirectChat;
using Microsoft.AspNetCore.Mvc;

namespace ChatSystem.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectChatController : BaseController
    {
        public DirectChatController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpPost("direct")]
        public async Task<ActionResult<Guid>> CreateOrGetDirect([FromBody] AddDirectChatRequest request)
        {

            var result = await _Mediator.Send(request);
            return Ok(result);
        }
    }
}
