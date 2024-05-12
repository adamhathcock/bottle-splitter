using Microsoft.EntityFrameworkCore;

namespace BottleSplitter.Model;

public class SplitterDbContext : DbContext
{
    public DbSet<SplitterUser> Users { get; set; }
}
