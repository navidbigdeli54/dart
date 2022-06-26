using Core.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Core.EF
{
    public class EFContext : DbContext
    {
        #region Properties
        public DbSet<User> Users { get; set; }

        public DbSet<GameSeason> GameSeasons { get; set; }

        public DbSet<Score> Scores { get; set; }

        public DbSet<Leaderboard> Leaderboard { get; set; }
        #endregion

        #region Protected Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Surely we should not put connection string in the app!
            optionsBuilder.UseNpgsql("Host=localhost;Database=DartDB;Username=postgres;Password=abcd1234");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().ToTable("tblUser");

            modelBuilder.Entity<GameSeason>().HasKey(x => x.Id);
            modelBuilder.Entity<GameSeason>().ToTable("tblGameSeason");
            modelBuilder.Entity<GameSeason>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<GameSeason>(x => x.UserId)
                .IsRequired();

            modelBuilder.Entity<Score>().HasKey(x => x.Id);
            modelBuilder.Entity<Score>().ToTable("tblScore");
            modelBuilder.Entity<Score>()
                .HasOne<GameSeason>()
                .WithMany()
                .HasForeignKey(x => x.GameSeasonId)
                .IsRequired();

            modelBuilder.Entity<Leaderboard>().HasKey(x => x.Id);
            modelBuilder.Entity<Leaderboard>().ToTable("tblLeaderboard");
            modelBuilder.Entity<Leaderboard>()
                .HasOne<GameSeason>()
                .WithOne()
                .HasForeignKey<Leaderboard>(x => x.GameSeasonId)
                .IsRequired();
        }
        #endregion
    }
}