using Domain.Common;

namespace Domain.Entities
{
    public class DirectChat : BaseEntity
    {
        public Guid UserId1 { get; set; }
        public Guid UserId2 { get; set; }
        public Guid WorkspaceId { get; set; }
        public string UserPairKey { get; set; } = null!;

        // Navigation
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public User User1 { get; set; } = null!;
        public User User2 { get; set; } = null!;
    }
}
