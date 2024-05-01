using System.Reflection;
using BottleSplitter.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BottleSplitter.Endpoints;

public static class OpenIddict
{
    public static void AddOpenIddictSettings(this WebApplicationBuilder builder) =>
        builder
            .Services.AddOpenIddict() // Register the OpenIddict core components.
            .AddCore(options =>
            {
                // Configure OpenIddict to use the Entity Framework Core stores and models.
                options.UseEntityFrameworkCore().UseDbContext<SplitterDbContext>();
            })
            // Register the OpenIddict client components.
            .AddClient(options =>
            {
                // Allow the OpenIddict client to negotiate the authorization code flow.
                options.AllowAuthorizationCodeFlow();

                // Register the signing and encryption credentials used to protect
                // sensitive data like the state tokens produced by OpenIddict.
                options.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();

                // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                options
                    .UseAspNetCore()
                    .EnableStatusCodePagesIntegration()
                    .EnableRedirectionEndpointPassthrough()
                    .EnablePostLogoutRedirectionEndpointPassthrough()
                    .DisableTransportSecurityRequirement();

                options.UseSystemNetHttp().SetProductInformation(Assembly.GetExecutingAssembly());

                // Register the GitHub integration.
                options
                    .UseWebProviders()
                    .AddGitHub(o =>
                    {
                        o.SetClientId(builder.Configuration["GITHUB_ID"] ?? "[your client id]")
                            .SetClientSecret(
                                builder.Configuration["GITHUB_SECRET"] ?? "[your client id]"
                            )
                            .SetRedirectUri("callback/github")
                            .AddScopes("emails:read");
                    });
            });
}
