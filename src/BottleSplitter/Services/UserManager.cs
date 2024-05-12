using System.Threading.Tasks;
using BottleSplitter.Model;
using Microsoft.EntityFrameworkCore;

namespace BottleSplitter.Services;

public interface IUserManager
{
    ValueTask SaveIfNotFound(SplitterUser splitterUser);
    ValueTask<SplitterUser?> GetUserByEmail(string email);
}

public class UserManager(IDbContextFactory<SplitterDbContext> dbContextFactory) : IUserManager
{
    public async ValueTask SaveIfNotFound(SplitterUser splitterUser)
    {
        var existingUser = await GetUserByEmail(splitterUser.Email);
        if (existingUser is null)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();
            context.Users.Add(splitterUser);
            await context.SaveChangesAsync();
        }
    }

    public async ValueTask<SplitterUser?> GetUserByEmail(string email)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}
