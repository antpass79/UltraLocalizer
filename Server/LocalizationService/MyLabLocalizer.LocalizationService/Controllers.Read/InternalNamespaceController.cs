using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Controllers
{
    [Route("api/read/[controller]")]
    public class InternalNamespaceController : Controller
    {
        private readonly IAsyncInternalNamespaceService _internalConceptsService;

        public InternalNamespaceController(IAsyncInternalNamespaceService internalConceptsService)
        {
            _internalConceptsService = internalConceptsService;
        }

        [HttpGet]
        async public Task<IEnumerable<InternalNamespace>> Get([FromQuery] string componentNamespace)
        {
            return await _internalConceptsService.GetAllAsync(componentNamespace);
        }
    }
}

