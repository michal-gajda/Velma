namespace Velma.Web.IdentityServer;

using Duende.IdentityModel;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Velma.Web.Identity.Models;
using Velma.Web.IdentityServer.Services;

internal static class ServiceExtensions
{
    private const string DEFAULT_PASSWORD = "P@ssw0rd";
    private const string API_BASE_URI = "http://localhost:5080";
    private const string WAF_BASE_URI = "http://localhost:6080";

    public static void AddDuendeIdentityServer(this IServiceCollection services)
    {
        services.AddTransient<IProfileService, ProfileService>();

        services
            .AddIdentityServer()
            .AddAspNetIdentity<ApplicationUser>()
            .AddDeveloperSigningCredential()
            .AddInMemoryApiResources(GetApiResources())
            .AddInMemoryIdentityResources(GetIdentityResources())
            .AddInMemoryApiScopes(GetApiScopes())
            .AddInMemoryClients(GetClients())
            .AddProfileService<ProfileService>();
        ;
    }

    private static IEnumerable<ApiResource> GetApiResources() =>
    [
        new ApiResource("api1", "API")
        {
            UserClaims =
            {
                JwtClaimTypes.Email,
                JwtClaimTypes.Role,
            },
        },
    ];

    private static IEnumerable<ApiScope> GetApiScopes() =>
    [
        new ApiScope(name: "api1", displayName: "Default API scope"),
    ];

    private static IEnumerable<Client> GetClients() =>
    [
        new Client
        {
            AllowAccessTokensViaBrowser = true,
            AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
            AllowedScopes =
            {
                "api1",
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "user",
            },
            AlwaysSendClientClaims = true,
            AlwaysIncludeUserClaimsInIdToken = true,
            ClientId = "client",
            ClientName = "Client for Postman",
            ClientSecrets =
            {
                new Secret(DEFAULT_PASSWORD.Sha256()),
            },
        },
        new Client
        {
            AllowAccessTokensViaBrowser = true,
            AllowedCorsOrigins =
            {
                API_BASE_URI,
                WAF_BASE_URI,
            },
            AllowedGrantTypes = GrantTypes.Code,
            AllowedScopes =
            {
                "api1",
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "user",
            },
            AlwaysIncludeUserClaimsInIdToken = false,
            AlwaysSendClientClaims = true,
            ClientId = "bff",
            ClientName = "Client for BFF",
            ClientSecrets =
            {
                new Secret(DEFAULT_PASSWORD.Sha256()),
            },
            FrontChannelLogoutUri = $"{WAF_BASE_URI}/logout",
            // FrontChannelLogoutUri = $"{WAF_BASE_URI}/signout-oidc",
            PostLogoutRedirectUris =
            {
                $"{WAF_BASE_URI}/signout-callback-oidc",
            },
            RedirectUris =
            {
                $"{WAF_BASE_URI}/signin-oidc",
            },
        },
        new Client
        {
            AllowAccessTokensViaBrowser = true,
            AllowedCorsOrigins =
            {
                API_BASE_URI,
            },
            AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
            AllowedScopes =
            {
                "api1",
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "user",
            },
            AlwaysSendClientClaims = true,
            AlwaysIncludeUserClaimsInIdToken = true,
            ClientId = "swagger",
            ClientName = "Client for Swagger",
            ClientSecrets =
            {
                new Secret(DEFAULT_PASSWORD.Sha256()),
            },
            RedirectUris =
            {
                $"{API_BASE_URI}/swagger/oauth2-redirect.html",
                $"{WAF_BASE_URI}/api/swagger/oauth2-redirect.html",
            },
        }
    ];

    private static IEnumerable<IdentityResource> GetIdentityResources() =>
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResource(name: "user", userClaims:
        [
            JwtClaimTypes.Email,
        ]),
    ];
}
