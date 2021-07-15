using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Controllers
{
    [Route("api/read/[controller]")]
    public class ConceptManagementInternalNamespaceController : Controller
    {
        private readonly IAsyncConceptTranslatedInternalNamespaceService _conceptTranslatedInternalNamespaceService;

        public ConceptManagementInternalNamespaceController(IAsyncConceptTranslatedInternalNamespaceService conceptTranslatedInternalNamespaceService)
        {
            _conceptTranslatedInternalNamespaceService = conceptTranslatedInternalNamespaceService;
        }

        [HttpGet]
        async public Task<IEnumerable<InternalNamespace>> Get([FromQuery] string componentNamespace)
        {
            return await _conceptTranslatedInternalNamespaceService.GetAllAsync(componentNamespace);
        }
    }
}

