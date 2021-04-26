using Globe.Shared.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class TranslatedComponentNamespaceGroupController : Controller
    {
        private readonly IComponentNamespaceGroupService _componentNamespaceGroupService;

        public TranslatedComponentNamespaceGroupController(IComponentNamespaceGroupService componentNamespaceGroupService)
        {
            _componentNamespaceGroupService = componentNamespaceGroupService;
        }

        [HttpGet]
        async public Task<IEnumerable<ComponentNamespaceGroup>> Get()
        {
            return await _componentNamespaceGroupService.GetAllAsync();
        }
    }
}
