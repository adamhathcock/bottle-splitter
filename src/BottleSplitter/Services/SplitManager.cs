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

    public async ValueTask SaveSplit(BottleSplit updated)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var split = await context.Splits.FindAsync(updated.Id).NotNull();
        split.Name = updated.Name;
        split.Settings = updated.Settings;
        await context.SaveChangesAsync();
    }

    public async ValueTask<BottleSplit?> GetSplitBySquid(string squid)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context
            .Splits.AsNoTracking()
            .Include(x => x.Owner)
            .Include(x => x.Members)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Squid == squid);
    }

    public async ValueTask CreateMembership(Guid splitId, Guid userId, int amount)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var split = await context.Splits.FindAsync(splitId);
        if (split is null)
        {
            throw new InvalidOperationException();
        }
        var user = await context.Users.FindAsync(userId);
        if (user is null)
        {
            throw new InvalidOperationException();
        }
        var membership = await context.Memberships.FirstOrDefaultAsync(x =>
            x.User == user && x.Split == split
        );
        if (membership is not null)
        {
            throw new InvalidOperationException();
        }

        membership = new SplitMembership()
        {
            Amount = amount,
            Split = split,
            User = user
        };
        await context.Memberships.AddAsync(membership);
        await context.SaveChangesAsync();
    }
}
