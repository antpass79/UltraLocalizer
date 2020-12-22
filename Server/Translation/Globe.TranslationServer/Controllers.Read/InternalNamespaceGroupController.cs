using Globe.Shared.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class InternalNamespaceGroupController : Controller
    {
        private readonly IAsyncInternalNamespaceGroupService _internalNamespaceGroupService;

        public InternalNamespaceGroupController(
            IAsyncInternalNamespaceGroupService internalNamespaceGroupService)
        {
            _internalNamespaceGroupService = internalNamespaceGroupService;
        }

        [HttpGet]
        async public Task<IEnumerable<InternalNamespaceGroup>> Get([FromBody] Language language)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("search");
            }
            
            return await _internalNamespaceGroupService.GetAllAsync(language);
        }
    }
}
