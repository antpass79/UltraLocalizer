using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Controllers
{
    [Route("api/read/[controller]")]
    public class ComponentNamespaceGroupController : Controller
    {
        private readonly IComponentNamespaceGroupService _componentNamespaceGroupService;

        public ComponentNamespaceGroupController(IComponentNamespaceGroupService componentNamespaceGroupService)
        {
            _componentNamespaceGroupService = componentNamespaceGroupService;
        }

        [HttpGet]
        async public Task<IEnumerable<ComponentNamespaceGroup>> Get([FromBody] Language language)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("search");
            }
            
            return await _componentNamespaceGroupService.GetAllAsync(language);
        }
    }
}
