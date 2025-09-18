using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using Infrastructure.EntityFramework.Extensions;
using Infrastructure.Common;

namespace Infrastructure.EntityFramework
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {

        private readonly IUserContextService _userContextService;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUserContextService userContextService)
           : base(options)
        {
            _userContextService = userContextService;
        }


        public DbSet<Workspace> Workspaces => Set<Workspace>();

        public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

        public DbSet<ChatMessageReply> ChatMessageReplies => Set<ChatMessageReply>();

        public DbSet<ChatRoom> ChatRooms => Set<ChatRoom>();

        public DbSet<RoomMember> RoomMembers => Set<RoomMember>();

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            AddSeedData(modelBuilder);

            modelBuilder.ApplyGlobalIsDeletedFilter();
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var auditedCreatedEntries = ChangeTracker.Entries<BaseEntity>().ToArray();
            foreach (var entity in auditedCreatedEntries)
            {
                entity.Entity.LastActionDate = DateTime.UtcNow;
                entity.Property(nameof(BaseEntity.LastActionDate)).IsModified = true;
            }

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var auditedCreatedEntries = ChangeTracker.Entries<BaseEntity>().ToArray();
            var currentUserId = _userContextService.GetCurrentUserId();
            foreach (var entity in auditedCreatedEntries)
            {
                entity.Entity.LastActionDate = DateTime.UtcNow;
                entity.Entity.ActionBy = currentUserId;
                entity.Property(nameof(BaseEntity.LastActionDate)).IsModified = true;
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        private static void AddSeedData(ModelBuilder modelBuilder)
        {
        }

        //private static void AddGlobalFilters(ModelBuilder modelBuilder)
        //{
        //    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        //    {
        //        if (entityType.FindProperty("IsDeleted") == null)
        //        {
        //            continue;
        //        }
        //        // 1. Add the IsDeleted property
        //        // entityType.("IsDeleted", typeof(bool));

        //        // 2. Create the query filter
        //        var parameter = Expression.Parameter(entityType.ClrType);

        //        // EF.Property<bool>(post, "IsDeleted")
        //        var propertyMethodInfo = typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(bool));
        //        var isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant("IsDeleted"));

        //        // EF.Property<bool>(post, "IsDeleted") == false
        //        BinaryExpression compareExpression = Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));

        //        // post => EF.Property<bool>(post, "IsDeleted") == false
        //        var lambda = Expression.Lambda(compareExpression, parameter);

        //        modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        //    }
        //}

        


        public async Task<List<T>> ReadFromSqlAsync<T>(string sql, Dictionary<string, object>? parameters = null)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    sqlParameters.Add(new SqlParameter(item.Key, item.Value));
                }
            }

            IQueryable<T> result;
            if (sqlParameters.Any())
                result = Database.SqlQueryRaw<T>(sql);
            else
                result = Database.SqlQueryRaw<T>(sql, sqlParameters);

            return await result.ToListAsync();
        }

        public virtual T UpdateEntity<T>(T entity) where T : notnull
        {
            var modelState = Entry(entity).State;
            if (modelState != EntityState.Detached)
                Entry(entity).State = EntityState.Detached;

            Entry(entity).State = EntityState.Modified;
            //Update(entity);

            return entity;
        }

        public virtual async Task<int> ExecuteDeleteAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var deleted = await Set<T>()
                .Where(predicate)
                .ExecuteDeleteAsync();

            return deleted;
        }

        public async Task<int> ExecuteSqlRawAsync(string sql, Dictionary<string, object>? parameters = null)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    sqlParameters.Add(new SqlParameter(item.Key, item.Value));
                }
            }

            int result;
            if (sqlParameters.Any())
                result = await Database.ExecuteSqlRawAsync(sql);
            else
                result = await Database.ExecuteSqlRawAsync(sql, sqlParameters);

            return result;
        }
    }
}
