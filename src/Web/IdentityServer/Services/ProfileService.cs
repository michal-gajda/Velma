namespace Velma.Web.IdentityServer.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Velma.Web.Identity.Models;
using Velma.Web.IdentityServer.Models;

internal sealed class ProfileService(UserManager<ApplicationUser> userManager) : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await userManager.GetUserAsync(context.Subject);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user?.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Sub, user?.Id ?? Guid.Empty.ToString()),

            new Claim(UserClaimNames.Role, user!.Role),
            new Claim(UserClaimNames.UserId, user.Id.ToString()),
        };

        context.IssuedClaims.AddRange(claims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await userManager.GetUserAsync(context.Subject);
        context.IsActive = user is not null;
    }
}
