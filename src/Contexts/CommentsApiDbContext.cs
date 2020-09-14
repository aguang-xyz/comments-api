using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CommentsApi.Entities;

namespace CommentsApi.Contexts
{
    public class CommentsApiDbContext : DbContext
    {
        private ILogger<CommentsApiDbContext> _logger;

        private IConfiguration _configuration;

        public CommentsApiDbContext(ILogger<CommentsApiDbContext> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // The email of user is unique.
            modelBuilder.Entity<User>()
              .HasIndex(user => user.Email)
              .IsUnique();

            // Query comments by category order by created time desc.
            modelBuilder.Entity<Comment>()
              .HasIndex(comment => new { comment.Category, comment.CreatedAt });

            // Query comments by category order by the nuber of likes desc.
            modelBuilder.Entity<Comment>()
              .HasIndex(comment => new { comment.Category, comment.CountOfLikes });

            // One user can only like one comment for at most one time.
            modelBuilder.Entity<Like>()
              .HasIndex(like => new { like.UserId, like.CommentId })
              .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Parse DbContext configurations.
            var server = _configuration["DbContext:Server"] ?? "localhost";
            var database = _configuration["DbContext:Database"] ?? "comments";
            var port = _configuration["DbContext:Port"] ?? "3306";
            var user = _configuration["DbContext:User"] ?? "root";
            var password = _configuration["DbContext:Password"] ?? "";

            // Construct the connection string;
            var connStr = $"server={server};port={port};database='{database}';user='{user}';password='{password}'";

            _logger.LogInformation($"UseMySql: {connStr}");

            // Setup mysql configuration.
            options.UseMySql(connStr);
        }
    }
}

