using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.EntityFramework.Configurations
{
    public class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
    {
        public void Configure(EntityTypeBuilder<Workspace> configuration)
        {
            configuration
                .Property(o => o.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            //configuration
            //      .OwnsOne(x => x.Metadata, builder => { builder.ToJson(); });

            configuration.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
