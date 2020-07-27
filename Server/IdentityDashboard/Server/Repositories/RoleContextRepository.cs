using Globe.BusinessLogic;
using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Data;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Repositories
{
    public class RoleContextRepository : IAsyncRoleRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<ApplicationRole> DbSet => _context.Set<ApplicationRole>();

        public RoleContextRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        async public Task<ApplicationRole> FindByIdAsync(string id)
        {
            return await this.DbSet.FindAsync(id);
        }

        async public Task<IEnumerable<ApplicationRole>> GetAsync(Expression<Func<ApplicationRole, bool>> filter = null, Func<IQueryable<ApplicationRole>, IOrderedQueryable<ApplicationRole>> orderBy = null)
        {
            var filtered = filter != null ? this.DbSet.Where(filter).AsQueryable() : this.DbSet.AsQueryable();
            var ordered = orderBy != null ? orderBy(filtered.AsNoTracking()) : filtered.AsNoTracking();

            return await ordered.ToListAsync();
        }

        async public Task InsertAsync(ApplicationRole entity)
        {
            await this.DbSet.AddAsync(entity);
        }

        async public Task UpdateAsync(ApplicationRole entity)
        {
            var role = await this.FindByIdAsync(entity.Id);
            if (role == null)
                throw new ArgumentNullException("Role doesn't exist for updating", nameof(role));

            role.Name = entity.Name;
            role.Description = entity.Description;

            this.DbSet.Update(role);
            await Task.CompletedTask;
        }

        async public Task DeleteAsync(ApplicationRole entity)
        {
            this.DbSet.Remove(entity);
            await Task.CompletedTask;
        }
    }
}
