using Local.Web.Components.Layout.Alerts;
using Local.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace Local.Web.Components.Admin.Users.Pages
{
    [Route(Routes.UPDATE_USER_PAGE)]
    [Authorize]
    public partial class UpdateUser(
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

        private async Task HandleValidSubmit()
        {
            try
            {
                await userRepository.UpdateAsync(user);
                messageManager.AddMessage(new Message("User updated successfully!", MessageType.Success));
            }
            catch (Exception ex)
            {
                messageManager.AddMessage(new Message(ex.Message, MessageType.Danger));
            }

            navigationManager.NavigateTo(Routes.USER_LIST_PAGE, true);
        }
    }
}
