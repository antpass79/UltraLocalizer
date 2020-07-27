using AutoMapper;
using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.AdministrativeDashboard.Server.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.UnitOfWorks;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Services
{
    public class RoleService : IAsyncRoleService
    {
        private readonly IMapper _mapper;
        private readonly IAsyncRoleUnitOfWork _roleUnitOfWork;

        public RoleService(IMapper mapper, IAsyncRoleUnitOfWork roleUnitOfWork)
        {
            _mapper = mapper;
            _roleUnitOfWork = roleUnitOfWork;
        }

        async public Task DeleteAsync(ApplicationRoleDTO entity)
        {
            var mappedRole = _mapper.Map<ApplicationRole>(entity);

            await _roleUnitOfWork.RoleRepository.DeleteAsync(mappedRole);
            await _roleUnitOfWork.SaveAsync();
        }

        async public Task DeleteAsync(string id)
        {
            var role = await _roleUnitOfWork.RoleRepository.FindByIdAsync(id);
            var mappedRole = _mapper.Map<ApplicationRole>(role);

            await _roleUnitOfWork.RoleRepository.DeleteAsync(mappedRole);
            await _roleUnitOfWork.SaveAsync();
        }

        async public Task<ApplicationRoleDTO> FindByIdAsync(string id)
        {
            var role = await _roleUnitOfWork.RoleRepository.FindByIdAsync(id);
            return _mapper.Map<ApplicationRoleDTO>(role);
        }

        async public Task<IEnumerable<ApplicationRoleDTO>> GetAsync(Expression<Func<ApplicationRoleDTO, bool>> filter = null, Func<IQueryable<ApplicationRoleDTO>, IOrderedQueryable<ApplicationUserDTO>> orderBy = null)
        {
            var result = await _roleUnitOfWork.RoleRepository.GetAsync();
            return _mapper.Map<IEnumerable<ApplicationRoleDTO>>(result);
        }

        async public Task InsertAsync(ApplicationRoleDTO entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            var mappedRole = _mapper.Map<ApplicationRole>(entity);

            await _roleUnitOfWork.RoleRepository.InsertAsync(mappedRole);
            await _roleUnitOfWork.SaveAsync();
        }

        async public Task UpdateAsync(ApplicationRoleDTO entity)
        {
            var mappedRole = _mapper.Map<ApplicationRole>(entity);

            await _roleUnitOfWork.RoleRepository.UpdateAsync(mappedRole);
            await _roleUnitOfWork.SaveAsync();
        }
    }
}
