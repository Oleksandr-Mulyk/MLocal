using Local.Web.Components.Layout.Alerts;
using Local.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace Local.Web.Components.Admin.Users.Pages
{
    [Route(AdminUserRoute.DELETE_ROLE_PAGE)]
    [Authorize]
    public partial class DeleteRole(
        IRoleRepository roleRepository,
        NavigationManager navigationManager,
        IMessageManager messageManager
        )
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        private IdentityRole role = new();

        protected override async Task OnInitializedAsync() =>
            role = await roleRepository.GetByIdAsync(Id) ??
            throw new InvalidOperationException($"Role with ID {Id} not found!");

        private async Task DeleteThisRole()
        {
            try
            {
                await roleRepository.DeleteAsync(role);
                messageManager.AddMessage(new Message("Role deleted successfully!", MessageType.Success));
            }
            catch (Exception ex)
            {
                messageManager.AddMessage(new Message(ex.Message, MessageType.Danger));
            }

            navigationManager.NavigateTo(AdminUserRoute.ROLE_LIST_PAGE, true);
        }

        private void Cancel() =>
            navigationManager.NavigateTo(AdminUserRoute.ROLE_LIST_PAGE, true);
    }
}
