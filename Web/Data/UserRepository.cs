using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MLocal.Web.Data
{
    public class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
    {
        public async Task<List<ApplicationUser>> GetAllUsersAsync() => await userManager.Users.ToListAsync();

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId) => await userManager.FindByIdAsync(userId);

        public async Task<ApplicationUser?> GetUserByUsernameAsync(string username) =>
            await userManager.FindByNameAsync(username);

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email) =>
            await userManager.FindByEmailAsync(email);

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            return false;
        }
    }
}
