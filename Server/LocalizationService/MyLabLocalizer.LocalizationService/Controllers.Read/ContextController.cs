using MyLabLocalizer.LocalizationService.DTOs;
using MyLabLocalizer.LocalizationService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Controllers
{
    [Route("api/read/[controller]")]
    public class ContextController : Controller
    {
        private readonly IAsyncContextService _contextService;

        public ContextController(IAsyncContextService contextService)
        {
            _contextService = contextService;
        }

        [HttpGet]
        async public Task<IEnumerable<ContextDTO>> Get()
        {
            return await _contextService.GetAllAsync();
        }
    }
}
