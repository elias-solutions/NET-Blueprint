using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Entities;

namespace NET.Backend.Blueprint.Api.DataAccess
{
    public class BlueprintDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; } 

        public BlueprintDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasMany(x => x.Addresses)
                .WithOne(x => x.Person)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
