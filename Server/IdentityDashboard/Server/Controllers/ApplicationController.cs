using MyLabLocalizer.IdentityDashboard.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace MyLabLocalizer.IdentityDashboard.Server.Controllers
{
    [Route("api/[controller]")]
    public class ApplicationController : Controller
    {
        private readonly IAsyncApplicationService _applicationService;
        public ApplicationController(IAsyncApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            var zip = _applicationService.Zip();
            return File((zip as MemoryStream).ToArray(), "application/zip", "UltraLocalizer.zip");
        }
    }
}