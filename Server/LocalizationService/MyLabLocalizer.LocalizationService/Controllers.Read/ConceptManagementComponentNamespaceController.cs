using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Controllers
{
    [Route("api/read/[controller]")]
    public class ConceptManagementComponentNamespaceController : Controller
    {
        private readonly IAsyncConceptTranslatedComponentNamespaceService _conceptTranslatedComponentNamespaceService;

        public ConceptManagementComponentNamespaceController(IAsyncConceptTranslatedComponentNamespaceService conceptTranslatedComponentNamespaceService)
        {
            _conceptTranslatedComponentNamespaceService = conceptTranslatedComponentNamespaceService;
        }

        [HttpGet]
        async public Task<IEnumerable<ComponentNamespace>> Get()
        {
            return await _conceptTranslatedComponentNamespaceService.GetAllAsync();
        }
    }
}
