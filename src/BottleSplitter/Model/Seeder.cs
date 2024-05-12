using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BottleSplitter.Model;

public static class Seeder
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<SplitterDbContext>>();
        await using var context = await factory.CreateDbContextAsync();
        var migrations = (await context.Database.GetPendingMigrationsAsync()).ToList();
        if (migrations.Any())
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            foreach (var migration in migrations)
            {
                logger.LogWarning($"Migrating: {migration}");
            }

            await context.Database.MigrateAsync();
            await context.SaveChangesAsync();
        }
    }
}
