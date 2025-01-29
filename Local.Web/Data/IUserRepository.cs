namespace Local.Web.Data
{
    public interface IUserRepository
    {
        public const string USERNAME_COLUMN_NAME = "UserName";

        public const string EMAIL_COLUMN_NAME = "Email";

        IQueryable<ApplicationUser> GetAll(
            IDictionary<string, string> filters,
            (string SortColumn, bool IsAscending) sort
            );

        Task<ApplicationUser> GetByIdAsync(string id);

        Task<ApplicationUser> CreateAsync(ApplicationUser user, string password);

        Task<ApplicationUser> UpdateAsync(ApplicationUser user);

        Task DeleteAsync(ApplicationUser user);
    }
}
