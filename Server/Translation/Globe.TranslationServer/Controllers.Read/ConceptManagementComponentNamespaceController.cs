using Globe.Shared.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
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
