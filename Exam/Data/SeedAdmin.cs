using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Exam.Data
{
    public class SeedAdmin
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            context.Database.EnsureCreated();
            
            
            var user = new IdentityUser()
            {
                Email = "admin@admin.com",
                UserName = "admin@admin.com",
                EmailConfirmed = true
            };
            if (userManager.FindByEmailAsync(user.Email).Result == null)
            {
                IdentityResult result = await userManager.CreateAsync(user, "Admin1234_");
                if (result.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync("Admin"))
                    {
                        var role = new IdentityRole {Name = "Admin"};
                        await roleManager.CreateAsync(role);
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}