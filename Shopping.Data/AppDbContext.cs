using Microsoft.EntityFrameworkCore;
using Shopping.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Data
{
    public class AppDbContext: DbContext
    {

        public AppDbContext(DbContextOptions opts) : base(opts)
        {
        } 

        public DbSet<CustomUser> customUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // md5 sha256 sha512

            modelBuilder.Entity<CustomUser>().HasData
            (
                 new CustomUser() { Id = 1, Email = "omer@gmail.com", Password = "password", City = "Baki", UserName = "omer" },
                 new CustomUser() { Id = 2, Email = "amin@gmail.com", Password = "password", City = "Gence", UserName = "amin" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
