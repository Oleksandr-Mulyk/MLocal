namespace Local.Web.Data.User
{
    public interface IUserRepository : IRepository<ApplicationUser, string>
    {
        Task<ApplicationUser> GetByUserNameAsync(string userName);

        Task<ApplicationUser> CreateAsync(ApplicationUser user, string password);

        Task<ApplicationUser> UpdateAsync(ApplicationUser user, string password);
    }
}
