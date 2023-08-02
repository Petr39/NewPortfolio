using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewPortfolio.Models;


namespace NewPortfolio.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Article>? Article { get; set; }

        public DbSet<BuildPost> BuildPosts {get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Build> Builds { get; set; }

        public DbSet<ArticlePost> ArticlePosts { get; set; }  

        public DbSet<Message> Messages { get; set; }

        public DbSet<Game> Games { get; set; }  

        public DbSet<Genre> Genres { get; set; }


    } 
}