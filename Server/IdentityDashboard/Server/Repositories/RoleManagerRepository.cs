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
    public class RoleManagerRepository : IAsyncRoleRepository
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleManagerRepository(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        async public Task<ApplicationRole> FindByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        async public Task<IEnumerable<ApplicationRole>> GetAsync(Expression<Func<ApplicationRole, bool>> filter = null, Func<IQueryable<ApplicationRole>, IOrderedQueryable<ApplicationRole>> orderBy = null)
        {
            return await _roleManager.Roles.ToListAsync();
        }

        async public Task InsertAsync(ApplicationRole entity)
        {
            var result = await _roleManager.CreateAsync(entity);
            if (!result.Succeeded)
                throw new ArgumentException("Impossible to create the role", nameof(entity));

            await Task.CompletedTask;
        }

        async public Task UpdateAsync(ApplicationRole entity)
        {
            var role = await _roleManager.FindByIdAsync(entity.Id);
            if (role == null)
                throw new ArgumentNullException("Role doesn't exist for updating", nameof(role));

            role.Name = entity.Name;
            role.Description = entity.Description;

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
                throw new ArgumentException("Impossible to update the role", nameof(entity));

            await Task.CompletedTask; 
        }

        async public Task DeleteAsync(ApplicationRole entity)
        {
            var result = await _roleManager.DeleteAsync(entity);
            if (!result.Succeeded)
                throw new ArgumentException("Impossible to delete the role", nameof(entity));

            await Task.CompletedTask;
        }
    }
}
