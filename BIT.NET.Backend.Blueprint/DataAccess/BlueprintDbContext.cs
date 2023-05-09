using BIT.NET.Backend.Blueprint.Entities;
using Microsoft.EntityFrameworkCore;

namespace BIT.NET.Backend.Blueprint.DataAccess
{
    public class BlueprintDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("GenericDatabase");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
