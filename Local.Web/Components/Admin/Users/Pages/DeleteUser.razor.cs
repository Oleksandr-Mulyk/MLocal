using Local.Web.Components.Layout.Alerts;
using Local.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace Local.Web.Components.Admin.Users.Pages
{
    [Route(Routes.DELETE_USER_PAGE)]
    [Authorize]
    public partial class DeleteUser(
        IUserRepository userRepository,
        NavigationManager navigationManager,
        IMessageManager messageManager
        )
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        private ApplicationUser user = new();

        protected override async Task OnInitializedAsync() =>
            user = await userRepository.GetByIdAsync(Id) ??
            throw new InvalidOperationException($"User with ID {Id} not found!");

        private async Task DeleteThisUser()
        {
            try
            {
                await userRepository.DeleteAsync(user);
                messageManager.AddMessage(new Message("User deleted successfully!", MessageType.Success));
            }
            catch (Exception ex)
            {
                messageManager.AddMessage(new Message(ex.Message, MessageType.Danger));
            }

            navigationManager.NavigateTo(Routes.USER_LIST_PAGE, true);
        }

        private void Cancel() =>
            navigationManager.NavigateTo(Routes.USER_LIST_PAGE, true);
    }
}
