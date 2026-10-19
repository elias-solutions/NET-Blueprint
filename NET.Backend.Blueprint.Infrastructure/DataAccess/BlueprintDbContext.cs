using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Domain.Entities;
using NET.Backend.Blueprint.Domain.Entities.Base;

namespace NET.Backend.Blueprint.Infrastructure.DataAccess;

public class BlueprintDbContext(DbContextOptions<BlueprintDbContext> options) : DbContext(options)
{
    private const int StringLength = 200;
    private const int EnumLength = 20;

    public DbSet<Person> Persons { get; set; }

    public DbSet<Address> Addresses { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTimeOffset>().HaveConversion<UtcDateTimeOffsetConverter>();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Person>()
            .HasMany(x => x.Addresses)
            .WithOne(x => x.Person)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Person>().Property(p => p.FirstName).HasMaxLength(StringLength);
        builder.Entity<Person>().Property(p => p.LastName).HasMaxLength(StringLength);
        builder.Entity<Person>().Property(p => p.Gender).HasConversion<string>().HasMaxLength(EnumLength);
        builder.Entity<Person>().Property(p => p.Description).HasMaxLength(PersonConstraints.DescriptionMaxLength);
        builder.Entity<Person>().Property(p => p.Email).HasMaxLength(PersonConstraints.EmailMaxLength);
        builder.Entity<Person>().Property(p => p.Phone).HasMaxLength(PersonConstraints.PhoneMaxLength);
        builder.Entity<Person>().Property(p => p.Type).HasConversion<string>().HasMaxLength(EnumLength);
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
