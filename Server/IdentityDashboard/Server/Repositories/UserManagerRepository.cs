using Globe.Identity.AdministrativeDashboard.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Repositories
{
    public class UserManagerRepository : IAsyncUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserManagerRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        async public Task<IEnumerable<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        async public Task<ApplicationUser> FindByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        async public Task<IEnumerable<ApplicationUser>> GetAsync(Expression<Func<ApplicationUser, bool>> filter = null, Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderBy = null)
        {
            return await _userManager.Users.ToListAsync();
        }

        async public Task InsertAsync(ApplicationUser entity)
        {
            var result = await _userManager.CreateAsync(entity);
            if (!result.Succeeded)
                throw new ArgumentException("Impossible to create the user", nameof(entity));

            await Task.CompletedTask;
        }

        async public Task UpdateAsync(ApplicationUser entity)
        {
            var user = await _userManager.FindByIdAsync(entity.Id);
            if (user == null)
                throw new ArgumentNullException("User doesn't exist for updating", nameof(user));

            user.LastName = entity.LastName;
            user.FirstName = entity.FirstName;
            user.UserName = entity.UserName;
            user.Email = entity.Email;

            var entityRoleIds = entity.Roles.Select(role => role.RoleId);
            var roles = _roleManager.Roles.Where(role => entityRoleIds.Contains(role.Id)).Select(role => role.Name).ToList();

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (string role in roles.Except(userRoles))
                await _userManager.AddToRoleAsync(user, role);

            foreach (string role in userRoles.Except(roles))
                await _userManager.RemoveFromRoleAsync(user, role);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new ArgumentException("Impossible to update the user", nameof(entity));

            await Task.CompletedTask; 
        }

        async public Task DeleteAsync(ApplicationUser entity)
        {
            var result = await _userManager.DeleteAsync(entity);
            if (!result.Succeeded)
                throw new ArgumentException("Impossible to delete the user", nameof(entity));

            await Task.CompletedTask;
        }
    }
}
