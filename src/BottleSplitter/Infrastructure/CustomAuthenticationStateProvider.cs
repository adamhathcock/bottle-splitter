using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

namespace BottleSplitter.Infrastructure;

public class CustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
    : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true)
        {
            var identity = new ClaimsIdentity(httpContextAccessor.HttpContext.User.Identity);
            var user = new ClaimsPrincipal(identity);
            return Task.FromResult(new AuthenticationState(user));
        }

        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
    }
}
