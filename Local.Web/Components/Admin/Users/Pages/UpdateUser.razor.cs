using Local.Web.Components.Admin.Users.Pages.ViewModel;
using Local.Web.Components.Layout.Alerts;
using Local.Web.Data;
using Local.Web.Data.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace Local.Web.Components.Admin.Users.Pages
{
    [Route(AdminUserRoute.UPDATE_USER_PAGE)]
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

        private UserViewModel userViewModel = new();

        protected override async Task OnInitializedAsync()
        {
            user = await userRepository.GetByIdAsync(Id) ??
            throw new InvalidOperationException($"User with ID {Id} not found!");

            userViewModel = new UserViewModel
            {
                Email = user.Email ?? user.UserName
            };
        }

        private async Task HandleValidSubmit()
        {
            try
            {
                user.UserName = userViewModel.Email;
                user.Email = userViewModel.Email;

                var password = string.IsNullOrEmpty(userViewModel.Password) ? null : userViewModel.Password;

                _ = await (
                    userViewModel.Password is null ?
                    userRepository.UpdateAsync(user) :
                    userRepository.UpdateAsync(user, userViewModel.Password)
                    );
                messageManager.AddMessage(new Message("User updated successfully!", MessageType.Success));

                navigationManager.NavigateTo(AdminUserRoute.USER_LIST_PAGE, true);
            }
            catch (Exception ex)
            {
                messageManager.AddMessage(new Message(ex.Message, MessageType.Danger));
                navigationManager.NavigateTo(
                    AdminUserRoute.UPDATE_USER_PAGE.Replace("{" + AdminUserRoute.USER_ID_PARAM_NAME + "}", Id)
                    );
            }
        }
    }
}
