using Local.Web.Components.Layout.Alerts;
using Local.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace Local.Web.Components.Admin.Users.Pages
{
    [Route(Routes.CREATE_USER_PAGE)]
    [Authorize]
    public partial class CreateUser(
        IUserRepository userRepository,
        NavigationManager navigationManager,
        IMessageManager messageManager
        )
    {
        private readonly ApplicationUser newUser = new();

        private string? password;

        private async Task HandleValidSubmit()
        {
            try
            {
                var result = await userRepository.CreateAsync(newUser, password);
                messageManager.AddMessage(new ("User created successfully!", MessageType.Success));
            }
            catch (Exception ex)
            {
                messageManager.AddMessage(new (ex.Message, MessageType.Error));
            }

            navigationManager.NavigateTo(Routes.USER_LIST_PAGE, true);
        }
    }
}
