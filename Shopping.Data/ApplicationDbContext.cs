using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Models;

namespace Shopping.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CustomUser> customUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CustomUser>().HasData
            (
                 new CustomUser() { Id = 1, Email = "omer@gmail.com", Password = "password", City = "Baki", UserName = "omer" },
                 new CustomUser() { Id = 2, Email = "amin@gmail.com", Password = "password", City = "Gence", UserName = "amin" }
            );
            base.OnModelCreating(builder);
        }
    }
}
