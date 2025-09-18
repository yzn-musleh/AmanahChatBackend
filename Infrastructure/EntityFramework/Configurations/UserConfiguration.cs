using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.EntityFramework.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> configuration)
        {
            configuration
                .Property(o => o.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            // Configure the relationship with Workspace
            configuration
                .HasOne<Workspace>()
                .WithMany()
                .HasForeignKey(x => x.WorkspaceId)
                .OnDelete(DeleteBehavior.Restrict);

            //configuration
            //     .OwnsOne(x => x.Metadata, builder => { builder.ToJson(); });

            configuration.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
