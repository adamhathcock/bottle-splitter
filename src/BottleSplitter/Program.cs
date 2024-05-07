using System;
using BottleSplitter.Components;
using BottleSplitter.Endpoints;
using BottleSplitter.Infrastructure;
using BottleSplitter.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using Narochno.EnvFile;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvFile();

// Add the database (in memory for the sample)
builder.Services.AddDbContext<SplitterDbContext>(options =>
{
    options.UseSqlite("Data Source=local.db");
    //For debugging only: options.EnableDetailedErrors(true);
    //For debugging only: options.EnableSensitiveDataLogging(true);
});

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents(x =>
{
    x.DetailedErrors = true;
    x.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(4);
});
builder.Services.AddMudServices();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

}).AddCookie(o =>
{
    // set the path for the authentication challenge
    o.LoginPath = "/signin";
    // set the path for the sign out
    o.LogoutPath = "/signout";
}).AddGithub(builder.Configuration);

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped<ISessionManager, SessionManager>();
builder.Services.TryAddEnumerable(
    ServiceDescriptor.Scoped<CircuitHandler, UserCircuitHandler>());
builder.WebHost.UseStaticWebAssets();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    await using var scope = app.Services.CreateAsyncScope();
    await Seeder.InitializeAsync(scope.ServiceProvider);
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.AddSignout();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
