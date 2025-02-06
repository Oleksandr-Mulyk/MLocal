using Local.Web.Components.Layout.Alerts;
using Local.Web.Data.ToDo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace Local.Web.Components.ToDo.Pages
{
    [Route(ToDoRoute.DELETE_TODO_PAGE)]
    [Authorize]
    public partial class DeleteToDo(
        IToDoRepository toDoRepository,
        NavigationManager navigationManager,
        IMessageManager messageManager
        )
    {
        [Parameter]
        public int Id { get; set; }

        private IToDoItem? toDoItem;

        protected override async Task OnInitializedAsync() =>
            toDoItem = await toDoRepository.GetByIdAsync(Id) ??
            throw new InvalidOperationException($"ToDo with ID {Id} not found!");

        private async Task DeleteThisToDo()
        {
            try
            {
                await toDoRepository.DeleteAsync(toDoItem);
                messageManager.AddMessage(new Message("ToDo deleted successfully!", MessageType.Success));
            }
            catch (Exception ex)
            {
                messageManager.AddMessage(new Message(ex.Message, MessageType.Danger));
            }

            navigationManager.NavigateTo(ToDoRoute.TODO_LIST_PAGE, true);
        }

        private void Cancel() =>
            navigationManager.NavigateTo(ToDoRoute.TODO_LIST_PAGE, true);
    }
}
