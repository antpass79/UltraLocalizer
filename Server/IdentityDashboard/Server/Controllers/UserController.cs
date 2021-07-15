using MyLabLocalizer.IdentityDashboard.Server.Services;
using MyLabLocalizer.IdentityDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Server.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IAsyncUserService _userService;

        public UserController(IAsyncUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        async public Task<IEnumerable<ApplicationUserDTO>> Get()
        {
            return await _userService.GetAsync();
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin, UserManager")]
        async public Task<UserWithRoles> Get(string userId)
        {
            return await _userService.FindByIdAsync(userId);
        }

        [HttpGet]
        [Authorize]
        [Route("GetUserByLanguage")]
        async public Task<IEnumerable<ApplicationUserDTO>> GetUserByLanguage([FromBody] LanguageDTO language)
        {
            return await _userService.FindByLanguageAsync(language);
        }

        [HttpGet]
        [Authorize]
        [Route("GetUserByPermission")]
        async public Task<IEnumerable<ApplicationUserDTO>> GetUserByPermission([FromBody] string userName)
        {
            return await _userService.FindByUserPermissionAsync(userName);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, UserManager")]
        async public Task Post([FromBody] UserWithRoles userWithRoles)
        {
            await _userService.InsertAsync(userWithRoles);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, UserManager")]
        async public Task Put([FromBody] UserWithRoles userWithRoles)
        {
            await _userService.UpdateAsync(userWithRoles);
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin, UserManager")]
        async public Task Delete(string userId)
        {
            await _userService.DeleteAsync(userId);
        }
    }
}
