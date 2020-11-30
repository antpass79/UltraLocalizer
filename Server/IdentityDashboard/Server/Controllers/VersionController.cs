using Globe.Identity.AdministrativeDashboard.Server.DTOs;
using Globe.Identity.AdministrativeDashboard.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Controllers
{
    [Route("api/[controller]")]
    public class VersionController : Controller
    {
        private readonly IAsyncVersionService _versionService;
        public VersionController(IAsyncVersionService versionService)
        {
            _versionService = versionService;
        }

        [HttpGet]
        public async Task<VersionDTO> Get()
        {
            return await _versionService.Get();
        }
    }
}