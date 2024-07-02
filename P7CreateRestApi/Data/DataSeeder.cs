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

        public static async Task SeedAdminRoles(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Vérifiez l'existence de l'utilisateur admin
            User userAdmin = await userManager.FindByNameAsync("admin@email.com");
            string[] roleNames = { "Admin", "User", "Trader" };

            var result = await userManager.AddToRolesAsync(userAdmin, roleNames);
            if (result.Succeeded)
            {
                Console.WriteLine("Les rôles ont été assignés avec succès.");
            }
            else
            {
                Console.WriteLine("Erreur lors de l'assignation des rôles :");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Description}");
                }
            }
        }
    }
}