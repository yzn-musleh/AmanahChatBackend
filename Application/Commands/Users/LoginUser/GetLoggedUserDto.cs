namespace Application.Commands.Users.LoginUser
{
    public class GetLoggedUserDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid WorkspaceId { get; set; }
        public bool isAdmin { get; set; }
        public string jwt { get; set; }
    }
}
