using Microsoft.AspNetCore.Identity;

namespace MLocal.Web.Data
{
    public interface IRoleRepository
    {
        Task<List<IdentityRole>> GetAllRolesAsync();

        Task<IdentityRole> GetRoleByIdAsync(string roleId);

        Task<bool> CreateRoleAsync(IdentityRole role);

        Task<bool> UpdateRoleAsync(IdentityRole role);

        Task<bool> DeleteRoleAsync(string roleId);
    }
}
