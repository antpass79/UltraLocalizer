using Globe.Shared.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class LanguageController : Controller
    {
        private readonly IAsyncLanguageService _languageService;

        public LanguageController(IAsyncLanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpGet]
        async public Task<IEnumerable<Language>> Get()
        {
            return await _languageService.GetAllAsync();
        }
    }
}
