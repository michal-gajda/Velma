namespace Velma.Web.Identity.Models;

using Microsoft.AspNetCore.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public string Role { get; set; } = string.Empty;
}
