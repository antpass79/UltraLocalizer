using AutoMapper;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.AdministrativeDashboard.Server.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.UnitOfWorks;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Services
{
    public class UserService : IAsyncUserService
    {
        private readonly IMapper _mapper;
        private readonly IAsyncUserUnitOfWork _userUnitOfWork;
        private readonly IAsyncRoleRepository _roleRepository;

        public UserService(IMapper mapper, IAsyncUserUnitOfWork userUnitOfWork, IAsyncRoleRepository roleRepository)
        {
            _mapper = mapper;
            _userUnitOfWork = userUnitOfWork;
            _roleRepository = roleRepository;
        }

        async public Task DeleteAsync(ApplicationUserDTO entity)
        {
            var mappedUser = _mapper.Map<ApplicationUser>(entity);

            await _userUnitOfWork.UserRepository.DeleteAsync(mappedUser);
            await _userUnitOfWork.SaveAsync();
        }

        async public Task DeleteAsync(string id)
        {
            var user = await _userUnitOfWork.UserRepository.FindByIdAsync(id);
            var mappedUser = _mapper.Map<ApplicationUser>(user);

            await _userUnitOfWork.UserRepository.DeleteAsync(mappedUser);
            await _userUnitOfWork.SaveAsync();
        }

        async public Task<UserWithRoles> FindByIdAsync(string id)
        {
            var user = await _userUnitOfWork.UserRepository.FindByIdAsync(id);

            return new UserWithRoles
            {
                User = _mapper.Map<ApplicationUserDTO>(user),
                Roles = await GetUserRolesAsync(user)
            };
        }

        async public Task<IEnumerable<ApplicationUserDTO>> GetAsync(Expression<Func<ApplicationUserDTO, bool>> filter = null, Func<IQueryable<ApplicationUserDTO>, IOrderedQueryable<ApplicationUserDTO>> orderBy = null)
        {
            return _mapper.Map<IEnumerable<ApplicationUserDTO>>(await _userUnitOfWork.UserRepository.GetAsync());
        }

        async public Task InsertAsync(UserWithRoles entity)
        {
            entity.User.Id = Guid.NewGuid().ToString();

            var mappedUser = _mapper.Map<ApplicationUser>(entity.User);
            var mappedRoles = _mapper.Map<IEnumerable<ApplicationRole>>(entity.Roles);

            var roles = mappedRoles.Select(role => role.Id);
            foreach (var role in roles)
            {
                mappedUser.Roles.Add(new IdentityUserRole<string>
                {
                    UserId = entity.User.Id,
                    RoleId = role
                });
            }

            await _userUnitOfWork.UserRepository.InsertAsync(mappedUser);
            await _userUnitOfWork.SaveAsync();
        }

        async public Task UpdateAsync(UserWithRoles entity)
        {
            var mappedUser = _mapper.Map<ApplicationUser>(entity.User);

            var mappedRoles = _mapper.Map<IEnumerable<ApplicationRole>>(entity.Roles);
            var mappedRoleIds = mappedRoles.Select(role => role.Id);

            var userRoles = await GetUserRolesAsync(mappedUser);
            var userRoleIds = userRoles.Select(role => role.Id);

            foreach (string role in userRoleIds.Except(mappedRoleIds))
            {
                var identityRole = mappedUser.Roles.ToList().Find(item => item.RoleId == role);
                mappedUser.Roles.Remove(identityRole);
            }

            mappedUser.Roles.Clear();
            foreach (var role in mappedRoleIds)
            {
                mappedUser.Roles.Add(new IdentityUserRole<string>
                {
                    UserId = entity.User.Id,
                    RoleId = role
                });
            }

            await _userUnitOfWork.UserRepository.UpdateAsync(mappedUser);
            await _userUnitOfWork.SaveAsync();
        }

        async private Task<IEnumerable<ApplicationRoleDTO>> GetUserRolesAsync(ApplicationUser user)
        {
            var userRoleNames = await _userUnitOfWork.UserRepository.GetRolesAsync(user);
            var allRoles = await _roleRepository.GetAsync();

            var userRoles = allRoles
                .Where(role => userRoleNames.Contains(role.Name))
                .Select(role =>
                new ApplicationRoleDTO
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description
                });

            return userRoles;
        }
    }
}
