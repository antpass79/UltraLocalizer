using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Services
{
    public interface IAsyncUserService
    {
        Task<UserWithRoles> FindByIdAsync(string id);
        Task<IEnumerable<ApplicationUserDTO>> GetAsync(Expression<Func<ApplicationUserDTO, bool>> filter = null, Func<IQueryable<ApplicationUserDTO>, IOrderedQueryable<ApplicationUserDTO>> orderBy = null);
        Task InsertAsync(UserWithRoles entity);
        Task UpdateAsync(UserWithRoles entity);
        Task DeleteAsync(ApplicationUserDTO entity);
        Task DeleteAsync(string id);
    }
}
