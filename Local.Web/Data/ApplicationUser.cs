using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Local.Web.Data
{
    public class ApplicationUser : IdentityUser
    {
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [ProtectedPersonalData]
        public override string? Email { get; set; }
    }
}
