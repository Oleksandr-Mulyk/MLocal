using Local.Web.Components.Admin.Users.Pages.ViewModel;
using Local.Web.Components.Layout.Alerts;
using Local.Web.Data.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace Local.Web.Components.Admin.Users.Pages
{
    [Route(AdminUserRoute.CREATE_ROLE_PAGE)]
    [Authorize]
    public partial class CreateRole(
        IRoleRepository roleRepository,
        NavigationManager navigationManager,
        IMessageManager messageManager
        )
    {
        private readonly RoleViewModel roleViewModel = new();

        private async Task HandleValidSubmit()
        {
            try
            {
                IdentityRole newRole = new()
                {
                    Name = roleViewModel.Name
                };
                var result = await roleRepository.CreateAsync(newRole);
                messageManager.AddMessage(new("Role created successfully!", MessageType.Success));
                navigationManager.NavigateTo(AdminUserRoute.ROLE_LIST_PAGE, true);
            }
            catch (Exception ex)
            {
                messageManager.AddMessage(new(ex.Message, MessageType.Danger));
                navigationManager.NavigateTo(AdminUserRoute.CREATE_ROLE_PAGE, true);
            }
        }
    }
}
