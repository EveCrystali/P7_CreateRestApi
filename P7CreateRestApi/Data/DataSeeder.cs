using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;

namespace Dot.Net.WebApi.Data
{
    [LogAspect]
    public static class DataSeeder
    {
        public static async Task SeedAdmin(UserManager<User> userManager)
        {
            User? user = await userManager.FindByEmailAsync("admin@email.com");
            if (user == null)
            {
                User newUser = new() { UserName = "admin@email.com", Email = "admin@email.com", Fullname = "Admin", EmailConfirmed = true, LockoutEnabled = false };
                await userManager.CreateAsync(newUser, "9vBZBB.QH83GeE.");
            }
        }

        [LogAspect]
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User", "Trader" };

            foreach (string roleName in roleNames)
            {
                bool roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}