﻿using Local.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using ApplicationUserExpression =
    System.Linq.Expressions.Expression<System.Func<Local.Web.Data.ApplicationUser, bool>>;

namespace Local.Web.Components.Admin.Users.Pages
{
    [Route(Routes.USER_LIST_PAGE)]
    [Authorize]
    public partial class Users(IUserRepository userRepository, NavigationManager navigationManager)
    {
        private const string USERNAME_COLUMN_NAME = "UserName";

        private const string EMAIL_COLUMN_NAME = "Email";

        private IList<ApplicationUser> users = [];

        private readonly int itemsPerPage = 10;

        private int currentPage = 1;

        private int totalCount;

        private string currentSortColumn = USERNAME_COLUMN_NAME;

        private ListSortDirection sortDirection = ListSortDirection.Ascending;

        private readonly Dictionary<string, string> searchValues = new()
        {
            { USERNAME_COLUMN_NAME, string.Empty },
            { EMAIL_COLUMN_NAME, string.Empty }
        };

        protected override async Task OnInitializedAsync() =>
            await LoadPageAsync(currentPage);

        private async Task LoadPageAsync(int pageNumber)
        {
            var filters = searchValues
                .Where(s => !string.IsNullOrEmpty(s.Value))
                .Select(s => s.Key switch
                {
                    USERNAME_COLUMN_NAME =>
                        (ApplicationUserExpression)(u => u.UserName != null && u.UserName.Contains(s.Value)),
                    EMAIL_COLUMN_NAME =>
                        (ApplicationUserExpression)(u => u.Email != null && u.Email.Contains(s.Value)),
                    _ => throw new NotImplementedException()
                });

            var userQuery = userRepository.GetAll(
                filters,
                u => EF.Property<string>(u, currentSortColumn),
                sortDirection
            );

            totalCount = await userQuery.CountAsync();
            currentPage = pageNumber;
            users = await userQuery.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
        }

        private void NavigateToCreateUser() =>
            navigationManager.NavigateTo(Routes.CREATE_USER_PAGE);

        private void EditUser(string id) =>
            navigationManager.NavigateTo(Routes.UPDATE_USER_PAGE.Replace("{" + Routes.USER_ID_PARAM_NAME + "}", id));

        private void DeleteUser(string id) =>
            navigationManager.NavigateTo(Routes.DELETE_USER_PAGE.Replace("{" + Routes.USER_ID_PARAM_NAME + "}", id));

        private async Task SortBy(string columnName)
        {
            if (currentSortColumn == columnName)
            {
                sortDirection = sortDirection == ListSortDirection.Ascending ?
                    ListSortDirection.Descending :
                    ListSortDirection.Ascending;
            }
            else
            {
                currentSortColumn = columnName;
                sortDirection = ListSortDirection.Ascending;
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
