using Domain.Common;

namespace Domain.Entities
{
    //userDevice(id, userId, fcmToken, deviceType)
    //user(id, name, email, username, hashedPassword, tags"metaData", referenceNumber, logo, status)
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string? Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public string? ProfileImageUrl { get; set; }

        public string? ConnectId { get; set; } = null!;

        public Guid WorkspaceId { get; set; }
    }
}
