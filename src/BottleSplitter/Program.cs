using System;
using BottleSplitter.Components;
using BottleSplitter.Endpoints;
using BottleSplitter.Infrastructure;
using BottleSplitter.Model;
using BottleSplitter.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using Narochno.EnvFile;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateSlimBuilder(args);
builder.WebHost.ConfigureAppConfiguration(
    (ctx, c) =>
    {
        StaticWebAssetsLoader.UseStaticWebAssets(ctx.HostingEnvironment, ctx.Configuration);
    }
);
builder.Configuration.AddEnvFile();

// Add the database (in memory for the sample)
builder.Services.AddDbContext<SplitterDbContext>(options =>
{
    options.UseSqlite("Data Source=local.db");
    //For debugging only: options.EnableDetailedErrors(true);
    //For debugging only: options.EnableSensitiveDataLogging(true);
});

// Add services to the container.
builder
    .Services.AddRazorComponents()
    .AddInteractiveServerComponents(x =>
    {
        x.DetailedErrors = true;
        x.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(4);
    });
builder.Services.AddMudServices();

builder.Services.AddHealthChecks();
builder.Services.AddSerilog(
    (services, lc) =>
        lc
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console()
);

builder.Services.AddAuthorization();
builder.Services.AddBottleSplitterAuth(builder.Configuration);

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<CustomAuthenticationStateProvider>()
);
builder.Services.AddScoped<IPreferencesService, PreferencesService>();

builder.Services.AddScoped<ISessionManager, SessionManager>();
builder.Services.TryAddEnumerable(ServiceDescriptor.Scoped<CircuitHandler, UserCircuitHandler>());
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
app.UseGithub();

app.MapHealthChecks("/health");
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
