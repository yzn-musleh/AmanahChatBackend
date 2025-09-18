namespace Application.Common.Interfaces
{
    public interface IChatHubService
    {
        /// <summary>
        /// Send a message to all members of a specific chat room
        /// </summary>
        Task SendMessageToRoom(Guid chatRoomId, object message);
        
        /// <summary>
        /// Send a typing indicator to a specific chat room
        /// </summary>
        Task SendTypingIndicator(Guid chatRoomId, string userName, bool isTyping);
        
        /// <summary>
        /// Notify room members when a user joins
        /// </summary>
        Task NotifyUserJoined(Guid chatRoomId, string userName);
        
        /// <summary>
        /// Notify room members when a user leaves
        /// </summary>
        Task NotifyUserLeft(Guid chatRoomId, string userName);
        
        /// <summary>
        /// Send message read receipt to the sender
        /// </summary>
        Task SendMessageReadReceipt(Guid chatRoomId, Guid messageId, string readerName);
    }
}
