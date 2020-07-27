using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.AdministrativeDashboard.Server.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Extensions
{
    public static class IServiceProviderExtensions
    {
        async public static Task CreateAdminAsync(this IServiceProvider serviceProvider)
        {
            var userSettingsOptions = serviceProvider.GetRequiredService<IOptions<UserSettingsOptions>>();
            if (!userSettingsOptions.Value.CreateAdmin)
                return;

            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var roles = userSettingsOptions.Value.Roles;
            IdentityResult roleResult;

            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role.Name);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = role.Name,
                        Description = role.Description
                    });
                }
            }

            var powerUser = new ApplicationUser
            {
                UserName = userSettingsOptions.Value.UserName,
            };

            string userPassword = userSettingsOptions.Value.UserPassword;
            var user = await userManager.FindByNameAsync(userSettingsOptions.Value.UserName);
            if (user == null)
            {
                var createPowerUser = await userManager.CreateAsync(powerUser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    foreach (var role in userSettingsOptions.Value.Roles)
                        await userManager.AddToRoleAsync(powerUser, role.Name);
                }
            }
            else
            {
                foreach (var role in userSettingsOptions.Value.Roles)
                {
                    if (!(await userManager.IsInRoleAsync(user, role.Name)))
                        await userManager.AddToRoleAsync(powerUser, role.Name);
                }
            }
        }
    }
}
