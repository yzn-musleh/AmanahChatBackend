using Domain.Entities.Base;

namespace Domain.Entities
{
    //message(id, senderUserId, List<messageReciepant>, content, status )
    public class ChatMessage : MessageBase
    {
        public List<ChatMessageReply> ChatMessageReplies { get; set; } 
    }
}
