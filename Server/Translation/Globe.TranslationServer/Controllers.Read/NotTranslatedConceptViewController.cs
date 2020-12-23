using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
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
