using System;
using System.Linq;
using BottleSplitter.Infrastructure;
using BottleSplitter.Model;
using BottleSplitter.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Narochno.Primitives;
using Octokit;

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
                g.Events.OnCreatingTicket += async context =>
                {
                    if (context.Identity is null)
                    {
                        throw new InvalidOperationException();
                    }

                    if (context.AccessToken is not null)
                    {
                        context.Identity.SetAccessToken(context.AccessToken);
                    }

                    var email = context.Identity.GetEmail().NotNull();
                    var id = await context
                        .HttpContext.RequestServices.GetRequiredService<IUserManager>()
                        .SaveIfNotFound(
                            new SplitterUser() { Email = email, Source = UserSource.Github }
                        );
                    context.Identity.SetId(id);
                };
            })
            .AddGithub(configuration);
}
