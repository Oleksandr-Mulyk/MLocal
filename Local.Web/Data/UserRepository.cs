using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Local.Web.Data
{
    public class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
    {
        public async Task<IList<ApplicationUser>> GetAllAsync() =>
            await userManager.Users.ToListAsync();

        public async Task<ApplicationUser> GetByIdAsync(string id) =>
            await userManager.FindByIdAsync(id) ??
            throw new Exception("User not found");

        public async Task<ApplicationUser> CreateAsync(ApplicationUser user)
        {
            var result = await userManager.CreateAsync(user);

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

        public async Task<ApplicationUser> DeleteAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                return result.Succeeded ?
                    user :
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            throw new Exception("User not found");
        }
    }
}
