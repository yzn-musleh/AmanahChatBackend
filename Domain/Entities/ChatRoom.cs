using Domain.Common;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Domain.Entities
{

    public enum ChatRoomType
    {
        Direct = 1,
        Group = 2
    }

    public class ChatRoom : BaseEntity
    {
        public string? Title { get; set; } 

        public string? Description { get; set; }
        public ChatRoomType Type { get; set; }

        // For direct chat, store a combined key (smallerGuid_biggerGuid)
        public string? UniqueDirectKey { get; set; }

        public List<RoomMember> RoomMembers { get; set; }

        public Guid WorkspaceId { get; set; }
    }
}
