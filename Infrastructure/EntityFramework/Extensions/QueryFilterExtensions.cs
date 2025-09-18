using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace Infrastructure.EntityFramework.Extensions
{
    public static class QueryFilterExtensions
    {
        public static void ApplyGlobalIsDeletedFilter(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetProperty("IsDeleted") != null)
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, "IsDeleted");
                    var constant = Expression.Constant(false);
                    var equal = Expression.Equal(property, constant);
                    var lambda = Expression.Lambda(equal, parameter);

                    // Get the generic Entity<T> method
                    var entityMethod = typeof(ModelBuilder)
                        .GetMethod(nameof(ModelBuilder.Entity), Type.EmptyTypes)
                        .MakeGenericMethod(entityType.ClrType);
                    var entityBuilder = entityMethod.Invoke(modelBuilder, null);

                    // Construct the Expression<Func<T, bool>> type
                    var funcType = typeof(Func<,>).MakeGenericType(entityType.ClrType, typeof(bool));
                    var expressionType = typeof(Expression<>).MakeGenericType(funcType);

                    // Get the HasQueryFilter method with the correct parameter type
                    var hasQueryFilterMethod = entityBuilder.GetType()
                        .GetMethod(nameof(EntityTypeBuilder.HasQueryFilter), new[] { expressionType });

                    if (hasQueryFilterMethod != null)
                    {
                        hasQueryFilterMethod.Invoke(entityBuilder, new[] { lambda });
                    }
                }
            }
        }
    }
}
