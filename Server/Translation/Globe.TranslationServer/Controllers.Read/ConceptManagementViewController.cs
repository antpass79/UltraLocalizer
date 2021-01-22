using Globe.Shared.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class ConceptManagementViewController : Controller
    {
        private readonly ITranslatedConceptService _translatedConceptService;

        public ConceptManagementViewController(
            ITranslatedConceptService translatedConceptService)
        {
            _translatedConceptService = translatedConceptService;
        }

        [HttpGet]
        async public Task<IEnumerable<TranslatedConcept>> Get([FromBody] ConceptManagementSearch search)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("search");
            }
            
            return await _translatedConceptService.GetAllAsync(search.ComponentNamespace, search.InternalNamespace, search.LanguageId, search.Context, search.Concept, search.LocalizedString);
        }
    }
}
