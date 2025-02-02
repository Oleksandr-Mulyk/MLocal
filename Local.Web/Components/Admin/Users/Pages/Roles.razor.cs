using Local.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using IdentityRoleExpression =
    System.Linq.Expressions.Expression<System.Func<Microsoft.AspNetCore.Identity.IdentityRole, bool>>;

namespace Local.Web.Components.Admin.Users.Pages
{
    [Route(AdminUserRoute.ROLE_LIST_PAGE)]
    [Authorize]
    public partial class Roles(IRoleRepository roleRepository, NavigationManager navigationManager)
    {
        private const string NAME_COLUMN_NAME = "Name";

        private IList<IdentityRole>? roles;

        private readonly int itemsPerPage = 10;

        private int currentPage = 1;

        private int totalCount;

        private string currentSortColumn = NAME_COLUMN_NAME;

        private ListSortDirection sortDirection = ListSortDirection.Ascending;

        private readonly Dictionary<string, string> searchValues = new()
        {
            { NAME_COLUMN_NAME, string.Empty }
        };

        protected override async Task OnInitializedAsync() =>
            await LoadPageAsync(currentPage);

        private async Task LoadPageAsync(int pageNumber)
        {
            var filters = searchValues
                .Where(s => !string.IsNullOrEmpty(s.Value))
                .Select(s => s.Key switch
                {
                    NAME_COLUMN_NAME =>
                        (IdentityRoleExpression)(r => r.Name != null && r.Name.Contains(s.Value)),
                    _ => throw new NotImplementedException()
                });

            var userQuery = roleRepository.GetAll(
                filters,
                u => EF.Property<string>(u, currentSortColumn),
                sortDirection
            );

            totalCount = await userQuery.CountAsync();
            currentPage = pageNumber;
            roles = await userQuery.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
        }

        private void NavigateToCreateRole() =>
            navigationManager.NavigateTo(AdminUserRoute.CREATE_ROLE_PAGE);

        private void NavigateToEditRole(string roleId) =>
            navigationManager.NavigateTo(
                AdminUserRoute.UPDATE_ROLE_PAGE.Replace("{" + AdminUserRoute.ROLE_ID_PARAM_NAME + "}", roleId)
                );

        private void NavigateToDeleteRole(string roleId) =>
            navigationManager.NavigateTo(
                AdminUserRoute.DELETE_ROLE_PAGE.Replace("{" + AdminUserRoute.ROLE_ID_PARAM_NAME + "}", roleId)
                );

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
