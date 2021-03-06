﻿using AutoMapper;
using MyLabLocalizer.IdentityDashboard.Server.Models;
using MyLabLocalizer.IdentityDashboard.Server.Repositories;
using MyLabLocalizer.IdentityDashboard.Server.UnitOfWorks;
using MyLabLocalizer.IdentityDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Server.Services
{
    public class UserService : IAsyncUserService
    {
        private readonly IMapper _mapper;
        private readonly IAsyncUserUnitOfWork _userUnitOfWork;
        private readonly IAsyncRoleRepository _roleRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IMapper mapper, IAsyncUserUnitOfWork userUnitOfWork, IAsyncRoleRepository roleRepository, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userUnitOfWork = userUnitOfWork;
            _roleRepository = roleRepository;
            _userManager = userManager;
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

        async public Task<IEnumerable<ApplicationUserDTO>> FindByLanguageAsync(LanguageDTO language)
        {
            List<ApplicationUserDTO> users = new List<ApplicationUserDTO>();
            var allUsers = await _userUnitOfWork.UserRepository.GetAsync();
            foreach (var user in allUsers)
            {
                
                if (await _userManager.IsInRoleAsync(user, "MasterTranslator") ||
                    await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    users.Add(new ApplicationUserDTO
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Email = user.Email
                    });
                }
                else
                {
                    bool isInRole = false;
                    if (await _userManager.IsInRoleAsync(user, "TranslatorDE")) 
                        isInRole |= language.IsoCoding == "de";
                    if (await _userManager.IsInRoleAsync(user, "TranslatorIT"))                       
                        isInRole |= language.IsoCoding == "it";
                    if (await _userManager.IsInRoleAsync(user, "TranslatorRU"))                       
                        isInRole |= language.IsoCoding == "ru";
                    if (await _userManager.IsInRoleAsync(user, "TranslatorFR"))                     
                        isInRole |= language.IsoCoding == "fr";
                    if (await _userManager.IsInRoleAsync(user, "TranslatorZH"))                      
                        isInRole |= language.IsoCoding == "zh";
                    if (await _userManager.IsInRoleAsync(user, "TranslatorES"))                      
                        isInRole |= language.IsoCoding == "es";
                    if (await _userManager.IsInRoleAsync(user, "TranslatorPT"))                        
                        isInRole |= language.IsoCoding == "pt";
                    if (isInRole)
                    {
                        users.Add(new ApplicationUserDTO
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            UserName = user.UserName,
                            Email = user.Email
                        });
                    }
                }
            }

            return await Task.FromResult(users);
        }

        async public Task<IEnumerable<ApplicationUserDTO>> FindByUserPermissionAsync(string userName)
        {
            List<ApplicationUserDTO> users = new List<ApplicationUserDTO>();
            var allUsers = await _userUnitOfWork.UserRepository.GetAsync();
            var userHimself = await _userManager.FindByNameAsync(userName);

            if (await _userManager.IsInRoleAsync(userHimself, "MasterTranslator") ||
                    await _userManager.IsInRoleAsync(userHimself, "Admin"))
            {
                foreach (var user in allUsers)
                {
                    users.Add(new ApplicationUserDTO
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Email = user.Email
                    });
                }
            }
            else
            {
                users.Add(new ApplicationUserDTO
                {
                    Id = userHimself.Id,
                    FirstName = userHimself.FirstName,
                    LastName = userHimself.LastName,
                    UserName = userHimself.UserName,
                    Email = userHimself.Email
                });

            }

            return await Task.FromResult(users);
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
