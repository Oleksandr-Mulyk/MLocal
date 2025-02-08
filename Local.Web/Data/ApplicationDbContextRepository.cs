using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Local.Web.Data
{
    public abstract class ApplicationDbContextRepository<T, TClass, TId>(
            DbSet<TClass> dbSet,
            ApplicationDbContext applicationDbContext
            ) : IRepository<T, TId> where T : class where TClass : class, T
    {
        public virtual IQueryable<T> GetAll() => dbSet.AsQueryable().Select(item => (T)item);

        public virtual IQueryable<T> GetAll(IEnumerable<Expression<Func<T, bool>>> filterExpressions) =>
            filterExpressions.Aggregate(GetAll(), (current, filter) => current.Where(filter));

        public virtual IQueryable<T> GetAll(
            Expression<Func<T, string?>> sortExpression,
            ListSortDirection sortDirection = ListSortDirection.Ascending
            )
            => ApplySort(GetAll(), sortExpression, sortDirection);

        public virtual IQueryable<T> GetAll(
            IEnumerable<Expression<Func<T, bool>>> filterExpressions,
            Expression<Func<T, string?>> sortExpression,
            ListSortDirection sortDirection = ListSortDirection.Ascending
            ) =>
            ApplySort(GetAll(filterExpressions), sortExpression, sortDirection);

        public virtual async Task<T> GetByIdAsync(TId id) =>
            await dbSet.FindAsync(id) ?? throw new Exception($"{typeof(TClass).Name} not found");

        public virtual async Task<T> CreateAsync(T entity)
        {
            var result = await dbSet.AddAsync(entity as TClass ?? throw new InvalidCastException());

            if (result.State == EntityState.Added)
            {
                await applicationDbContext.SaveChangesAsync();
                return result.Entity;
            }

            throw new Exception($"{typeof(TClass).Name} not created");
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            var result = dbSet.Update(entity as TClass ?? throw new InvalidCastException());

            if (result.State == EntityState.Modified)
            {
                await applicationDbContext.SaveChangesAsync();
                return result.Entity;
            }

            throw new Exception($"{typeof(TClass).Name} not updated");
        }

        public virtual async Task DeleteAsync(T entity)
        {
            var result = dbSet.Remove(entity as TClass ?? throw new InvalidCastException());

            if (result.State == EntityState.Deleted)
            {
                await applicationDbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"{typeof(TClass).Name} Item not deleted");
            }
        }

        private static IQueryable<T> ApplySort(
            IQueryable<T> query,
            Expression<Func<T, string?>> sortExpression,
            ListSortDirection sortDirection
            )
        {
            return sortDirection == ListSortDirection.Ascending ?
                query.OrderBy(sortExpression) :
                query.OrderByDescending(sortExpression);
        }
    }

    public abstract class ApplicationDbContextRepository<T, TClass>(
            DbSet<TClass> dbSet,
            ApplicationDbContext applicationDbContext
            ) : ApplicationDbContextRepository<T, TClass, int>(dbSet, applicationDbContext)
        where T : class where TClass : class, T
    {
    }
}
