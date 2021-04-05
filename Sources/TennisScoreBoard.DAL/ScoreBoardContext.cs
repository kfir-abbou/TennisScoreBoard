using Microsoft.EntityFrameworkCore;
using TennisScoreBoard.EF.Model;

namespace TennisScoreBoard.EF
{
    public sealed class ScoreBoardContext : DbContext
    {

        public ScoreBoardContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<TennisPlayer> Players { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<GameScores> GameScores { get; set; }
        public DbSet<TennisSet> TennisSet { get; set; }
        public DbSet<TennisMatch> TennisMatch { get; set; }

        public void SaveData()
        {
            this.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=tennisScoreboard.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TennisPlayer>()
                .HasKey(p => new { p.Id });

            modelBuilder.Entity<GameScores>()
                .HasKey(p => new { p.Id });

            modelBuilder.Entity<Game>()
                .HasKey(p => new { p.Id });

            modelBuilder.Entity<TennisSet>(entity =>
            {
                entity.HasKey(p => new { p.Id });
                entity.HasMany(set => set.Games);
            });

            modelBuilder.Entity<TennisMatch>(entity =>
            {
                entity.HasKey(p => new { p.Id });
                entity.HasMany(match => match.Sets);
            });
        }
    }
    
}
