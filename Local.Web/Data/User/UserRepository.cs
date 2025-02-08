using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Local.Web.Data.User
{
    public class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
    {
        public IQueryable<ApplicationUser> GetAll() => userManager.Users;

        public IQueryable<ApplicationUser> GetAll(
            IEnumerable<Expression<Func<ApplicationUser, bool>>> filterExpressions
            ) =>
            filterExpressions.Aggregate(GetAll(), (current, filter) => current.Where(filter));

        public IQueryable<ApplicationUser> GetAll(
            Expression<Func<ApplicationUser, string?>> sortExpression,
            ListSortDirection sortDirection = ListSortDirection.Ascending
            ) =>
            ApplySort(GetAll(), sortExpression, sortDirection);

        public IQueryable<ApplicationUser> GetAll(
            IEnumerable<Expression<Func<ApplicationUser, bool>>>? filterExpressions,
            Expression<Func<ApplicationUser, string?>>? sortExpression,
            ListSortDirection sortDirection = ListSortDirection.Ascending
            ) =>
            ApplySort(GetAll(filterExpressions), sortExpression, sortDirection);

        public async Task<int> GetTotalCount() =>
            await userManager.Users.CountAsync();

        public async Task<ApplicationUser> GetByIdAsync(string id) =>
            await userManager.FindByIdAsync(id) ??
            throw new Exception("User not found");

        public async Task<ApplicationUser> GetByUserNameAsync(string userName) =>
            await userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName) ??
            throw new Exception("User not found");

        public async Task<ApplicationUser> CreateAsync(ApplicationUser user)
        {
            var result = await userManager.CreateAsync(user);

            return UserResult(result, user);
        }

        public async Task<ApplicationUser> CreateAsync(ApplicationUser user, string password)
        {
            var result = await userManager.CreateAsync(user, password);

            return UserResult(result, user);
        }

        public async Task<ApplicationUser> UpdateAsync(ApplicationUser user)
        {
            var result = await userManager.UpdateAsync(user);

            return UserResult(result, user);
        }

        public async Task<ApplicationUser> UpdateAsync(ApplicationUser user, string password)
        {
            user = await SetUserPasswordAsync(user, password);

            return await UpdateAsync(user);
        }

        public async Task DeleteAsync(ApplicationUser user)
        {
            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        private async Task<ApplicationUser> SetUserPasswordAsync(ApplicationUser user, string password)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResult = await userManager.ResetPasswordAsync(user, token, password);

            return passwordResult.Succeeded ?
                user :
                throw new Exception(string.Join(", ", passwordResult.Errors.Select(e => e.Description)));
        }

        private static IQueryable<ApplicationUser> ApplySort(
            IQueryable<ApplicationUser> query,
            Expression<Func<ApplicationUser, string?>> sortExpression,
            ListSortDirection sortDirection
            ) =>
            sortDirection == ListSortDirection.Ascending ?
                query.OrderBy(sortExpression) :
                query.OrderByDescending(sortExpression);

        private static ApplicationUser UserResult(IdentityResult result, ApplicationUser user) =>
            result.Succeeded ?
                user :
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}
