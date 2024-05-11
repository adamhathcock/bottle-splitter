using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BottleSplitter.Endpoints;

public static class Authentication
{
    public static void AddBottleSplitterAuth(
        this IServiceCollection services,
        IConfiguration configuration
    ) =>
        services
            .AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(o =>
            {
                o.LogoutPath = "/signout";
            })
            .AddGithub(configuration);
}
