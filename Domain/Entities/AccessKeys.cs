namespace Domain.Entities
{
    public class AccessKeys
    {
        public Guid Id { get; set; }  
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        public string KeyId { get; set; } = null!;  
        public string KeyHash { get; set; } = null!; 
        public byte[] Salt { get; set; } = null!; 


        // Relationship
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
