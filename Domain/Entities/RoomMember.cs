using Domain.Common;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Domain.Entities
{
    //messageRecipent(id, messageId, recipentUserId, status)
    public class RoomMember : BaseEntity
    {
        public Guid ChatRoomId { get; set; }

        public Guid UserId { get; set; }

        public bool IsAdmin { get; set; }


        public ChatRoom ChatRoom { get; set; }

        public User User { get; set; }
    }
}
