using Core.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Core.EF
{
    public class EFContext : DbContext
    {
        #region Properties
        public DbSet<User> Users { get; set; }

        public DbSet<GameSeason> GameSeasons { get; set; }

        public DbSet<LeaderboardEntry> Leaderboard { get; set; }
        #endregion

        #region Constructors
        public EFContext()
        {

        }
        #endregion

        #region Protected Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=DartDB;Username=postgres");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<GameSeason>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<GameSeason>(x=>x.UserId)
                .IsRequired();

            modelBuilder.Entity<LeaderboardEntry>()
                .HasOne<GameSeason>()
                .WithOne()
                .HasForeignKey<LeaderboardEntry>(x=>x.GameSeasonId)
                .IsRequired();
        }
        #endregion
    }
}