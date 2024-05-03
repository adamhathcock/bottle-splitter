using System;
using BottleSplitter.Components;
using BottleSplitter.Endpoints;
using BottleSplitter.Infrastructure;
using BottleSplitter.Model;
using BottleSplitter.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using Narochno.EnvFile;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvFile();

// Add the database (in memory for the sample)
builder.Services.AddDbContext<SplitterDbContext>(options =>
{
    options.UseSqlite("Data Source=local.db");
    options.UseOpenIddict();
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
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(50);
        options.SlidingExpiration = false;
    });
builder.AddOpenIddictSettings();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IThemeService, ThemeService>();
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
app.UseGithub();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode()
    .AllowAnonymous().RequireAuthorization();

app.Run();
