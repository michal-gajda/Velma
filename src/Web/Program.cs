namespace Velma.Web;

using Velma.Web.Identity;
using Velma.Web.IdentityServer;

public class Program
{
    const int EXIT_SUCCESS = 0;

    public static async Task<int> Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddIdentity();
        builder.Services.AddDuendeIdentityServer();

        var app = builder.Build();

        app.UseIdentityServer();

        app.MapGet("/", () => "Hello World!");

        await app.RunAsync();

        return EXIT_SUCCESS;
    }
}
