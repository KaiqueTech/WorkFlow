using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WorkFlow.Domain.Enuns;
using WorkFlow.Domain.Models;

public static class SeedData
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUserModel>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var roleName in Enum.GetNames(typeof(UserRoleEnum)))
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var managerEmail = "manager@deal.com";
        var managerRole = UserRoleEnum.Manager.ToString();

        var adminUser = await userManager.FindByEmailAsync(managerEmail);

        if (adminUser == null)
        {
            var user = new ApplicationUserModel
            {
                UserName = managerEmail,
                Email = managerEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "Manager@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, managerRole);
            }
        }

        var solicitanteEmail = "user@deal.com";
        var solicitanteRole = UserRoleEnum.User.ToString();

        var solicitanteUser = await userManager.FindByEmailAsync(solicitanteEmail);

        if (solicitanteUser == null)
        {
            var user = new ApplicationUserModel
            {
                UserName = solicitanteEmail,
                Email = solicitanteEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "User@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, solicitanteRole);
            }
        }
    }
}
