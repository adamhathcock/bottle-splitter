using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BottleSplitter.Model;

public class SplitterDbContext(DbContextOptions<SplitterDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<SplitterUser>().HasIndex(u => u.Email);

    public DbSet<SplitterUser> Users { get; set; }
    public DbSet<BottleSplit> Splits { get; set; }
    public DbSet<BottleSplitMembers> SplitMembers { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var insertedEntries = ChangeTracker
            .Entries()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity);

        foreach (var insertedEntry in insertedEntries)
        {
            //If the inserted object is an Auditable.
            if (insertedEntry is Auditable auditableEntity)
            {
                auditableEntity.DateCreated = DateTimeOffset.UtcNow;
            }
        }

        var modifiedEntries = ChangeTracker
            .Entries()
            .Where(x => x.State == EntityState.Modified)
            .Select(x => x.Entity);

        foreach (var modifiedEntry in modifiedEntries)
        {
            //If the inserted object is an Auditable.
            if (modifiedEntry is Auditable auditableEntity)
            {
                auditableEntity.DateUpdated = DateTimeOffset.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public T Delete<T>(T entity)
        where T : Auditable
    {
        //If the type we are trying to delete is auditable, then we don't actually delete it but instead set it to be updated with a delete date.

        entity.DateDeleted = DateTimeOffset.UtcNow;
        Attach(entity);
        Entry(entity).State = EntityState.Modified;

        return entity;
    }
}
