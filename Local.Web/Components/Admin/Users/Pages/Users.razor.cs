using Local.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Local.Web.Components.Admin.Users.Pages
{
    [Route(Routes.USER_LIST_PAGE)]
    [Authorize]
    public partial class Users(IUserRepository userRepository, NavigationManager navigationManager)
    {
        private IList<ApplicationUser> users = [];

        private readonly int itemsPerPage = 10;

        private int currentPage = 1;

        private int totalCount;

        private string currentSortColumn = IUserRepository.USERNAME_COLUMN_NAME;

        private bool isAscending = true;

        private readonly Dictionary<string, string> searchValues = new()
        {
            { IUserRepository.USERNAME_COLUMN_NAME, string.Empty },
            { IUserRepository.EMAIL_COLUMN_NAME, string.Empty }
        };

        protected override async Task OnInitializedAsync() =>
            await LoadPageAsync(currentPage);

        private async Task LoadPageAsync(int pageNumber)
        {
            var userQuery = userRepository.GetAll(
                searchValues.Where(s => s.Value != string.Empty).ToDictionary(),
                (currentSortColumn, isAscending)
                );

            totalCount = await userQuery.CountAsync();

            currentPage = pageNumber;
            users = await userQuery.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
        }

        private void NavigateToCreateUser() =>
            navigationManager.NavigateTo(Routes.CREATE_USER_PAGE);

        private void EditUser(string id) =>
            navigationManager.NavigateTo(Routes.DELETE_USER_PAGE.Replace("{" + Routes.USER_ID_PARAM_NAME + "}", id));

        private void DeleteUser(string id) =>
            navigationManager.NavigateTo(Routes.DELETE_USER_PAGE.Replace("{" + Routes.USER_ID_PARAM_NAME + "}", id));

        private async Task SortBy(string columnName)
        {
            if (currentSortColumn == columnName)
            {
                isAscending = !isAscending;
            }
            else
            {
                currentSortColumn = columnName;
                isAscending = true;
            }
            await LoadPageAsync(currentPage);
        }

        private async Task OnSearchChanged(string columnName, string value)
        {
            searchValues[columnName] = value;

            await LoadPageAsync(currentPage);
        }
    }
}
