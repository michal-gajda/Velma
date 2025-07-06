namespace Velma.Web.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Velma.Web.Identity.Models;
using Velma.Web.Identity.Services;

internal static class ServiceExtensions
{
    private const string DEFAULT_PASSWORD = "P@ssw0rd";

    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Velma"));

        services
            .AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            // .AddDefaultTokenProviders()
            ;
    }

    public static void UseIdentity(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Velma.Web.Identity.ServiceExtensions");
        var userManager = app.Services.GetRequiredService<UserManager<ApplicationUser>>();

        try
        {
            var portfolioId = Guid.Parse("69f1d3d7-976e-4c7b-847e-b320d080893d");

            var user = new ApplicationUser
            {
                Id = "487812e4-14a4-4712-909c-e9b076048e41",
                Email = "user@domain.com",
                EmailConfirmed = true,
                Role = "User",
                UserName = "user@domain.com",
            };

            logger.LogInformation("Adding user '{UserEmail}'.", user.Email);
            userManager.CreateAsync(user, DEFAULT_PASSWORD);
            logger.LogInformation("Added user '{UserEmail}'.", user.Email);

            var admin = new ApplicationUser
            {
                Id = "f5d9912e-1617-4e48-8458-ce39db47595f",
                Email = "admin@domain.com",
                EmailConfirmed = true,
                Role = "Admin",
                UserName = "admin@domain.com",
            };

            logger.LogInformation("Adding admin '{UserEmail}'.", admin.Email);
            userManager.CreateAsync(admin, DEFAULT_PASSWORD);
            logger.LogInformation("Added admin '{UserEmail}'.", admin.Email);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, exception.Message);
        }
    }
}
