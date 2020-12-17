using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class ConceptViewController : Controller
    {
        private readonly IAsyncGroupedStringEntityService _groupedStringEntityService;

        public ConceptViewController(
            IAsyncGroupedStringEntityService groupedStringEntityService)
        {
            _groupedStringEntityService = groupedStringEntityService;
        }

        [HttpGet]
        async public Task<IEnumerable<ConceptViewDTO>> Get([FromBody] ConceptViewSearchDTO search)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("search");
            }
            
            return await _groupedStringEntityService.GetAllAsync(search.ComponentNamespace, search.InternalNamespace, search.LanguageId, search.JobListId);
        }
    }
}
