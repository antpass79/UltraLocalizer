using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Repositories;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/[controller]")]
    public class LocalizableStringController : Controller
    {
        private readonly IAsyncLocalizableStringService _localizableStringService;

        public LocalizableStringRepository LocalizableStringRepository { get; }

        public LocalizableStringController(IAsyncLocalizableStringService localizableStringService)
        {
            _localizableStringService = localizableStringService;
        }

        [HttpGet]
        [Authorize]
        async public Task<IEnumerable<LocalizableString>> Get()
        {
            return await _localizableStringService.GetAllAsync();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        async public Task<IActionResult> Put([FromBody] IEnumerable<LocalizableString> strings)
        {
            await _localizableStringService.SaveAsync(strings);
            return await Task.FromResult(Ok());
        }
    }
}
