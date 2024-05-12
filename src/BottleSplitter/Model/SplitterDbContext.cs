using Microsoft.EntityFrameworkCore;

namespace BottleSplitter.Model;

public class SplitterDbContext(DbContextOptions<SplitterDbContext> options) : DbContext(options)
{
    public DbSet<SplitterUser> Users { get; set; }
}
