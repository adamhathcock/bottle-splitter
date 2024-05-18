using System;
using System.Threading.Tasks;
using BottleSplitter.Model;
using Microsoft.EntityFrameworkCore;
using Speckle.InterfaceGenerator;

namespace BottleSplitter.Services;

[GenerateAutoInterface]
public class UserManager(IDbContextFactory<SplitterDbContext> dbContextFactory) : IUserManager
{
    public async ValueTask<Guid> SaveIfNotFound(SplitterUser splitterUser)
    {
        var existingUser = await GetUserByEmail(splitterUser.Email);
        if (existingUser is null)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();
            context.Users.Add(splitterUser);
            await context.SaveChangesAsync();
            existingUser = splitterUser;
        }
        return existingUser.Id;
    }

    public async ValueTask<SplitterUser?> GetUserByEmail(string email)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}
