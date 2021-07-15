using MyLabLocalizer.IdentityDashboard.Server.Services;
using MyLabLocalizer.IdentityDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Server.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly IAsyncRoleService _asyncRoleManager;

        public RoleController(IAsyncRoleService asyncRoleManager)
        {
            _asyncRoleManager = asyncRoleManager;
        }

        [HttpGet]
        [Authorize]
        async public Task<IEnumerable<ApplicationRoleDTO>> Get()
        {
            return await _asyncRoleManager.GetAsync();
        }

        [HttpGet("{roleId}")]
        [Authorize(Roles = "Admin")]
        async public Task<ApplicationRoleDTO> Get(string roleId)
        {
            return await _asyncRoleManager.FindByIdAsync(roleId);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        async public Task Post([FromBody] ApplicationRoleDTO role)
        {
            await _asyncRoleManager.InsertAsync(role);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        async public Task Put([FromBody] ApplicationRoleDTO role)
        {
            await _asyncRoleManager.UpdateAsync(role);
        }

        [HttpDelete("{roleId}")]
        [Authorize(Roles = "Admin")]
        async public Task Delete(string roleId)
        {
            await _asyncRoleManager.DeleteAsync(roleId);
        }
    }
}
