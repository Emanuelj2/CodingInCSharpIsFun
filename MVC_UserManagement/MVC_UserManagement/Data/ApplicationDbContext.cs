using Microsoft.EntityFrameworkCore;
using MVC_UserManagement.Models;
using System.Runtime.CompilerServices;

namespace MVC_UserManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<UserModel> Users { get; set; } = null!;

        //seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserModel>().HasData(
                new UserModel
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "exmaple@example.com",
                    Password = "Password123"
                }
             );
        }
    }
}
