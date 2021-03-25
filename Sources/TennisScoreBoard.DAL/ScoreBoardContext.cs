using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace TennisScoreBoard.EF
{
    public sealed class ScoreBoardContext : DbContext, IScoreBoardContext
    {
        public ScoreBoardContext(DbContextOptions<ScoreBoardContext> options) : base(options)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TennisPlayer>()
                .HasKey(p => new { p.Id });
            modelBuilder.Entity<GameScores>()
                .HasKey(p => new { p.Id });
            modelBuilder.Entity<Game>()
                .HasKey(p => new { p.Id });
            modelBuilder.Entity<TennisSet>()
                .HasKey(p => new { p.Id });
            modelBuilder.Entity<TennisMatch>()
                .HasKey(p => new { p.Id });
        }
    }

    public interface IScoreBoardContext
    {
        DbSet<TennisPlayer> Players { get; }
        DbSet<Game> Game { get; }
        DbSet<GameScores> GameScores { get; }
        DbSet<TennisSet> TennisSet { get; }
        DbSet<TennisMatch> TennisMatch { get; }
        void SaveData();

    }
}
