using Microsoft.AspNetCore.Identity;

namespace Local.Web.Data
{
    public class RoleRepository(RoleManager<IdentityRole> roleManager) : IRoleRepository
    {
        public IQueryable<IdentityRole> GetAll() =>
            roleManager.Roles;

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
