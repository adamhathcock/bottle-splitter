using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BottleSplitter.Model;
using Microsoft.EntityFrameworkCore;
using Narochno.Primitives;

namespace BottleSplitter.Services;

public interface ISplitManager
{
    ValueTask<List<BottleSplit>> GetSplits(Guid userId);
    ValueTask CreateSplit(Guid currentUserId, BottleSplit split);
}

public class SplitManager(IDbContextFactory<SplitterDbContext> dbContextFactory) : ISplitManager
{
    public async ValueTask<List<BottleSplit>> GetSplits(Guid userId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Splits.Where(x => x.Owner.Id == userId).ToListAsync();
    }

    public async ValueTask CreateSplit(Guid currentUserId, BottleSplit split)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var user = await context.Users.FindAsync(currentUserId).NotNull();
        split.Owner = user;
        context.Splits.Add(split);
        await context.SaveChangesAsync();
    }
}
