using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Local.Web.Data.User
{
    public class RoleRepository(RoleManager<IdentityRole> roleManager) : IRepository<IdentityRole, string>
    {
        public IQueryable<IdentityRole> GetAll() => roleManager.Roles;

        public IQueryable<IdentityRole> GetAll(IEnumerable<Expression<Func<IdentityRole, bool>>> filterExpressions) =>
            filterExpressions.Aggregate(roleManager.Roles, (current, filter) => current.Where(filter));

        public IQueryable<IdentityRole> GetAll(
            Expression<Func<IdentityRole, string?>> sortExpression,
            ListSortDirection sortDirection = ListSortDirection.Ascending
            ) =>
            ApplySort(GetAll(), sortExpression, sortDirection);

        public IQueryable<IdentityRole> GetAll(
            IEnumerable<Expression<Func<IdentityRole, bool>>>? filterExpressions,
            Expression<Func<IdentityRole, string?>>? sortExpression,
            ListSortDirection sortDirection = ListSortDirection.Ascending
            ) =>
            ApplySort(GetAll(filterExpressions), sortExpression, sortDirection);

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

        private static IQueryable<IdentityRole> ApplySort(
            IQueryable<IdentityRole> query,
            Expression<Func<IdentityRole, string?>> sortExpression,
            ListSortDirection sortDirection
            ) =>
            sortDirection == ListSortDirection.Ascending ?
                query.OrderBy(sortExpression) :
                query.OrderByDescending(sortExpression);
    }
}
