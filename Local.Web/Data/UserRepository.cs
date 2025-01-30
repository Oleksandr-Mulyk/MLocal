using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Local.Web.Data
{
    public class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
    {
        public IQueryable<ApplicationUser> GetAll(
            IEnumerable<Expression<Func<ApplicationUser, bool>>>? filterExpressions = null,
            Expression<Func<ApplicationUser, string?>>? sortExpression = null,
            ListSortDirection sortDirection = ListSortDirection.Ascending
            )
        {
            var users = userManager.Users;

            if (filterExpressions is not null)
            {
                users = filterExpressions.Aggregate(users, (current, filter) => current.Where(filter));
            }

            if (sortExpression is not null)
            {
                users = sortDirection == ListSortDirection.Ascending ?
                    users.OrderBy(sortExpression) :
                    users.OrderByDescending(sortExpression);
            }

            return users;
        }

        public async Task<IList<ApplicationUser>> GetAllAsync() =>
            await userManager.Users.ToListAsync();

        public async Task<int> GetTotalCount() =>
            await userManager.Users.CountAsync();

        public async Task<ApplicationUser> GetByIdAsync(string id) =>
            await userManager.FindByIdAsync(id) ??
            throw new Exception("User not found");

        public async Task<ApplicationUser> CreateAsync(ApplicationUser user, string password)
        {
            var result = await userManager.CreateAsync(user, password);

            return result.Succeeded ?
                user :
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<ApplicationUser> UpdateAsync(ApplicationUser user)
        {
            var result = await userManager.UpdateAsync(user);

            return result.Succeeded ?
                user :
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task DeleteAsync(ApplicationUser user)
        {
            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
