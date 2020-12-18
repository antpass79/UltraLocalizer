using Globe.Shared.DTOs;
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
        private readonly IJobListService _groupedStringEntityService;

        public ConceptViewController(
            IJobListService groupedStringEntityService)
        {
            _groupedStringEntityService = groupedStringEntityService;
        }

        [HttpGet]
        async public Task<IEnumerable<JobListConcept>> Get([FromBody] JobListConceptSearch search)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("search");
            }
            
            return await _groupedStringEntityService.GetAllAsync(search.ComponentNamespace, search.InternalNamespace, search.LanguageId, search.JobListId);
        }
    }
}
