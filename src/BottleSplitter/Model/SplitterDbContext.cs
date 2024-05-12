using Microsoft.EntityFrameworkCore;

namespace BottleSplitter.Model;

public class SplitterDbContext(DbContextOptions<SplitterDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<SplitterUser>()
            .HasIndex(u => u.Email);

    public DbSet<SplitterUser> Users { get; set; }
    public DbSet<BottleSplit> Splits { get; set; }
    public DbSet<BottleSplitMembers> SplitMembers { get; set; }
}
