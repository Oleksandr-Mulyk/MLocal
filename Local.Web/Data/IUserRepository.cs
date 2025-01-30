using System.ComponentModel;
using System.Linq.Expressions;

namespace Local.Web.Data
{
    public interface IUserRepository
    {
        IQueryable<ApplicationUser> GetAll(
            IEnumerable<Expression<Func<ApplicationUser, bool>>> filterExpressions,
            Expression<Func<ApplicationUser, string?>> sortExpression,
            ListSortDirection sortDirection
            );

        Task<ApplicationUser> GetByIdAsync(string id);

        Task<ApplicationUser> CreateAsync(ApplicationUser user, string password);

        Task<ApplicationUser> UpdateAsync(ApplicationUser user, string? password = null);

        Task DeleteAsync(ApplicationUser user);
    }
}
