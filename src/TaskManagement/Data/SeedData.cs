namespace TaskManagement.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;

    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Models.ApplicationUser>>();

            // Create roles
            await CreateRoles(roleManager);

            // Create users
            await CreateUsers(userManager);
        }

        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task CreateUsers(UserManager<Models.ApplicationUser> userManager)
        {
            var users = new[]
            {
            new Models.ApplicationUser { UserName = "admin"},
            new Models.ApplicationUser { UserName = "user" }
            };

            foreach (var user in users)
            {
                if (userManager.FindByNameAsync(user.UserName).Result == null)
                {
                    await userManager.CreateAsync(user, "Pass@123"); // Set the password here
                    //await userManager.AddToRoleAsync(user, "User"); // Assign a role to the user

                    await userManager.AddToRoleAsync(user, user.UserName == "user" ? "User" : "Admin"); // Assign a role to the user
                }
            }
        }

        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    Initialize(services).Wait();
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during seed data initialization
                }
            }
        }
    }

}
