using Application.Commands.Messages.SendMessage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatSystem.UI
{
    [Authorize]
    public class ChatHub : BaseHub
    {
        private readonly IMediator _mediator;

        public ChatHub(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _mediator = serviceProvider.GetRequiredService<IMediator>();
        }

        /// <summary>
        /// Client calls this method to send a message
        /// </summary>
        public async Task SendMessage(Guid roomMemberId, string message, Guid chatRoomId, string? filePath = null)
        {
            var request = new SendMessageRequest
            {
                RoomMemberId = roomMemberId,
                Message = message,
                ChatRoomId = chatRoomId,
                LastActionDate = DateTime.UtcNow,
                FilePath = filePath
            };

            var result = await _mediator.Send(request);
            
            // Send result back to the caller
            await Clients.Caller.SendAsync("MessageSent", new { 
                Success = result.ErrorCode == Domain.Enums.ErrorCodeEnum.None, 
                MessageId = result.Result,
                Error = result.Message 
            });
        }

        /// <summary>
        /// Client calls this method to indicate typing status
        /// </summary>
        public async Task SendTypingIndicator(Guid chatRoomId, string userName, bool isTyping)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId.ToString());
            await Clients.OthersInGroup(chatRoomId.ToString())
                .SendAsync("SendTypingIndicator", new { UserName = userName, IsTyping = isTyping });

            await Console.Out.WriteLineAsync("typing indicator sent from "+userName+" in chat room " + chatRoomId);
        }

        /// <summary>
        /// Enhanced group joining with user notification
        /// </summary>
        public async Task JoinRoom(Guid chatRoomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId.ToString());
            Console.WriteLine("User joined room");
            await Clients.OthersInGroup(chatRoomId.ToString())
                .SendAsync("UserJoined", new {  Timestamp = DateTimeOffset.UtcNow });
        }

        /// <summary>
        /// Enhanced group leaving with user notification
        /// </summary>
        public async Task LeaveRoom(Guid chatRoomId, string userName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId.ToString());
            await Clients.OthersInGroup(chatRoomId.ToString())
                .SendAsync("UserLeft", new { UserName = userName, Timestamp = DateTimeOffset.UtcNow });
        }
    }
}
