using MyLabLocalizer.IdentityDashboard.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Server.Services
{
    public interface IAsyncRoleService
    {
        Task<ApplicationRoleDTO> FindByIdAsync(string id);
        Task<IEnumerable<ApplicationRoleDTO>> GetAsync(Expression<Func<ApplicationRoleDTO, bool>> filter = null, Func<IQueryable<ApplicationRoleDTO>, IOrderedQueryable<ApplicationUserDTO>> orderBy = null);
        Task InsertAsync(ApplicationRoleDTO entity);
        Task UpdateAsync(ApplicationRoleDTO entity);
        Task DeleteAsync(ApplicationRoleDTO entity);
        Task DeleteAsync(string id);
    }
}
