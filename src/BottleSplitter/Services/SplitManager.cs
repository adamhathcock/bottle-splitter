using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BottleSplitter.Model;
using Microsoft.EntityFrameworkCore;
using Narochno.Primitives;
using Speckle.InterfaceGenerator;

namespace BottleSplitter.Services;

[GenerateAutoInterface]
public class SplitManager(
    IDbContextFactory<SplitterDbContext> dbContextFactory,
    ISquidGenerator squidGenerator
) : ISplitManager
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
        var id = Guid.NewGuid();
        split.Id = id;
        split.Owner = user;
        split.Squid = squidGenerator.GetSquid();
        context.Splits.Add(split);
        await context.SaveChangesAsync();
    }

    public async ValueTask<BottleSplit?> GetSplitBySquid(string squid)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Splits.FirstOrDefaultAsync(x => x.Squid == squid);
    }
}
