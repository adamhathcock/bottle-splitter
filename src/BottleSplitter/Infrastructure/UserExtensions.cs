using System.Security.Claims;
using OpenIddict.Abstractions;

namespace BottleSplitter.Infrastructure;

public static class UserExtensions
{
    public const string AccessToken = "access_token";
    public static string? GetEmail(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.Email);

    public static string? GetName(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.Name);

    public static string? GetNameIdentifier(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.NameIdentifier);

    public static string? GetEmail(this ClaimsIdentity user) => user.GetClaim(ClaimTypes.Email);

    public static string? GetAccessToken(this ClaimsPrincipal user) => user.FindFirstValue(AccessToken);
    public static void SetToken(this ClaimsIdentity user, string token) => user.SetClaim(AccessToken, token);
}
