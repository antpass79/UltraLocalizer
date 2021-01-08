using Globe.Shared.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
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

