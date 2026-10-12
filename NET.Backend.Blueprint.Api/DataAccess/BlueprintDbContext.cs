using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Entities.Base;

namespace NET.Backend.Blueprint.Api.DataAccess;

public class BlueprintDbContext(DbContextOptions options) : DbContext(options)
{
    private const int StringLength = 200;
    public DbSet<Person> Persons { get; set; }
    public DbSet<Address> Addresses { get; set; } 

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Person>()
            .HasMany(x => x.Addresses)
            .WithOne(x => x.Person)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Person>().Property(p => p.FirstName).HasMaxLength(StringLength);
        builder.Entity<Person>().Property(p => p.LastName).HasMaxLength(StringLength);
        builder.Entity<Address>().Property(p => p.Street).HasMaxLength(StringLength);
        builder.Entity<Address>().Property(p => p.Number).HasMaxLength(StringLength);
        builder.Entity<Address>().Property(p => p.City).HasMaxLength(StringLength);
        builder.Entity<Address>().Property(p => p.PostalCode).HasMaxLength(StringLength);

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