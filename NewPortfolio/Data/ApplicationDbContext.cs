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
    
        public DbSet<Avatar> Avatars { get; set; }
      



    }
}