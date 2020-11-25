using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
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