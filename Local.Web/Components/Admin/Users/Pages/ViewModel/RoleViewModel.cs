using System.ComponentModel.DataAnnotations;

namespace Local.Web.Components.Admin.Users.Pages.ViewModel
{
    public class RoleViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
