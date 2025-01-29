namespace Local.Web.Data
{
    public interface IUserRepository
    {
        Task<IList<ApplicationUser>> GetAllAsync();

        Task<ApplicationUser> GetByIdAsync(string id);

        Task<ApplicationUser> CreateAsync(ApplicationUser user);

        Task<ApplicationUser> UpdateAsync(ApplicationUser user);

        Task<ApplicationUser> DeleteAsync(string id);
    }
}
