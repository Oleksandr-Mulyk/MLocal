using System.ComponentModel;
using System.Linq.Expressions;

namespace Local.Web.Data.User
{
    public interface IUserRepository
    {
        IQueryable<ApplicationUser> GetAll(
            IEnumerable<Expression<Func<ApplicationUser, bool>>> filterExpressions = null,
            Expression<Func<ApplicationUser, string?>> sortExpression = null,
            ListSortDirection sortDirection = ListSortDirection.Ascending
            );

        Task<ApplicationUser> GetByIdAsync(string id);

        Task<ApplicationUser> GetByUserNameAsync(string userName);

        Task<ApplicationUser> CreateAsync(ApplicationUser user, string password);

        Task<ApplicationUser> UpdateAsync(ApplicationUser user, string? password = null);

        Task DeleteAsync(ApplicationUser user);
    }
}
