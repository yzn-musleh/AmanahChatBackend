namespace Domain.Entities
{
    public class Tenant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        // Relationships
        public List<Workspace> Workspaces { get; set; }
        public List<AccessKeys> AccessKeys { get; set; }
    }
}
