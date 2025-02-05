using System.ComponentModel;
using System.Linq.Expressions;

namespace Local.Web.Data.ToDo
{
    public interface IToDoRepository
    {
        IQueryable<IToDoItem> GetAll(
            IEnumerable<Expression<Func<IToDoItem, bool>>> filterExpressions,
            Expression<Func<IToDoItem, string?>> sortExpression,
            ListSortDirection sortDirection
            );

        IQueryable<IToDoItem> GetAllByUser(
            ApplicationUser user,
            IEnumerable<Expression<Func<IToDoItem, bool>>> filterExpressions,
            Expression<Func<IToDoItem, string?>> sortExpression,
            ListSortDirection sortDirection
            );

        Task<IToDoItem> GetByIdAsync(int id);

        Task<IToDoItem> CreateAsync(IToDoItem toDoItem);

        Task<IToDoItem> UpdateAsync(IToDoItem toDoItem);

        Task DeleteAsync(IToDoItem toDoItem);
    }
}
