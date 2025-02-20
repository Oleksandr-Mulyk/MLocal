using Local.Web.Components.Layout.Alerts;
using Local.Web.Components.ToDo.ViewModel;
using Local.Web.Data;
using Local.Web.Data.ToDo;
using Local.Web.Data.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Local.Web.Components.ToDo.Pages
{
    [Route(ToDoRoute.UPDATE_TODO_PAGE)]
    [Authorize]
    public partial class UpdateToDo(
        IRepository<IToDoItem> toDoRepository,
        IUserRepository userRepository,
        NavigationManager navigationManager,
        IMessageManager messageManager,
        IHttpContextAccessor httpContextAccessor
        )
    {
        [Parameter]
        public int Id { get; set; } = default;

        private IToDoItem? toDoItem;

        private ToDoViewModel? toDoViewModel = new();

        private readonly ToDoItemStatus[] statuses = Enum.GetValues<ToDoItemStatus>();

        private List<ApplicationUser> users = [];

        private readonly string? currentUserName = httpContextAccessor.HttpContext.User.Identity.Name;

        private bool isDisabled = false;

        protected override async Task OnInitializedAsync()
        {
            toDoItem = await toDoRepository.GetByIdAsync(Id) ??
            throw new InvalidOperationException($"ToDo with ID {Id} not found!");

            if (toDoItem.CreatedBy?.UserName != currentUserName &&
                !toDoItem.AssignedTo.Any(u => u.UserName == currentUserName) &&
                !toDoItem.VisibleFor.Any(u => u.UserName == currentUserName))
            {
                navigationManager.NavigateTo("/404");
                return;
            }

            toDoViewModel = new ToDoViewModel
            {
                Title = toDoItem.Title,
                Status = toDoItem.Status,
                Description = toDoItem.Description,
                DeathLine = toDoItem.DeathLine,
                AssignedTo = [.. toDoItem.AssignedTo.Select(u => u.Id)],
                VisibleFor = [.. toDoItem.VisibleFor.Select(u => u.Id)],
                StatusChanged = toDoItem.StatusChanged,
                StatusChangedBy = toDoItem.StatusChangedBy?.UserName ?? toDoItem.StatusChangedBy?.Email
            };

            users = await userRepository.GetAll().ToListAsync();
            isDisabled = toDoItem.CreatedBy.UserName != currentUserName;
        }

        private async Task HandleValidSubmit()
        {
            try
            {
                toDoItem.Title = toDoViewModel.Title;
                toDoItem.Description = toDoViewModel.Description;
                toDoItem.DeathLine = toDoViewModel.DeathLine;
                toDoItem.AssignedTo = [.. userRepository.GetAll([u => toDoViewModel.AssignedTo.Contains(u.Id)])];
                toDoItem.VisibleFor = [.. userRepository.GetAll([u => toDoViewModel.VisibleFor.Contains(u.Id)])];

                if (toDoItem.Status != toDoViewModel.Status)
                {
                    toDoItem.Status = toDoViewModel.Status;
                    var user = await userRepository.GetByUserNameAsync(currentUserName);
                    toDoItem.StatusChanged = DateTime.Now;
                    toDoItem.StatusChangedBy = user;
                }

                await toDoRepository.UpdateAsync(toDoItem);
                messageManager.AddMessage(new Message(Loc["ToDo updated successfully!"], MessageType.Success));

                navigationManager.NavigateTo(ToDoRoute.TODO_LIST_PAGE, true);
            }
            catch (Exception ex)
            {
                messageManager.AddMessage(new Message(ex.Message, MessageType.Danger));
                navigationManager.NavigateTo(
                    ToDoRoute.UPDATE_TODO_PAGE.Replace("{" + ToDoRoute.TODO_ID_PARAM_NAME + "}", Id.ToString())
                );
            }
        }

        private string? GetStatusComment() =>
            toDoViewModel.StatusChanged is not null && toDoViewModel.StatusChangedBy is not null ?
            $"Status changed at {toDoViewModel.StatusChanged} by {toDoViewModel.StatusChangedBy}" :
            null;
    }
}
