using Local.Web.Components.Admin.Users.Pages.ViewModel;
using Local.Web.Components.Layout.Alerts;
using Local.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace Local.Web.Components.Admin.Users.Pages
{
    [Route(AdminUserRoute.UPDATE_ROLE_PAGE)]
    [Authorize]
    public partial class UpdateRole(
        IRoleRepository roleRepository,
        NavigationManager navigationManager,
        IMessageManager messageManager
        )
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        private IdentityRole role = new();

        private RoleViewModel roleViewModel = new();

        protected override async Task OnInitializedAsync()
        {
            role = await roleRepository.GetByIdAsync(Id) ??
            throw new InvalidOperationException($"User with ID {Id} not found!");

            roleViewModel = new RoleViewModel
            {
                Name = role.Name!
            };
        }

        private async Task HandleValidSubmit()
        {
            try
            {
                role.Name = roleViewModel.Name;

                await roleRepository.UpdateAsync(role);
                messageManager.AddMessage(new Message("Role updated successfully!", MessageType.Success));

                navigationManager.NavigateTo(AdminUserRoute.ROLE_LIST_PAGE, true);
            }
            catch (Exception ex)
            {
                messageManager.AddMessage(new Message(ex.Message, MessageType.Danger));
                navigationManager.NavigateTo(
                    AdminUserRoute.UPDATE_ROLE_PAGE.Replace("{" + AdminUserRoute.ROLE_ID_PARAM_NAME + "}", Id)
                    );
            }
        }
    }
}
