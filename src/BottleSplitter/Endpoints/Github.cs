using System;
using System.Linq;
using AspNet.Security.OAuth.GitHub;
using BottleSplitter.Infrastructure;
using BottleSplitter.Model;
using BottleSplitter.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Narochno.Primitives;
using Octokit;

namespace BottleSplitter.Endpoints;

public static class Github
{
    public static void UseGithub(this WebApplication application) =>
        application.MapGet(
            "challenge/github",
            () =>
                Results.Challenge(
                    properties: new() { RedirectUri = "/" },
                    authenticationSchemes: [GitHubAuthenticationDefaults.AuthenticationScheme]
                )
        );

    public static void AddGithub(
        this AuthenticationBuilder builder,
        IConfiguration configuration
    ) =>
        builder.AddGitHub(o =>
        {
            o.ClientId = configuration["OAUTH_GITHUB_ID"].NotNull();
            o.ClientSecret = configuration["OAUTH_GITHUB_SECRET"].NotNull();
            o.CallbackPath = "/callback/github";
            o.Scope.Add("emails:read");

            // Optional
            // if you need an access token to call GitHub Apis
            o.Events.OnCreatingTicket += async context =>
            {
                if (context.Identity is null)
                {
                    throw new InvalidOperationException();
                }
                if (context.AccessToken is not null)
                {
                    context.Identity.SetAccessToken(context.AccessToken);
                }
                if (string.IsNullOrEmpty(context.Identity.GetEmail()))
                {
                    var client = new GitHubClient(new ProductHeaderValue(Consts.ClientId));
                    client.Credentials = new Credentials(context.AccessToken);
                    var emails = await client.User.Email.GetAll();
                    context.Identity.SetEmail(emails.First().Email);
                }

                var email = context.Identity.GetEmail().NotNull();
                var id = await context
                    .HttpContext.RequestServices.GetRequiredService<IUserManager>()
                    .SaveIfNotFound(
                        new SplitterUser() { Email = email, Source = UserSource.Github }
                    );
                context.Identity.SetId(id);
            };
        });
}
