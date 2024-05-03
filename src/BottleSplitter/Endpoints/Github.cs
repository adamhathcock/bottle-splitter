using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BottleSplitter.Infrastructure;
using BottleSplitter.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Narochno.Primitives;
using Octokit;
using OpenIddict.Abstractions;
using OpenIddict.Client.AspNetCore;
using OpenIddict.Client.WebIntegration;

namespace BottleSplitter.Endpoints;

public static class Github
{
    public static void UseGithub(this WebApplication app)
    {
        app.MapGet(
            "challenge/github",
            () =>
                Results.Challenge(
                    properties: null,
                    authenticationSchemes:
                    [
                        OpenIddictClientWebIntegrationConstants.Providers.GitHub
                    ]
                )
        );

        app.MapMethods(
            "callback/github",
            [HttpMethods.Get, HttpMethods.Post],
            async (HttpContext context) =>
            {
                // Retrieve the authorization data validated by OpenIddict as part of the callback handling.
                var result = await context.AuthenticateAsync(
                    OpenIddictClientWebIntegrationConstants.Providers.GitHub
                );

                // Build an identity based on the external claims and that will be used to create the authentication cookie.
                var identity = new ClaimsIdentity(authenticationType: CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = result.Principal;
                if (principal is null)
                {
                    return Results.Unauthorized();
                }

                identity
                    .SetClaim(ClaimTypes.Email, principal.GetClaim(ClaimTypes.Email))
                    .SetClaim(ClaimTypes.Name, principal.GetClaim(ClaimTypes.Name))
                    .SetClaim(
                        ClaimTypes.NameIdentifier,
                        principal.GetClaim(ClaimTypes.NameIdentifier)
                    );

                // Preserve the registration details to be able to resolve them later.
                identity
                    .SetClaim(
                        OpenIddictConstants.Claims.Private.RegistrationId,
                        principal.GetClaim(OpenIddictConstants.Claims.Private.RegistrationId)
                    )
                    .SetClaim(
                        OpenIddictConstants.Claims.Private.ProviderName,
                        principal.GetClaim(OpenIddictConstants.Claims.Private.ProviderName)
                    );

                // Build the authentication properties based on the properties that were added when the challenge was triggered.
                var properties = new AuthenticationProperties(
                    result.Properties?.Items ?? new Dictionary<string, string?>()
                )
                {
                    RedirectUri = result.Properties?.RedirectUri ?? "/"
                };

                // If needed, the tokens returned by the authorization server can be stored in the authentication cookie.
                //
                // To make cookies less heavy, tokens that are not used are filtered out before creating the cookie.
                properties.StoreTokens(
                    result
                        .Properties.NotNull()
                        .GetTokens()
                        .Where(token =>
                            token.Name
                                is
                                    // Preserve the access, identity and refresh tokens returned in the token response, if available.
                                    OpenIddictClientAspNetCoreConstants
                                        .Tokens
                                        .BackchannelAccessToken
                                    or OpenIddictClientAspNetCoreConstants
                                        .Tokens
                                        .BackchannelIdentityToken
                                    or OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken
                        )
                );
                var token =  properties
                    .GetTokens()
                    .First(x =>
                        x.Name
                        == OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken
                    )
                    .Value;
                identity.SetToken(token);

                if (identity.GetEmail().IsNullOrEmpty())
                {
                    var client = new GitHubClient(new ProductHeaderValue(Consts.ClientId));
                    client.Credentials = new Credentials(token);
                    var emails = await client.User.Email.GetAll();
                    identity.SetClaim(ClaimTypes.Email, emails.First().Email);
                }
                // Ask the default sign-in handler to return a new cookie and redirect the
                // user agent to the return URL stored in the authentication properties.
                //
                // For scenarios where the default sign-in handler configured in the ASP.NET Core
                // authentication options shouldn't be used, a specific scheme can be specified here.
                var user = new ClaimsPrincipal(identity);
               // var provider = context.RequestServices.GetRequiredService<CustomAuthenticationStateProvider>();
               // await provider.SetAuthenticationState(user);
                return Results.SignIn(user, properties, CookieAuthenticationDefaults.AuthenticationScheme);
            }
        );
    }
}
