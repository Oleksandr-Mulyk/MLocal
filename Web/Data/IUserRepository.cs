namespace MLocal.Web.Data
{
    public interface IUserRepository
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();

        Task<ApplicationUser> GetUserByIdAsync(string userId);

        Task<ApplicationUser> GetUserByUsernameAsync(string username);

        Task<ApplicationUser> GetUserByEmailAsync(string email);

        Task<bool> DeleteUserAsync(string userId);
    }
}
