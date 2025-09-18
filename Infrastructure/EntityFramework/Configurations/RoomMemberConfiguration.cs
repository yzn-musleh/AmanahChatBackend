using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.EntityFramework.Configurations
{
    public class RoomMemberConfiguration : IEntityTypeConfiguration<RoomMember>
    {
        public void Configure(EntityTypeBuilder<RoomMember> configuration)
        {
            configuration
                .Property(o => o.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            configuration
                .HasOne(x => x.ChatRoom)
                .WithMany(x=> x.RoomMembers)
                .HasForeignKey(x => x.ChatRoomId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            configuration
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            //configuration
            //      .OwnsOne(x => x.Metadata, builder => { builder.ToJson(); });\


            configuration.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
