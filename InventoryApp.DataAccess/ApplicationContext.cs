using InventoryApp.Models;
using InventoryApp.Models.Users.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace InventoryApp.DataAccess
{
    public class ApplicationContext : DbContext
    {      
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new List<User>
            {
                new User {Id = Guid.Parse("9a8e2f25-b0f1-4b47-9f2f-a9d6e43b34ec"),
                    Email = "akezhan.2003@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("akezhanqwe"),
                    FullName ="Akezhan Issadilov",
                    Role = UserRole.SuperAdmin},
            });
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();


           

        }
    }
}
