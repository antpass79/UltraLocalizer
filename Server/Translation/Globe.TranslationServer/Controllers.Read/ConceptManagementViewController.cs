using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class ConceptManagementViewController : Controller
    {
        private readonly IConceptTranslatedService _conceptTranslatedService;

        public ConceptManagementViewController(
            IConceptTranslatedService conceptTranslatedService)
        {
            _conceptTranslatedService = conceptTranslatedService;
        }

        [HttpGet]
        async public Task<IEnumerable<ConceptTranslated>> Get([FromBody] ConceptManagementSearch search)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("search");
            }
            
            return await _conceptTranslatedService.GetAllAsync(search.ComponentNamespace, search.InternalNamespace, search.LanguageId, search.Context, search.Concept, search.LocalizedString);
        }
    }
}
