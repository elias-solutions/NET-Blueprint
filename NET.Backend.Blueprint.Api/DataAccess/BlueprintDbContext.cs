using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Authorization;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Entities.Base;
using NET.Backend.Blueprint.Extensions;

namespace NET.Backend.Blueprint.Api.DataAccess
{
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

        public Task<int> SaveChangesAsync(User user, CancellationToken cancellationToken = new())
        {
            AddAuditInformation(user);
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddAuditInformation(User user)
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is EntityBase && x.State is EntityState.Added or EntityState.Modified);

            foreach (var entity in entities)
            {
                var date = DateTime.UtcNow.ToUtcDateTimeOffset();

                if (entity.State == EntityState.Modified)
                {
                    ((EntityBase)entity.Entity).Modified = date;
                    ((EntityBase)entity.Entity).ModifiedBy = user.Id;
                }

                if (entity.State == EntityState.Added)
                {
                    ((EntityBase)entity.Entity).Created = date;
                    ((EntityBase)entity.Entity).CreatedBy = user.Id;
                }

                ((EntityBase)entity.Entity).Version = Guid.NewGuid();
            }
        }
    }
}
