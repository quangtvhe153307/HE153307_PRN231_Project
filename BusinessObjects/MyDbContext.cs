using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        public DbSet<MoviePurchased> MoviePurchaseds { get; set; }
        public DbSet<MovieRated> MovieRateds { get; set; }
        public DbSet<MovieSeason> MovieSeasons { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>()
                .HasOne(b => b.User)
                .WithMany(c => c.RefreshTokens)
                .HasForeignKey(b => b.UserId);
            modelBuilder.Entity<Comment>()
                    .HasKey(c => new { c.MovieId, c.UserId, c.CommentedDate });            
            modelBuilder.Entity<MoviePurchased>()
                    .HasKey(c => new { c.MovieId, c.UserId});
            modelBuilder.Entity<MovieRated>()
                    .HasKey(c => new { c.MovieId, c.UserId});

        }
    }
}