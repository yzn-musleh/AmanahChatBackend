using Domain.Entities.Base;

namespace Domain.Entities
{
    public class ChatMessageReply : MessageBase
    {
        public Guid ChatMessageId { get; set; }

        public ChatMessage ChatMessage { get; set; } = new();
    }
}
