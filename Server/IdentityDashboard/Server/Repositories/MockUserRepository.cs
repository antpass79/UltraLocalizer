using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Repositories
{
    public class MockUserRepository : IAsyncUserRepository
    {
        List<ApplicationUser> users = new List<ApplicationUser>();
        private readonly IAsyncRoleRepository _roleRepository;

        public MockUserRepository(IAsyncRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;

            for (int i = 0; i < 3; i++)
            {
                users.Add(new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString() + "-" + i.ToString(),
                    FirstName = "First Name " + i.ToString(),
                    LastName = "Last Name " + i.ToString(),
                    UserName = "User Name " + i.ToString(),
                    Email = $"email{i}@email.com"
                });
            }
        }

        async public Task<IEnumerable<string>> GetRolesAsync(ApplicationUser user)
        {
            var roles = await _roleRepository.GetAsync();
            var userRoleIds = user.Roles.Select(role => role.RoleId);

            return await Task.FromResult(roles.Where(role => userRoleIds.Contains(role.Id)).Select(role => role.Name));
        }
        
        async public Task<ApplicationUser> FindByIdAsync(string id)
        {
            return await Task.FromResult(users.SingleOrDefault(user => user.Id == id));
        }

        async public Task<IEnumerable<ApplicationUser>> GetAsync(Expression<Func<ApplicationUser, bool>> filter = null, Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderBy = null)
        {
            var selectedUsers = new List<ApplicationUser>(users);

            if (filter != null)
                selectedUsers = users.Where(user => filter.Compile()(user)).ToList();

            if (orderBy != null)
                selectedUsers = orderBy(users.AsQueryable()).ToList();

            return  await Task.FromResult(selectedUsers);
        }

        async public Task InsertAsync(ApplicationUser entity)
        {
            entity.Id = Guid.NewGuid().ToString() + "-" + users.Count.ToString();
            users.Add(entity);
            await Task.CompletedTask;
        }

        async public Task UpdateAsync(ApplicationUser entity)
        {
            var user = await FindByIdAsync(entity.Id);
            if (user == null)
                throw new ArgumentException("User doesn't exist for updating", nameof(entity));
            else
            {
                users.Remove(user);
                users.Add(entity);
            }
            await Task.CompletedTask;
        }

        async public Task DeleteAsync(ApplicationUser entity)
        {
            var user = await FindByIdAsync(entity.Id);
            users.Remove(user);
            await Task.CompletedTask;
        }
    }
}
