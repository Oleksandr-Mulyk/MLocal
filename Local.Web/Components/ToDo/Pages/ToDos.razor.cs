using Local.Web.Data;
using Local.Web.Data.ToDo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Expressions;
using ToDoExpression =
    System.Linq.Expressions.Expression<System.Func<Local.Web.Data.ToDo.IToDoItem, bool>>;

namespace Local.Web.Components.ToDo.Pages
{
    [Route(ToDoRoute.TODO_LIST_PAGE)]
    [Authorize]
    public partial class ToDos(IRepository<IToDoItem> toDoRepository, NavigationManager navigationManager)
    {
        private const string TITLE_COLUMN_NAME = "Title";

        private const string STATUS_COLUMN_NAME = "Status";

        private const string CREATED_BY_COLUMN_NAME = "CreatedBy";

        private const string ASSIGNED_TO_COLUMN_NAME = "AssignedTo";

        private const string CREATED_COLUMN_NAME = "Created";

        private const string DEATHLINE_COLUMN_NAME = "DeathLine";

        private IList<IToDoItem>? toDoList;

        private readonly int itemsPerPage = 10;

        private int currentPage = 1;

        private int totalCount;

        private string currentSortColumn = DEATHLINE_COLUMN_NAME;

        private ListSortDirection sortDirection = ListSortDirection.Ascending;

        private readonly ToDoItemStatus[] statuses = Enum.GetValues<ToDoItemStatus>();

        private readonly Dictionary<string, string> stringSearchValues = new()
        {
            { TITLE_COLUMN_NAME, string.Empty },
            { STATUS_COLUMN_NAME, string.Empty },
            { CREATED_BY_COLUMN_NAME, string.Empty },
            { ASSIGNED_TO_COLUMN_NAME, string.Empty }
        };

        private readonly Dictionary<string, DateTime?> dateTimeSearchValues = new()
        {
            { CREATED_COLUMN_NAME + "_from", null },
            { CREATED_COLUMN_NAME + "_to", null },
            { DEATHLINE_COLUMN_NAME + "_from", null },
            { DEATHLINE_COLUMN_NAME + "_to", null }
        };

        protected async override Task OnInitializedAsync() =>
            await LoadPageAsync(currentPage);

        private async Task LoadPageAsync(int pageNumber)
        {
            var filters = stringSearchValues
                .Where(s => !string.IsNullOrEmpty(s.Value))
                .Select(s => s.Key switch
                {
                    TITLE_COLUMN_NAME =>
                        (ToDoExpression)(t => t.Title != null && t.Title.Contains(s.Value)),
                    STATUS_COLUMN_NAME =>
                        t => t.Status.ToString() == s.Value,
                    CREATED_BY_COLUMN_NAME =>
                        t => t.CreatedBy != null && t.CreatedBy.UserName.Contains(s.Value),
                    ASSIGNED_TO_COLUMN_NAME =>
                        t => t.AssignedTo.Any(u => u.UserName.Contains(s.Value)),
                    _ => throw new NotImplementedException()
                })
                .Concat(
                    dateTimeSearchValues
                    .Where(s => s.Value.HasValue)
                    .Select(s => s.Key switch
                    {
                        CREATED_COLUMN_NAME + "_from" =>
                            (ToDoExpression)(t => t.Created >= s.Value),
                        CREATED_COLUMN_NAME + "_to" =>
                            t => t.Created <= s.Value,
                        DEATHLINE_COLUMN_NAME + "_from" =>
                            t => t.DeathLine >= s.Value,
                        DEATHLINE_COLUMN_NAME + "_to" =>
                            t => t.DeathLine <= s.Value,
                        _ => throw new NotImplementedException()
                    })
                );

            Expression<Func<IToDoItem, string?>> sort = u => EF.Property<string>(u, currentSortColumn);

            var query = filters.Count() > 0 ?
                toDoRepository.GetAll(filters, sort, sortDirection) :
                toDoRepository.GetAll(sort, sortDirection);

            totalCount = await query.CountAsync();
            currentPage = pageNumber;
            toDoList = await query.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
        }

        private void NavigateToCreateToDo() =>
            navigationManager.NavigateTo(ToDoRoute.CREATE_TODO_PAGE);

        private void NavigateToEditToDo(int toDoId) =>
            navigationManager.NavigateTo(
                ToDoRoute.UPDATE_TODO_PAGE.Replace("{" + ToDoRoute.TODO_ID_PARAM_NAME + "}", toDoId.ToString())
                );

        private void NavigateToDeleteToDo(int toDoId) =>
            navigationManager.NavigateTo(
                ToDoRoute.DELETE_TODO_PAGE.Replace("{" + ToDoRoute.TODO_ID_PARAM_NAME + "}", toDoId.ToString())
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
            stringSearchValues[columnName] = value;

            await LoadPageAsync(currentPage);
        }

        private async Task OnDateTimeSearchChanged(string columnName, ChangeEventArgs e)
        {
            dateTimeSearchValues[columnName] =
                DateTime.TryParse(e.Value?.ToString(), out DateTime date) ? date : null;

            await LoadPageAsync(currentPage);
        }
    }
}
