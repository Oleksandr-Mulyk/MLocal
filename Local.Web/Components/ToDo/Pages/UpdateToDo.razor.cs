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

        protected override async Task OnInitializedAsync()
        {
            toDoItem = await toDoRepository.GetByIdAsync(Id) ??
            throw new InvalidOperationException($"ToDo with ID {Id} not found!");

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
                    string? userName = httpContextAccessor.HttpContext.User.Identity.Name;
                    var user = await userRepository.GetByUserNameAsync(userName);
                    toDoItem.StatusChanged = DateTime.Now;
                    toDoItem.StatusChangedBy = user;
                }

                await toDoRepository.UpdateAsync(toDoItem);
                messageManager.AddMessage(new Message("ToDo updated successfully!", MessageType.Success));

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
    }
}
