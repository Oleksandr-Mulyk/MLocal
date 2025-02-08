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
    [Route(ToDoRoute.CREATE_TODO_PAGE)]
    [Authorize]
    public partial class CreateToDo(
        IRepository<IToDoItem> toDoRepository,
        IUserRepository userRepository,
        NavigationManager navigationManager,
        IMessageManager messageManager,
        IHttpContextAccessor httpContextAccessor
        )
    {
        private readonly ToDoViewModel toDoViewModel = new();

        private List<ApplicationUser> users = [];

        protected override async Task OnInitializedAsync()
        {
            users = await userRepository.GetAll().ToListAsync();
        }

        private async Task HandleValidSubmit()
        {
            try
            {
                string? userName = httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await userRepository.GetByUserNameAsync(userName);

                IToDoItem newToDo = new ToDoItem()
                {
                    Title = toDoViewModel.Title,
                    Description = toDoViewModel.Description,
                    DeathLine = toDoViewModel.DeathLine,
                    Status = ToDoItemStatus.Plane,
                    Created = DateTime.Now,
                    CreatedBy = user,
                    AssignedTo = [.. userRepository.GetAll([u => toDoViewModel.AssignedTo.Contains(u.Id)])],
                    VisibleFor = [.. userRepository.GetAll([u => toDoViewModel.VisibleFor.Contains(u.Id)])]
                };
                await toDoRepository.CreateAsync(newToDo);
                messageManager.AddMessage(new("To Do created successfully!", MessageType.Success));
                navigationManager.NavigateTo(ToDoRoute.TODO_LIST_PAGE, true);
            }
            catch (Exception ex)
            {
                messageManager.AddMessage(new(ex.Message, MessageType.Danger));
                navigationManager.NavigateTo(ToDoRoute.CREATE_TODO_PAGE, true);
            }
        }
    }
}
