using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
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
