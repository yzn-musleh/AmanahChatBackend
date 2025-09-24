using Domain.Common;

namespace Domain.Entities
{
    //application(id, theme, logo , app-key, firebase file, List<account>)
    //account(id, applicationId, referenceNumber, List<user>)
    public class Workspace : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        // Relationship
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
