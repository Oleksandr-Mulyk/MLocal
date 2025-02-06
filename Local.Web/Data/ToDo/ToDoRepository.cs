using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Local.Web.Data.ToDo
{
    public class ToDoRepository(ApplicationDbContext applicationDbContext) : IToDoRepository
    {
        public IQueryable<IToDoItem> GetAll(
            IEnumerable<Expression<Func<IToDoItem, bool>>>? filterExpressions = null,
            Expression<Func<IToDoItem, string?>>? sortExpression = null,
            ListSortDirection sortDirection = ListSortDirection.Ascending
            )
        {
            var toDoItems = applicationDbContext.ToDoItems
                .Include(item => item.CreatedBy)
                .Include(item => item.AssignedTo)
                .Include(item => item.VisibleFor)
                .AsQueryable()
                .Select(item => (IToDoItem)item);

            if (filterExpressions is not null)
            {
                toDoItems = filterExpressions.Aggregate(
                    toDoItems,
                    (current, filter) => current.Where(filter)
                    );
            }

            if (sortExpression is not null)
            {
                toDoItems = (
                    sortDirection == ListSortDirection.Ascending ?
                    toDoItems.OrderBy(sortExpression) :
                    toDoItems.OrderByDescending(sortExpression)
                    );
            }

            return toDoItems;
        }

        public IQueryable<IToDoItem> GetAllByUser(
            ApplicationUser user,
            IEnumerable<Expression<Func<IToDoItem, bool>>>? filterExpressions = null,
            Expression<Func<IToDoItem, string?>>? sortExpression = null,
            ListSortDirection sortDirection = ListSortDirection.Ascending
            )
        {
            var userFilter = (Expression<Func<IToDoItem, bool>>)(
                item => item.CreatedBy == user || item.AssignedTo.Contains(user) || item.VisibleFor.Contains(user)
                );

            var updatedFilterExpressions = filterExpressions is null ?
                new List<Expression<Func<IToDoItem, bool>>> { userFilter } :
                [.. filterExpressions];
            updatedFilterExpressions.Add(userFilter);

            return GetAll(updatedFilterExpressions, sortExpression, sortDirection);
        }

        public async Task<IToDoItem> GetByIdAsync(int id) =>
            await applicationDbContext.ToDoItems.FindAsync(id) ?? throw new Exception("ToDoItem not found");

        public async Task<IToDoItem> CreateAsync(IToDoItem toDoItem)
        {
            var result = await applicationDbContext.ToDoItems
                .AddAsync(toDoItem as ToDoItem ?? throw new InvalidCastException());
            
            if (result.State == EntityState.Added)
            {
                await applicationDbContext.SaveChangesAsync();
                return result.Entity;
            }

            throw new Exception("ToDo Item not created");
        }

        public async Task<IToDoItem> UpdateAsync(IToDoItem toDoItem)
        {
            var result = applicationDbContext.ToDoItems
                .Update(toDoItem as ToDoItem ?? throw new InvalidCastException());

            if (result.State == EntityState.Modified)
            {
                await applicationDbContext.SaveChangesAsync();
                return result.Entity;
            }

            throw new Exception("ToDo Item not updated");
        }

        public async Task DeleteAsync(IToDoItem toDoItem)
        {
            var result = applicationDbContext.ToDoItems
                .Remove(toDoItem as ToDoItem ?? throw new InvalidCastException());

            if (result.State == EntityState.Deleted)
            {
                await applicationDbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("ToDo Item not deleted");
            }
        }
    }
}
