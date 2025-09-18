using Domain.Common;

namespace Domain.Entities.Base
{
    public abstract class MessageBase : BaseEntity
    {
        public Guid RoomMemberId { get; set; }

        public string? Message { get; set; }

        public string? FilePath { get; set; }

        public bool IsRead { get; set; } = false;

        public RoomMember RoomMember { get; set; }  
    }
}
