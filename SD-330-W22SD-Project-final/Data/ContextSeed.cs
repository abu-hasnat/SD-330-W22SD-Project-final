using Microsoft.AspNetCore.Identity;

namespace SD_330_W22SD_Project_final.Data
{
    public class ContextSeed
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.EnsureCreated();
                var _userManager =
                         serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>();
                var _roleManager =
                         serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                SeedRolesAsync(_roleManager);
                SeedAdminAsync(_userManager);

                context.SaveChanges();
            }
        }
        public static void SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            
            roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
            roleManager.CreateAsync(new IdentityRole(Enums.Roles.User.ToString()));
        }

        public static void SeedAdminAsync(UserManager<IdentityUser> userManager)
        {
            
            var adminUser = new IdentityUser
            {
                UserName = "admin@test.com",
                Email = "admin@test.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != adminUser.Id))
            {
                var user = userManager.FindByEmailAsync(adminUser.Email);
                if (user == null)
                {
                    userManager.CreateAsync(adminUser, "123Test@");
                    userManager.AddToRoleAsync(adminUser, Enums.Roles.Admin.ToString());
                }

            }
            
            var users = new List<IdentityUser> {
                new IdentityUser{ UserName = "test@test.com", Email = "test@test.com", EmailConfirmed = true, PhoneNumberConfirmed = true },
                new IdentityUser{ UserName = "test2@test.com", Email = "test2@test.com", EmailConfirmed = true, PhoneNumberConfirmed = true }
            };
            foreach (var user in users)
            {
                if (userManager.Users.All(u => u.Id != user.Id))
                {
                    var user1 = userManager.FindByEmailAsync(user.Email);
                    if (user1 == null)
                    {
                        userManager.CreateAsync(user, "123Test@");
                        userManager.AddToRoleAsync(user, Enums.Roles.User.ToString());
                    }

                }
            }

        }
    }
}
