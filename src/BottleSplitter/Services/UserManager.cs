using System.Threading.Tasks;
using BottleSplitter.Model;
using Microsoft.EntityFrameworkCore;

namespace BottleSplitter.Services;

public interface IUserManager
{
    ValueTask SaveIfNotFound(SplitterUser splitterUser);
}

public class UserManager(IDbContextFactory<SplitterDbContext> dbContextFactory) : IUserManager
{
    public async ValueTask SaveIfNotFound(SplitterUser splitterUser)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var existingUser = await context.Users.FirstOrDefaultAsync(x =>
            x.Email == splitterUser.Email
        );
        if (existingUser is null)
        {
            context.Users.Add(splitterUser);
            await context.SaveChangesAsync();
        }
    }
}
