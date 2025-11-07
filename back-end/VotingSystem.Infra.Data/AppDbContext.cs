using Microsoft.EntityFrameworkCore;
using VotingSystem.Domain.Entities;
using VotingSystem.Data.Mappings;
using VotingSystem.Infra.Data;

namespace VotingSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Participant> Participants { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                                .Property("Id")
                                .HasDefaultValueSql("uuid_generate_v4()");

                    modelBuilder.Entity(entityType.ClrType)
                                .Property("CreatedAt")
                                .HasDefaultValueSql("CURRENT_TIMESTAMP");
                }
            }

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.HasPostgresExtension("uuid-ossp");

            // SeedData
            modelBuilder.Entity<Participant>().HasData(DbSeed.GetParticipants());
        }

    }
}
