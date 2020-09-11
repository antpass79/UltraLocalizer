using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class ComponentNamespaceController : Controller
    {
        private readonly IAsyncComponentNamespaceService _componentConceptsService;

        public ComponentNamespaceController(IAsyncComponentNamespaceService componentConceptsService)
        {
            _componentConceptsService = componentConceptsService;
        }

        [HttpGet]
        async public Task<IEnumerable<ComponentNamespaceDTO>> Get()
        {
            return await _componentConceptsService.GetAllAsync();
        }
    }
}
