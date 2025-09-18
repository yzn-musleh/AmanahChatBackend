using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.ChatRooms
{
    public class GetChatRoomDto
    {
        public Guid RoomMemberId { get; set; }
        public Guid ChatRoomId { get; set; }
        public string? Title { get; set; }
        public int TotalMembers { get; set; }
        public int TotalMessages { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset LastActionDate { get; set; }

        public bool IsAdmin { get; set; }
    }

}
