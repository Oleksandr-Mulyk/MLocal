using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MLocal.Web.Data
{
    public class RoleRepository(RoleManager<IdentityRole> roleManager) : IRoleRepository
    {
        public async Task<List<IdentityRole>> GetAllRolesAsync() => await roleManager.Roles.ToListAsync();

        public async Task<IdentityRole> GetRoleByIdAsync(string roleId) => await roleManager.FindByIdAsync(roleId);

        public async Task<bool> CreateRoleAsync(IdentityRole role)
        {
            var result = await roleManager.CreateAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> UpdateRoleAsync(IdentityRole role)
        {
            var result = await roleManager.UpdateAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                var result = await roleManager.DeleteAsync(role);
                return result.Succeeded;
            }

            return false;
        }
    }
}
