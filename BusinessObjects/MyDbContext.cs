using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace BusinessObjects
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("PRN231Project"));
        }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieEpisode> MovieEpisodes { get; set; }
        public DbSet<PurchasedMovie> PurchasedMovies { get; set; }
        public DbSet<MovieRated> MovieRateds { get; set; }
        public DbSet<MovieSeason> MovieSeasons { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<MovieView> MovieViews { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>()
                .HasOne(b => b.User)
                .WithMany(c => c.RefreshTokens)
                .HasForeignKey(b => b.UserId);
            modelBuilder.Entity<Comment>()
                    .HasKey(c => new { c.MovieId, c.UserId, c.CommentedDate });            
            modelBuilder.Entity<PurchasedMovie>()
                    .HasKey(c => new { c.MovieId, c.UserId});
            modelBuilder.Entity<MovieRated>()
                    .HasKey(c => new { c.MovieId, c.UserId});
            modelBuilder.Entity<MovieView>()
                    .HasKey(c => new { c.EpisodeId, c.UserId, c.ViewedDate});

            //add unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(t => t.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ModifiedUser)
                .WithMany()
                .HasForeignKey(t => t.ModifiedBy)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.CreatedUser)
                .WithMany()
                .HasForeignKey(t => t.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MovieView>()
                .HasOne(m => m.MovieEpisode)
                .WithMany(m => m.MovieViews)
                .HasForeignKey(m => m.EpisodeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<MovieView>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}