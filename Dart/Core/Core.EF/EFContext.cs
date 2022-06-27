using Core.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Core.EF
{
    public class EFContext : DbContext
    {
        #region Properties
        public DbSet<User> Users { get; set; }

        public DbSet<GameSession> GameSessions { get; set; }

        public DbSet<Score> Scores { get; set; }

        public DbSet<Leaderboard> Leaderboard { get; set; }
        #endregion

        #region Protected Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if TEST
            optionsBuilder.UseNpgsql("Host=localhost;Database=DartDBTest;Username=postgres;Password=abcd1234");
#else
            optionsBuilder.UseNpgsql("Host=localhost;Database=DartDB;Username=postgres;Password=abcd1234");
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().Ignore(x => x.IsDirty);
            modelBuilder.Entity<User>().ToTable("tblUser");

            modelBuilder.Entity<GameSession>().HasKey(x => x.Id);
            modelBuilder.Entity<GameSession>().Ignore(x => x.IsDirty);
            modelBuilder.Entity<GameSession>().ToTable("tblGameSession");
            modelBuilder.Entity<GameSession>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<GameSession>(x => x.UserId)
                .IsRequired();

            modelBuilder.Entity<Score>().HasKey(x => x.Id);
            modelBuilder.Entity<Score>().Ignore(x => x.IsDirty);
            modelBuilder.Entity<Score>().ToTable("tblScore");
            modelBuilder.Entity<Score>()
                .HasOne<GameSession>()
                .WithMany()
                .HasForeignKey(x => x.GameSessionId)
                .IsRequired();

            modelBuilder.Entity<Leaderboard>().HasKey(x => x.Id);
            modelBuilder.Entity<Leaderboard>().Ignore(x => x.IsDirty);
            modelBuilder.Entity<Leaderboard>().ToTable("tblLeaderboard");
            modelBuilder.Entity<Leaderboard>()
                .HasOne<GameSession>()
                .WithOne()
                .HasForeignKey<Leaderboard>(x => x.GameSessionId)
                .IsRequired();
        }
#endregion
    }
}