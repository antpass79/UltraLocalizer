using MyLabLocalizer.IdentityDashboard.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Server.Controllers
{
    [Route("api/[controller]")]
    public class StyleController : Controller
    {
        private readonly IAsyncStyleService _styleService;
        public StyleController(IAsyncStyleService styleService)
        {
            _styleService = styleService;
        }

        [HttpGet]
        public async Task<string> Get(string filePath)
        {
            return await _styleService.GetFileContent(filePath);
        }
    }
}