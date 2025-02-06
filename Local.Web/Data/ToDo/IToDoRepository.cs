using System.ComponentModel;
using System.Linq.Expressions;

namespace Local.Web.Data.ToDo
{
    public interface IToDoRepository
    {
        IQueryable<IToDoItem> GetAll(
            IEnumerable<Expression<Func<IToDoItem, bool>>> filterExpressions = null,
            Expression<Func<IToDoItem, string?>> sortExpression = null,
            ListSortDirection sortDirection = ListSortDirection.Ascending
            );

        IQueryable<IToDoItem> GetAllByUser(
            ApplicationUser user,
            IEnumerable<Expression<Func<IToDoItem, bool>>> filterExpressions = null,
            Expression<Func<IToDoItem, string?>> sortExpression = null,
            ListSortDirection sortDirection = ListSortDirection.Ascending
            );

        Task<IToDoItem> GetByIdAsync(int id);

        Task<IToDoItem> CreateAsync(IToDoItem toDoItem);

        Task<IToDoItem> UpdateAsync(IToDoItem toDoItem);

        Task DeleteAsync(IToDoItem toDoItem);
    }
}
