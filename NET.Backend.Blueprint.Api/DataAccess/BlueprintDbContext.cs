using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Entities;

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
}