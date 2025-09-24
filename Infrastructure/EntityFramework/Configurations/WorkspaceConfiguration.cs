using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System.Reflection.Emit;

namespace Infrastructure.EntityFramework.Configurations
{
    public class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
    {
        public void Configure(EntityTypeBuilder<Workspace> configuration)
        {
            configuration
                .Property(o => o.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

           configuration
                .HasOne(w => w.Tenant)
                .WithMany(t => t.Workspaces)
                .HasForeignKey(w => w.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            //configuration
            //      .OwnsOne(x => x.Metadata, builder => { builder.ToJson(); });

            configuration.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
