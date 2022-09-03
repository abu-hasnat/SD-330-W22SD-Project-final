using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SD_330_W22SD_Project_final.Models;

namespace SD_330_W22SD_Project_final.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        private void SeedUsers(ModelBuilder builder, string guid)
        {
            string password = "123Test@";
            PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();

            var users = new List<IdentityUser> {
                new IdentityUser{Id = guid, UserName = "admin@test.com", Email = "admin@test.com", EmailConfirmed = true, PhoneNumberConfirmed = true, LockoutEnabled = false,PasswordHash = passwordHasher.HashPassword(null, password) },
                new IdentityUser{Id = Guid.NewGuid().ToString(), UserName = "test@test.com", Email = "test@test.com", EmailConfirmed = true, PhoneNumberConfirmed = true,PasswordHash = passwordHasher.HashPassword(null, password) },
                new IdentityUser{Id = Guid.NewGuid().ToString(), UserName = "test2@test.com", Email = "test2@test.com", EmailConfirmed = true, PhoneNumberConfirmed = true,PasswordHash = passwordHasher.HashPassword(null, password) }
            };
            foreach (var user in users)
            {
                builder.Entity<IdentityUser>().HasData(user);
            }
        }

        private void SeedRoles(ModelBuilder builder, string guid)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = guid, Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" });
        }

        private void SeedUserRoles(ModelBuilder builder, string guid, string guid2)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId = guid2, UserId = guid }
                );
        }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}