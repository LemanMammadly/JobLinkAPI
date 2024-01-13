using Microsoft.AspNetCore.Identity;

namespace JobLink.Core.Entities;

public class AppUser:IdentityUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresDate { get; set; }
    public EmailToken? EmailToken { get; set; }
    public bool IsDeleted { get; set; }
}
