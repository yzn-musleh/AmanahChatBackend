using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.EntityFramework.Configurations
{
    public class ChatMessageReplyConfiguration : IEntityTypeConfiguration<ChatMessageReply>
    {
        public void Configure(EntityTypeBuilder<ChatMessageReply> configuration)
        {
            configuration
                .Property(o => o.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            configuration
                .HasOne(x => x.RoomMember)
                .WithMany()
                .HasForeignKey(x => x.RoomMemberId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            configuration
                .HasOne(x => x.ChatMessage)
                .WithMany(x=> x.ChatMessageReplies)
                .HasForeignKey(x => x.ChatMessageId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            //configuration
            //      .OwnsOne(x => x.Metadata, builder => { builder.ToJson(); });

            configuration.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
