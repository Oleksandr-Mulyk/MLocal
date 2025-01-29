using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Local.Web.Data
{
    public class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
    {
        public IQueryable<ApplicationUser> GetAll(
            IDictionary<string, string>? filters = null,
            (string SortColumn, bool IsAscending) sort = default
            )
        {
            var users = userManager.Users;

            if (filters is not null)
            {
                foreach (var filter in filters)
                {
                    switch (filter.Key)
                    {
                        case IUserRepository.USERNAME_COLUMN_NAME:
                            users = users.Where(u => u.UserName.Contains(filter.Value));
                            break;
                        case IUserRepository.EMAIL_COLUMN_NAME:
                            users = users.Where(u => u.Email.Contains(filter.Value));
                            break;
                    }
                }
            }

            if (sort != default)
            {
                switch (sort.SortColumn)
                {
                    case IUserRepository.USERNAME_COLUMN_NAME:
                        users = sort.IsAscending ?
                            users.OrderBy(u => u.UserName) :
                            users.OrderByDescending(u => u.UserName);
                        break;
                    case IUserRepository.EMAIL_COLUMN_NAME:
                        users = sort.IsAscending ?
                            users.OrderBy(u => u.Email) :
                            users.OrderByDescending(u => u.Email);
                        break;
                }
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
