using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;

namespace Dot.Net.WebApi.Data
{
    [LogAspect]
    public static class DataSeeder
    {
        public const string AdminEmail = "admin@email.com";
        public static readonly string[] roleNames = ["Admin", "User", "Trader"];

        public static async Task SeedAdmin(UserManager<User> userManager)
        {
            User? user = await userManager.FindByEmailAsync("admin@email.com");
            if (user == null)
            {
                User newUser = new() { UserName = AdminEmail, Email = AdminEmail, Fullname = "Admin", EmailConfirmed = true, LockoutEnabled = false };
                await userManager.CreateAsync(newUser, "9vBZBB.QH83GeE.");
            }
        }

        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (string roleName in roleNames)
            {
                bool roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task SeedAdminRoles(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            User? userAdmin = await userManager.FindByNameAsync(AdminEmail);

            if (userAdmin == null)
            {
                string messageLog = $"The admin user with the email {AdminEmail} was not found.";
                logger.LogError(messageLog);
                return;
            }

            IdentityResult result = await userManager.AddToRolesAsync(userAdmin, roleNames);
            if (result.Succeeded)
            {
                logger.LogInformation("Les rôles ont été assignés avec succès.");
            }
            else
            {
                logger.LogInformation("Erreur lors de l'assignation des rôles :");
                foreach (IdentityError error in result.Errors)
                {
                    string messageLog = $"- {error.Description}";
                    logger.LogCritical(messageLog);
                }
            }
        }
    }
}