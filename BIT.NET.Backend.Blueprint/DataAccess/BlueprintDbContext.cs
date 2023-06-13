using BIT.NET.Backend.Blueprint.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BIT.NET.Backend.Blueprint.DataAccess
{
    public class BlueprintDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public BlueprintDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Database"));

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
