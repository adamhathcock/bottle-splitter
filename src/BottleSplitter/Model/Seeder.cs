using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BottleSplitter.Model;

public static class Seeder
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<SplitterDbContext>();
        await context.Database.EnsureCreatedAsync();
        await context.SaveChangesAsync();
    }
}
