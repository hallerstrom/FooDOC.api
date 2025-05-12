using Microsoft.AspNetCore.Identity;

namespace FooDOC.api.Models;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
}
