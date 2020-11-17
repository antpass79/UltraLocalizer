using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class LanguageController : Controller
    {
        private readonly IAsyncLanguageService _languageService;
        private readonly IAsyncNotificationService _fakeNotificationService;

        public LanguageController(IAsyncLanguageService languageService, IAsyncNotificationService fakeNotificationService)
        {
            _languageService = languageService;
            _fakeNotificationService = fakeNotificationService;
        }

        [HttpGet]
        async public Task<IEnumerable<LanguageDTO>> Get()
        {
            await _fakeNotificationService.JoblistChanged("Bravo!");
            await _fakeNotificationService.ConceptsChanged(10);
            return await _languageService.GetAllAsync();
        }
    }
}
