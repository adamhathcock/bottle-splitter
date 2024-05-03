using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Http;

namespace BottleSplitter.Infrastructure;

public sealed class UserCircuitHandler(CustomAuthenticationStateProvider authenticationStateProvider, IHttpContextAccessor httpContextAccessor) : CircuitHandler
{
    public override Task OnCircuitOpenedAsync(Circuit circuit,
        CancellationToken cancellationToken)
    {
        authenticationStateProvider.SetAuthenticationState(Task.FromResult(new AuthenticationState(httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal(new ClaimsIdentity()))));
        return Task.CompletedTask;
    }


    public override Task OnConnectionUpAsync(Circuit circuit,
        CancellationToken cancellationToken)
    {
        authenticationStateProvider.SetAuthenticationState(Task.FromResult(new AuthenticationState(httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal(new ClaimsIdentity()))));
        return Task.CompletedTask;
    }

}
