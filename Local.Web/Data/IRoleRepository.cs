using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Local.Web.Data
{
    public interface IRoleRepository
    {
        IQueryable<IdentityRole> GetAll(
            IEnumerable<Expression<Func<IdentityRole, bool>>> filterExpressions,
            Expression<Func<IdentityRole, string?>> sortExpression,
            ListSortDirection sortDirection
            );

        Task<IdentityRole> GetByIdAsync(string id);

        Task<IdentityRole> CreateAsync(IdentityRole role);

        Task<IdentityRole> UpdateAsync(IdentityRole role);

        Task DeleteAsync(IdentityRole role);
    }
}
