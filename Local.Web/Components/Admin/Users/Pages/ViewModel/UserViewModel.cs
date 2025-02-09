using System.ComponentModel.DataAnnotations;

namespace Local.Web.Components.Admin.Users.Pages.ViewModel
{
    public class UserViewModel
    {
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
    }
}
