using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Controllers
{
    [Route("api/read/[controller]")]
    public class ComponentNamespaceController : Controller
    {
        private readonly IComponentNamespaceService _componentConceptsService;

        public ComponentNamespaceController(IComponentNamespaceService componentConceptsService)
        {
            _componentConceptsService = componentConceptsService;
        }

        [HttpGet]
        async public Task<IEnumerable<ComponentNamespace>> Get()
        {
            return await _componentConceptsService.GetAllAsync();
        }
    }
}
