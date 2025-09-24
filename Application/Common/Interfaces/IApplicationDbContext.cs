using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<AccessKeys> AccessKeys { get; }
        DbSet<Tenant> Tenant { get; }
        DbSet<Workspace> Workspaces { get; }

        DbSet<ChatMessage> ChatMessages { get; }

        DbSet<ChatMessageReply> ChatMessageReplies { get; }

        DbSet<ChatRoom> ChatRooms { get; }

        DbSet<RoomMember> RoomMembers { get; }

        DbSet<User> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task<List<T>> ReadFromSqlAsync<T>(string sql, Dictionary<string, object>? parameters = null);

        Task<int> ExecuteSqlRawAsync(string sql, Dictionary<string, object>? parameters = null);

        T UpdateEntity<T>(T entity) where T : notnull;

        Task<int> ExecuteDeleteAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
    }
}
