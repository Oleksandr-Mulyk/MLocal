using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Local.Web.Data.User
{
    public class RoleRepository(RoleManager<IdentityRole> roleManager) : IRoleRepository
    {
        public IQueryable<IdentityRole> GetAll(
            IEnumerable<Expression<Func<IdentityRole, bool>>>? filterExpressions = null,
            Expression<Func<IdentityRole, string?>>? sortExpression = null,
            ListSortDirection sortDirection = ListSortDirection.Ascending
            )
        {
            var roles = roleManager.Roles;

            if (filterExpressions is not null)
            {
                roles = filterExpressions.Aggregate(roles, (current, filter) => current.Where(filter));
            }

            if (sortExpression is not null)
            {
                roles = sortDirection == ListSortDirection.Ascending ?
                    roles.OrderBy(sortExpression) :
                    roles.OrderByDescending(sortExpression);
            }

            return roles;
        }

        public async Task<IdentityRole> GetByIdAsync(string id) =>
            await roleManager.FindByIdAsync(id) ??
            throw new Exception("Role not found");

        public async Task<IdentityRole> CreateAsync(IdentityRole role)
        {
            var result = await roleManager.CreateAsync(role);

            return result.Succeeded ?
                role :
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<IdentityRole> UpdateAsync(IdentityRole role)
        {
            var result = await roleManager.UpdateAsync(role);
            return result.Succeeded ?
                role :
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task DeleteAsync(IdentityRole role)
        {
            var result = await roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
