using AutoMapper;
using Globe.Identity.AdministrativeDashboard.Server.Data;
using Globe.Identity.AdministrativeDashboard.Server.Extensions;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.AdministrativeDashboard.Server.Options;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Globe.Identity.Models;
using Globe.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        readonly IAsyncLoginService _loginService;
        private readonly IMapper _mapper;
        ApplicationDbContext context;
        IServiceProvider serviceProvider;

        public LoginController(IAsyncLoginService loginService, IMapper mapper, ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _loginService = loginService;
            _mapper = mapper;

            this.context = context;
            this.serviceProvider = serviceProvider;
        }

        [HttpPost]
        async public Task<LoginResultDTO> Post([FromBody] Credentials credentials)
        {
            await ApplyMigrationsAsync(context);
            await CreateAdminAsync(serviceProvider);

            if (!ModelState.IsValid)
            {
                return new LoginResultDTO
                {
                    Successful = false,
                    Error = "Invalid credentials"
                };
            }

            var mappedCredentials = _mapper.Map<Credentials>(credentials);
            var result = await _loginService.LoginAsync(mappedCredentials);

            return _mapper.Map<LoginResultDTO>(result);
        }

        [HttpDelete]
        [Authorize]
        async public Task Delete()
        {
            await _loginService.LogoutAsync();
        }

        async static Task ApplyMigrationsAsync(ApplicationDbContext dbContext)
        {
            if (dbContext.Database.GetPendingMigrations().Any())
                await dbContext.Database.MigrateAsync();
        }

        async Task CreateAdminAsync(IServiceProvider serviceProvider)
        {
            var userSettingsOptions = serviceProvider.GetRequiredService<IOptions<UserSettingsOptions>>();
            if (!userSettingsOptions.Value.CreateAdmin)
                return;

            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var roles = userSettingsOptions.Value.Roles.ToList();
            IdentityResult roleResult;

            if (roles == null || roles.Count() == 0)
            {
                roles = new List<Role>();
                roles.Add(new Role
                {
                    Name = "admin",
                    Description = "Full Access"
                });
            }

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
