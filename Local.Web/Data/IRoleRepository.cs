using Microsoft.AspNetCore.Identity;

namespace Local.Web.Data
{
    public interface IRoleRepository
    {
        IQueryable<IdentityRole> GetAll();

        Task<IdentityRole> GetByIdAsync(string id);

        Task<IdentityRole> CreateAsync(IdentityRole role);

        Task<IdentityRole> UpdateAsync(IdentityRole role);

        Task DeleteAsync(IdentityRole role);
    }
}
