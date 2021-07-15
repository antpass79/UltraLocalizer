using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Shared.Utilities;
using MyLabLocalizer.LocalizationService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Controllers
{
    [Route("api/read/[controller]")]
    public class EditStringViewController : Controller
    {
        private readonly ITranslatedConceptService _translatedConceptService;

        public EditStringViewController(
            ITranslatedConceptService translatedConceptService)
        {
            _translatedConceptService = translatedConceptService;
        }

        [HttpGet]
        async public Task<IEnumerable<LocalizeString>> Get([FromBody] EditStringSearch search)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("search");
            }
            
            return await _translatedConceptService.GetAllStringsAsync(SharedConstants.COMPONENT_NAMESPACE_ALL, SharedConstants.INTERNAL_NAMESPACE_ALL, search.LanguageId, SharedConstants.CONTEXT_ALL, search.Concept, search.LocalizedString, search.StringId);
        }
    }
}
