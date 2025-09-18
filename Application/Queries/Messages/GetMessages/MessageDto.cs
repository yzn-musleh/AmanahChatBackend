namespace Application.Queries.Messages.GetMessages
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        
        public Guid RoomMemberId { get; set; }
        
        public string? Message { get; set; }
        
        public string? FilePath { get; set; }
        
        public bool IsRead { get; set; }
        
        public DateTimeOffset Timestamp { get; set; }
        
        public string SenderName { get; set; } = string.Empty;
        
        public string? SenderProfileImage { get; set; }
        
        public List<MessageReplyDto> Replies { get; set; } = new();
    }
    
    public class MessageReplyDto
    {
        public Guid Id { get; set; }
        
        public Guid RoomMemberId { get; set; }
        
        public string? Message { get; set; }
        
        public string? FilePath { get; set; }
        
        public DateTimeOffset Timestamp { get; set; }
        
        public string SenderName { get; set; } = string.Empty;
        
        public string? SenderProfileImage { get; set; }
    }
}

