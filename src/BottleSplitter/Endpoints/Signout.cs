using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;

namespace BottleSplitter.Endpoints;

public static class Signout
{
    public static void AddSignout(this WebApplication endpoints) =>
        endpoints.MapGet(
            "/signout",
            async ctx =>
            {
                await ctx.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new AuthenticationProperties { RedirectUri = "/" }
                );
            }
        );
}
