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
            modelBuilder.Entity<TennisPlayer>(entity =>
                {
                    entity.HasKey(p => new { p.Id });
                    entity.Property<string>(player => player.FirstName).IsRequired();
                    entity.Property<string>(player => player.LastName).IsRequired();
                });

            modelBuilder.Entity<GameScores>()
                .HasKey(p => new { p.Id });
            modelBuilder.Entity<Game>()
                .HasKey(p => new { p.Id });

            modelBuilder.Entity<TennisSet>(entity =>
            {
                entity.HasKey(p => new { p.Id });
                entity.HasMany<Game>(set => set.Games);
            });

            modelBuilder.Entity<TennisMatch>(entity =>
            {
                entity.HasKey(p => new { p.Id });
                entity.HasMany<TennisSet>(match => match.Sets);
            });

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
