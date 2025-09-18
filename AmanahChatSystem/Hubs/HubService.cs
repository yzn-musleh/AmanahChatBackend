using Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ChatSystem.UI
{
    public class ChatHubService : IChatHubService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatHubService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        /// <summary>
        /// Send a message to all members of a specific chat room
        /// </summary>
        public async Task SendMessageToRoom(Guid chatRoomId, object message)
        {
            Console.WriteLine("message recieved");
            await _hubContext.Clients.Group(chatRoomId.ToString())
                .SendAsync("ReceiveMessage", message);
        }

        /// <summary>
        /// Send a typing indicator to a specific chat room
        /// </summary>
        public async Task SendTypingIndicator(Guid chatRoomId, string userName, bool isTyping)
        {
            await _hubContext.Clients.Group(chatRoomId.ToString())
                .SendAsync("TypingIndicator", new { UserName = userName, IsTyping = isTyping });
        }

        /// <summary>
        /// Notify room members when a user joins
        /// </summary>
        public async Task NotifyUserJoined(Guid chatRoomId, string userName)
        {
            await _hubContext.Clients.Group(chatRoomId.ToString())
                .SendAsync("UserJoined", new { UserName = userName, Timestamp = DateTimeOffset.UtcNow });
        }

        /// <summary>
        /// Notify room members when a user leaves
        /// </summary>
        public async Task NotifyUserLeft(Guid chatRoomId, string userName)
        {
            await _hubContext.Clients.Group(chatRoomId.ToString())
                .SendAsync("UserLeft", new { UserName = userName, Timestamp = DateTimeOffset.UtcNow });
        }

        /// <summary>
        /// Send message read receipt to the sender
        /// </summary>
        public async Task SendMessageReadReceipt(Guid chatRoomId, Guid messageId, string readerName)
        {
            await _hubContext.Clients.Group(chatRoomId.ToString())
                .SendAsync("MessageRead", new { MessageId = messageId, ReaderName = readerName, Timestamp = DateTimeOffset.UtcNow });
        }
    }
}
