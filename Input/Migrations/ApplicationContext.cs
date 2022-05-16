using Input.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Input.Migrations
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
        
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Fandom> Fandoms { get; set; }
        public DbSet<FanFiction> FanFictions { get; set; }
        public DbSet<Moderation> Moderations { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }
    }
}