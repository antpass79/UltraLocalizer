using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
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
        async public Task<IEnumerable<InternalNamespaceDTO>> Get([FromQuery] string componentNamespace)
        {
            return await _internalConceptsService.GetAllAsync(componentNamespace);
        }
    }
}

