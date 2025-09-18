namespace Application.Commands.ChatRooms.AddChatRoom
{
    public class AddUserToChatRoomDto
    {
        public Guid UserId { get; set; }

        public bool IsAdmin { get; set; } = false;
    }
}