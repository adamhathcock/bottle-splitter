using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Narochno.Primitives;

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
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(o =>
            {
                o.LoginPath = "/";
                o.LogoutPath = "/signout";
            })
            .AddGoogle(g =>
            {
                g.ClientId = configuration["OAUTH_GOOGLE_ID"].NotNull();
                g.ClientSecret = configuration["OAUTH_GOOGLE_SECRET"].NotNull();
                g.CallbackPath = "/callback/google";
            })
            .AddGithub(configuration);
}
