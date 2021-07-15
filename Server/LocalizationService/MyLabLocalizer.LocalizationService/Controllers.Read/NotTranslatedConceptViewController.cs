using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.DTOs;
using MyLabLocalizer.LocalizationService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Controllers
{
    [Route("api/read/[controller]")]
    public class NotTranslatedConceptViewController : Controller
    {
        private readonly IAsyncNotTranslatedConceptViewService _notTranslatedConceptViewService;

        public NotTranslatedConceptViewController(
            IAsyncNotTranslatedConceptViewService notTranslatedConceptViewService)
        {
            _notTranslatedConceptViewService = notTranslatedConceptViewService;
        }

        [HttpGet]
        async public Task<IEnumerable<NotTranslatedConceptView>> Get([FromBody] NotTranslatedConceptViewSearch search)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("search");
            }
            
            return await _notTranslatedConceptViewService.GetAllAsync(search);
        }
    }
}
