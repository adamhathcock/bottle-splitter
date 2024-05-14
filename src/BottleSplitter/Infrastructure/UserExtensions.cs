using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Narochno.Primitives;

namespace BottleSplitter.Infrastructure;

public static class UserExtensions
{
    public const string AccessToken = "access_token";
    public const string Id = "split_id";

    public static string? GetEmail(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.Email);

    public static string? GetName(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.Name);

    public static string? GetNameIdentifier(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.NameIdentifier);

    public static Guid? GetId(this ClaimsPrincipal user)
    {
        var s = user.FindFirstValue(Id);
        if (s is null)
        {
            return null;
        }

        return Guid.Parse(s);
    }

    public static string? GetClaim(this ClaimsIdentity user, string type) =>
        user.Claims.FirstOrDefault(x => x.Type == type)?.Value;

    public static string GetClaim(this ClaimsPrincipal user, string type) =>
        (user.Claims.FirstOrDefault(x => x.Type == type)?.Value).NotNull();

    public static ClaimsIdentity SetClaim(this ClaimsIdentity user, string name, string value)
    {
        user.AddClaim(new Claim(name, value));
        return user;
    }

    public static string? GetEmail(this ClaimsIdentity user) => user.GetClaim(ClaimTypes.Email);

    public static void SetEmail(this ClaimsIdentity user, string val) =>
        user.SetClaim(ClaimTypes.Email, val);

    public static void SetId(this ClaimsIdentity user, Guid id) => user.SetClaim(Id, id.ToString());

    public static string? GetAccessToken(this ClaimsIdentity user) => user.GetClaim(AccessToken);

    public static void SetAccessToken(this ClaimsIdentity user, string token) =>
        user.SetClaim(AccessToken, token);
}

public static class HttpContextExtensions
{
    public static async Task<AuthenticationScheme[]> GetExternalProvidersAsync(
        this HttpContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context);

        var schemes = context.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();

        return (
            from scheme in await schemes.GetAllSchemesAsync()
            where !string.IsNullOrEmpty(scheme.DisplayName)
            select scheme
        ).ToArray();
    }

    public static async Task<bool> IsProviderSupportedAsync(
        this HttpContext context,
        string provider
    )
    {
        ArgumentNullException.ThrowIfNull(context);

        return (
            from scheme in await context.GetExternalProvidersAsync()
            where string.Equals(scheme.Name, provider, StringComparison.OrdinalIgnoreCase)
            select scheme
        ).Any();
    }
}
