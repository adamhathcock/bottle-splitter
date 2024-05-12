using System.Threading.Tasks;
using BottleSplitter.Model;
using Microsoft.EntityFrameworkCore;

namespace BottleSplitter.Services;

public interface ISplitManager
{
    ValueTask CreateSplit(BottleSplit split);
}

public class SplitManager(IDbContextFactory<SplitterDbContext> dbContextFactory) : ISplitManager
{
    public async ValueTask CreateSplit(BottleSplit split)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        context.Splits.Add(split);
        await context.SaveChangesAsync();
    }
}
