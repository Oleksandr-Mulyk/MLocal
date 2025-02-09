using Local.Web.Components.Admin.Users.Pages.ViewModel;
using Local.Web.Components.Layout.Alerts;
using Local.Web.Data;
using Local.Web.Data.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace Local.Web.Components.Admin.Users.Pages
{
    [Route(AdminUserRoute.CREATE_USER_PAGE)]
    [Authorize]
    public partial class CreateUser(
        IUserRepository userRepository,
        NavigationManager navigationManager,
        IMessageManager messageManager
        )
    {
        private readonly UserViewModel userViewModel = new();

        private async Task HandleValidSubmit()
        {
            try
            {
                ApplicationUser newUser = new()
                {
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email,
                    EmailConfirmed = true
                };
                _= await (
                    userViewModel.Password is null ?
                    userRepository.CreateAsync(newUser) :
                    userRepository.CreateAsync(newUser, userViewModel.Password)
                    );
                messageManager.AddMessage(new("User created successfully!", MessageType.Success));
                navigationManager.NavigateTo(AdminUserRoute.USER_LIST_PAGE, true);
            }
            catch (Exception ex)
            {
                messageManager.AddMessage(new(ex.Message, MessageType.Danger));
                navigationManager.NavigateTo(AdminUserRoute.CREATE_USER_PAGE, true);
            }
        }
    }
}
