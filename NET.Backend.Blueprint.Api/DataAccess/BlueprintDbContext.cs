using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Entities.Base;

namespace NET.Backend.Blueprint.Api.DataAccess;

public class BlueprintDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Address> Addresses { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasMany(x => x.Addresses)
            .WithOne(x => x.Person)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        AssignNewVersions();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void AssignNewVersions()
    {
        var changedEntries = ChangeTracker.Entries<EntityBase>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in changedEntries)
        {
            entry.Entity.Version = Guid.NewGuid();
        }
    }
}