using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Repositories
{
    public class MockRoleRepository : IAsyncRoleRepository
    {
        List<ApplicationRole> roles = new List<ApplicationRole>();

        public MockRoleRepository()
        {
            for (int i = 0; i < 3; i++)
            {
                roles.Add(new ApplicationRole
                {
                    Id = Guid.NewGuid().ToString() + "-" + i.ToString(),
                    Name = "Role " + i.ToString(),
                    Description = "Description " + i.ToString()
                });
            }
        }

        async public Task<ApplicationRole> FindByIdAsync(string id)
        {
            return await Task.FromResult(roles.SingleOrDefault(user => user.Id == id));
        }

        async public Task<IEnumerable<ApplicationRole>> GetAsync(Expression<Func<ApplicationRole, bool>> filter = null, Func<IQueryable<ApplicationRole>, IOrderedQueryable<ApplicationRole>> orderBy = null)
        {
            var selectedRoles = new List<ApplicationRole>(roles);

            if (filter != null)
                selectedRoles = roles.Where(role => filter.Compile()(role)).ToList();

            if (orderBy != null)
                selectedRoles = orderBy(roles.AsQueryable()).ToList();

            return await Task.FromResult(selectedRoles);
        }

        async public Task InsertAsync(ApplicationRole entity)
        {
            entity.Id = Guid.NewGuid().ToString() + "-" + roles.Count.ToString();
            roles.Add(entity);
            await Task.CompletedTask;
        }

        async public Task UpdateAsync(ApplicationRole entity)
        {
            var role = await FindByIdAsync(entity.Id);
            if (role == null)
                throw new ArgumentException("Role doesn't exist for updating", nameof(entity));
            else
            {
                roles.Remove(role);
                roles.Add(entity);
            }

            await Task.CompletedTask;
        }

        async public Task DeleteAsync(ApplicationRole entity)
        {
            var role = await FindByIdAsync(entity.Id);
            roles.Remove(role);
            await Task.CompletedTask;
        }
    }
}
